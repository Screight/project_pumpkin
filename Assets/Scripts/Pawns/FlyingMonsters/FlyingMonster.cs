using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonster : Enemy
{
    enum ENEMY_STATE { PATROL, CHASE, ATTACK, IDLE, RETURN, HIT, DEAD, REPOSITION, PREPARE_ATTACK }
    enum ANIMATION_STATE { MOVE, PREPARE_ATTACK, ATTACK, RECOVER_FROM_ATTACK, HIT, DIE, LAST_NO_USE}

    string m_moveAnimationName      = "move";
    string m_prepareAttackAnimationName    = "prepare_attack";
    string m_attackAnimationName      = "attack";
    string m_recoverFromAttackAnimationName       = "recover_from_attack";
    string m_hitAnimationName = "hit";
    string m_dieAnimationName       = "die";

    int[] m_animationHash = new int[(int)ANIMATION_STATE.LAST_NO_USE];


    [SerializeField] LayerMask m_obstacleLayer;

    [SerializeField] float m_speed = 40;
    [SerializeField] float m_chargeSpeed = 100;
    Rigidbody2D m_rb2D;

    Timer m_restTimer;
    [SerializeField] float m_timeToWaitBetweenAttacks = 1;

    Timer m_memoryTimer;
    [SerializeField] float m_timeToRememberPlayer = 1;

    bool m_isPlayerInEnemyRangeFlag = false;

    [SerializeField] Transform m_patrolPoint_1;
    [SerializeField] Transform m_patrolPoint_2;
    [SerializeField] float m_attackRange = 20;
    [SerializeField] float m_visionRange = 60;

    [SerializeField] float m_minHeightReposition = 5;
    [SerializeField] float m_maxHeightReposition = 15;
    float m_minAngleReposition;
    float m_maxAngleReposition;

    bool m_isFacingRight;
    ENEMY_STATE m_state;
    [SerializeField] GameObject m_player;
    Player m_playerScript;
    PathFinderTest m_pathFinder;
    bool m_isGoingFrom1To2;
    bool m_isCharging;
    bool m_hasCharged = false;
    bool m_isInitialized = false;
    bool m_isRecovering = false;
    Vector2 m_directionToAttack;

    ANIMATION_STATE m_animationState;
    int m_currentAnimationHash;
    Animator m_animator;

    protected override void Awake()
    {
        base.Awake();
        m_rb2D = GetComponent<Rigidbody2D>();
        m_pathFinder = GetComponent<PathFinderTest>();

        m_restTimer = gameObject.AddComponent<Timer>();
        m_memoryTimer = gameObject.AddComponent<Timer>();
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_animator = GetComponent<Animator>();
    }

    void ChangeAnimationState(ANIMATION_STATE p_animationState)
    {
        int newAnimationHash = m_animationHash[(int)p_animationState];

        if (m_currentAnimationHash == newAnimationHash) return;   // stop the same animation from interrupting itself
        m_animator.Play(newAnimationHash);                // play the animation
        m_currentAnimationHash = newAnimationHash;                // reassigning the new state
        m_animationState = p_animationState;
    }

    void InitializePatrol()
    {
        m_pathFinder.SetInitialNode(transform.position);
        //m_pathFinder.SnapToClosestNode();
        m_pathFinder.SetTargetNode(m_patrolPoint_2.position);
        m_isGoingFrom1To2 = true;
        ChangeAnimationState(ANIMATION_STATE.MOVE);
        m_state = ENEMY_STATE.PATROL;
    }

    void Patrol()
    {
        m_pathFinder.NavigateToTargetPosition();
        if (m_pathFinder.IsFinished()) { SwapPatrolTarget(); }
    }

    void SwapPatrolTarget()
    {
        if (m_isGoingFrom1To2)
        {
            m_pathFinder.SetInitialNode(m_patrolPoint_2.position);
            m_pathFinder.SetTargetNode(m_patrolPoint_1.position);
            m_isGoingFrom1To2 = false;
        }
        else
        {
            m_pathFinder.SetInitialNode(m_patrolPoint_1.position);
            m_pathFinder.SetTargetNode(m_patrolPoint_2.position);
            m_isGoingFrom1To2 = true;
        }
    }

    void InitializeChase()
    {
        //m_pathFinder.SnapToClosestNode();
        m_pathFinder.SetInitialNode(transform.position);
        m_pathFinder.SetTargetNode(m_player.transform.position);
        ChangeAnimationState(ANIMATION_STATE.MOVE);
    }

    void Chase()
    {
        m_pathFinder.SetTargetNode(m_player.transform.position);
        m_pathFinder.NavigateToTargetPosition();
    }

    void InitializePrepareAttack(){
        ChangeAnimationState(ANIMATION_STATE.PREPARE_ATTACK);
        m_directionToAttack = GetDirection(m_player.transform.position, transform.position);
    }

    void InitializeAttack()
    {
        ChangeAnimationState(ANIMATION_STATE.PREPARE_ATTACK);
        FaceToPosition(Player.Instance.transform.position.x);
    }

    void Attack()
    {
        m_isCharging = true;
        m_rb2D.velocity = m_chargeSpeed * m_directionToAttack;
        m_hasCharged = true;
        ChangeAnimationState(ANIMATION_STATE.ATTACK);
        m_state = ENEMY_STATE.ATTACK;
    }

    void InitializeReposition()
    {
        m_hasCharged = false;
        float repositionAngle = Random.Range(m_minAngleReposition, m_maxAngleReposition);
        //Vector2 targetPosition = new Vector2(transform.position.x, transform.position.y) + new Vector2(m_attackRange * Mathf.Sin(repositionAngle), m_attackRange * Mathf.CoETURN, HIT, DEAD, REPOSITION }s(repositionAngle));

        Vector2 targetPosition;

        if (transform.position.x > m_player.transform.position.x)
        {
            targetPosition = new Vector2(transform.position.x + m_attackRange, transform.position.y + m_attackRange / 2);
        }
        else
        {
            targetPosition = new Vector2(transform.position.x - m_attackRange, transform.position.y + m_attackRange / 2);
        }

        //m_pathFinder.SnapToClosestNode();
        m_pathFinder.SetInitialNode(transform.position);
        m_pathFinder.SetTargetNode(targetPosition);
        ChangeAnimationState(ANIMATION_STATE.MOVE);
    }

    void Reposition() {
        m_pathFinder.NavigateToTargetPosition(); 
    }

    bool CanEnemySeePlayer(Vector2 p_position) {
        Vector2 playerPosition = new Vector2(m_player.transform.position.x, m_player.transform.position.y);
        Vector2 raycastDirection = (playerPosition - p_position).normalized;

        RaycastHit2D obstaclesHit = Physics2D.Raycast(transform.position, raycastDirection, m_attackRange, m_obstacleLayer);

        if(obstaclesHit.collider == null) { return true; }

        float distanceToPlayer = (playerPosition - p_position).magnitude;

        if(distanceToPlayer < obstaclesHit.distance) { return true; }
        return false;
    }

    bool DoesEnemyKnowsWhereThePlayerIs(){
        if(IsPlayerInRange(m_visionRange)){
            m_isPlayerInEnemyRangeFlag = true;
            return true;
        }
        else if(m_isPlayerInEnemyRangeFlag){ 
            m_isPlayerInEnemyRangeFlag = false;
            m_memoryTimer.Stop();
            m_memoryTimer.Run();
            return true;
            }
        else if(!m_memoryTimer.IsFinished){
            return true;
        }
        return false;
    }

    bool IsPlayerInRange(float p_range)
    {
        Vector2 distanceToPlayer;
        distanceToPlayer.x = transform.position.x - m_player.transform.position.x;
        distanceToPlayer.y = transform.position.y - m_player.transform.position.y;

        if (distanceToPlayer.magnitude <= p_range) { return true; }
        return false;
    }

    void FaceToPosition(float p_positionToFaceTo)
    {
        int direction = 1;
        if (p_positionToFaceTo - transform.position.x < 0) { direction = -1; };
        if (direction != FacingDirection()) { FlipX(); }
    }

    void FaceToDirection(){
        if(m_rb2D.velocity.x > 0 && !m_isFacingRight){ FlipX(); return ;}
        else if (m_rb2D.velocity.x < 0 && m_isFacingRight) { FlipX(); return ;}
        if(m_state == ENEMY_STATE.ATTACK) { return ;}
        if(m_pathFinder.GetDirection().y == 0){
            if(m_pathFinder.GetDirection().x == 1 && !m_isFacingRight){
                FlipX();
            }
            else if(m_pathFinder.GetDirection().x == -1 && m_isFacingRight){
                FlipX();
            }
        }
        else if(m_state == ENEMY_STATE.REPOSITION || m_state == ENEMY_STATE.CHASE){
            FaceToPosition(Player.Instance.transform.position.x);
        }
        else if(m_state == ENEMY_STATE.RETURN){
            FaceToPosition(m_pathFinder.GetTargetNode().Position.x);
        }
    }

    public override void Damage(float p_damage)
    {
        if(m_state == ENEMY_STATE.DEAD) { return ;}
        m_state = ENEMY_STATE.HIT;
        ChangeAnimationState(ANIMATION_STATE.HIT);
        base.Damage(p_damage);
        
    }

    public void EndHit(){
        if(m_health <= 0) { 
            m_state = ENEMY_STATE.DEAD;
            ChangeAnimationState(ANIMATION_STATE.DIE);
            m_state = ENEMY_STATE.DEAD;
            return;
        }
        m_state = ENEMY_STATE.PATROL;
        ChangeAnimationState(ANIMATION_STATE.MOVE);
    }

    void ReturnToNormalState()
    {
        m_state = ENEMY_STATE.PATROL;
        ChangeAnimationState(ANIMATION_STATE.MOVE);
    }

    public override void Reset(){
        base.Reset();
        ReturnToNormalState();
    }

    int FacingDirection()
    {
        if (m_isFacingRight) { return 1; }
        else { return -1; }
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }

    void SetState(ENEMY_STATE p_state) { m_state = p_state; }

    Vector2 GetDirection(Vector3 p_finalPosition, Vector3 p_originPosition)
    {
        Vector2 unitaryVector;
        unitaryVector = new Vector2(p_finalPosition.x - p_originPosition.x, p_finalPosition.y - p_originPosition.y);
        return unitaryVector.normalized;
    }

    protected override void Start()
    {
        base.Start();
        m_isFacingRight = true;
        m_restTimer.Duration = m_timeToWaitBetweenAttacks;
        m_memoryTimer.Duration = m_timeToRememberPlayer;

        m_playerScript = m_player.GetComponent<Player>();
        m_isGoingFrom1To2 = true;
        m_isCharging = false;

        m_minAngleReposition = Mathf.Asin((m_minHeightReposition / m_attackRange));
        m_maxAngleReposition = Mathf.Asin((m_maxHeightReposition / m_attackRange));

        
        m_state = ENEMY_STATE.PATROL;
        m_pathFinder.SetSpeed(m_speed);

        m_animationHash[(int)ANIMATION_STATE.MOVE] = Animator.StringToHash(m_moveAnimationName);
        m_animationHash[(int)ANIMATION_STATE.PREPARE_ATTACK] = Animator.StringToHash(m_prepareAttackAnimationName);
        m_animationHash[(int)ANIMATION_STATE.ATTACK] = Animator.StringToHash(m_attackAnimationName);
        m_animationHash[(int)ANIMATION_STATE.RECOVER_FROM_ATTACK] = Animator.StringToHash(m_recoverFromAttackAnimationName);
        m_animationHash[(int)ANIMATION_STATE.HIT] = Animator.StringToHash(m_hitAnimationName);
        m_animationHash[(int)ANIMATION_STATE.DIE] = Animator.StringToHash(m_dieAnimationName);
    }

    private void Update()
    {
        if(!m_isInitialized) {
            InitializePatrol(); 
            m_isInitialized = true;
        }
        FaceToDirection();

        switch (m_state)
        {
            default: break;
            case ENEMY_STATE.PATROL:
                {
                    if (IsPlayerInRange(m_visionRange))
                    {
                        SetState(ENEMY_STATE.CHASE);
                        InitializeChase();
                    }
                    else { Patrol(); }
                }
                break;
            case ENEMY_STATE.CHASE:
                {
                    if (!DoesEnemyKnowsWhereThePlayerIs())
                    {
                        SetState(ENEMY_STATE.PATROL);
                        InitializePatrol();
                    }
                    else if (IsPlayerInRange(m_attackRange))
                    {
                        SetState(ENEMY_STATE.ATTACK);
                    }
                    else { Chase(); }               
                }
                break;
            case ENEMY_STATE.IDLE:
                {
                    if (!m_isRecovering)
                    {
                        if (!IsPlayerInRange(m_attackRange))
                        {
                            SetState(ENEMY_STATE.CHASE);
                            InitializeChase();
                        }
                        else
                        {
                            InitializeReposition();
                            SetState(ENEMY_STATE.REPOSITION);
                        }
                    }
                }
                break;
            case ENEMY_STATE.ATTACK:
                {
                    if (m_isCharging) { return; }
                    
                    InitializeReposition();
                    SetState(ENEMY_STATE.REPOSITION);
                }
                break;
            case ENEMY_STATE.REPOSITION:
                {
                    if (m_pathFinder.IsFinished()) {
                        SetState(ENEMY_STATE.PREPARE_ATTACK);
                        InitializePrepareAttack();
                    }
                    else { Reposition(); }
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_visionRange);
        Gizmos.color = Color.red;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(m_patrolPoint_1.position.x, m_patrolPoint_1.position.y, transform.position.z),3);
        Gizmos.DrawSphere(new Vector3(m_patrolPoint_2.position.x, m_patrolPoint_2.position.y, transform.position.z),3);

    }

    private void EndRecovering() { m_isRecovering = false;}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.tag == "floor" || collision.tag == "platform") && m_state == ENEMY_STATE.ATTACK && m_state != ENEMY_STATE.DEAD)
        {
            m_pathFinder.SetInitialNodeToNone();
            m_state = ENEMY_STATE.IDLE;
            m_rb2D.velocity = Vector2.zero;
            m_memoryTimer.Stop();
            m_isRecovering = true;
            m_isCharging = false;
            ChangeAnimationState(ANIMATION_STATE.RECOVER_FROM_ATTACK);
        }
    }
}