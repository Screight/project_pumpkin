using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonster : MonoBehaviour
{
    enum ENEMY_STATE { PATROL, CHASE, ATTACK, IDLE, RETURN, HIT, DEAD, REPOSITION }

    [SerializeField] LayerMask m_enemyLayer;

    [SerializeField] float m_speed = 40;
    [SerializeField] float m_chargeSpeed = 100;
    Rigidbody2D m_rb2D;

    Timer m_restTimer;
    [SerializeField] float m_TimeToWaitBetweenAttacks = 1;

    Timer m_memoryTimer;
    [SerializeField] float m_TimeToRememberPlayer = 1;
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

    private void Awake()
    {
        m_rb2D = GetComponent<Rigidbody2D>();

        m_restTimer = gameObject.AddComponent<Timer>();
        m_memoryTimer = gameObject.AddComponent<Timer>();
        m_player = GameObject.FindGameObjectWithTag("Player");

        m_pathFinder = GetComponent<PathFinderTest>();
    }

        void InitializePatrol()
    {
        m_pathFinder.SetInitialNode(transform.position);
        //m_pathFinder.SnapToClosestNode();
        m_pathFinder.SetTargetNode(m_patrolPoint_2.position);
        m_isGoingFrom1To2 = true;
    }

    void Patrol()
    {
        m_pathFinder.NavigateToTargetPosition();
        if (m_pathFinder.IsFinished()) {
            SwapPatrolTarget();
        }
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
    }

    void Chase()
    {
        m_pathFinder.SetTargetNode(m_player.transform.position);
        m_pathFinder.NavigateToTargetPosition();
    }

    void InitializeAttack()
    {
        m_isCharging = true;
        m_rb2D.velocity = m_chargeSpeed * GetDirection(m_player.transform.position, transform.position);
        m_hasCharged = true;
    }

    void Attack()
    {
        
    }

    void InitializeReposition()
    {
        m_hasCharged = false;
        float repositionAngle = Random.Range(m_minAngleReposition, m_maxAngleReposition);
        //Vector2 targetPosition = new Vector2(transform.position.x, transform.position.y) + new Vector2(m_attackRange * Mathf.Sin(repositionAngle), m_attackRange * Mathf.Cos(repositionAngle));

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
    }

    void Reposition()
    {
        m_pathFinder.NavigateToTargetPosition();
    }

    bool CanEnemySeePlayer(Vector2 p_position) {
        Vector2 playerPosition = new Vector2(m_player.transform.position.x, m_player.transform.position.y);
        Vector2 raycastDirection = (playerPosition - p_position).normalized;

        RaycastHit2D obstaclesHit = Physics2D.Raycast(transform.position, raycastDirection, m_attackRange, m_enemyLayer);

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

        if (distanceToPlayer.magnitude <= p_range)
        {
            return true;
        }
        return false;
    }

    void FaceToPosition(float p_positionToFaceTo)
    {
        int direction = 1;
        if (p_positionToFaceTo - transform.position.x < 0) { direction = -1; };
        if (direction != FacingDirection()) { FlipX(); }
    }

    int FacingDirection()
    {
        if (m_isFacingRight) return 1;
        else return -1;
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }

    void SetState(ENEMY_STATE p_state)
    {
        m_state = p_state;
    }

    Vector2 GetDirection(Vector3 p_finalPosition, Vector3 p_originPosition)
    {
        Vector2 unitaryVector;
        unitaryVector = new Vector2(p_finalPosition.x - p_originPosition.x, p_finalPosition.y - p_originPosition.y);
        return unitaryVector.normalized;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_isFacingRight = true;
        m_restTimer.Duration = m_TimeToWaitBetweenAttacks;
        m_memoryTimer.Duration = m_TimeToRememberPlayer;

        m_playerScript = m_player.GetComponent<Player>();
        m_isGoingFrom1To2 = true;
        m_isCharging = false;

        m_minAngleReposition = Mathf.Asin((m_minHeightReposition / m_attackRange));
        m_maxAngleReposition = Mathf.Asin((m_maxHeightReposition / m_attackRange));

        InitializePatrol();
        m_state = ENEMY_STATE.PATROL;
    }

    private void Update()
    {
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
                    if (m_restTimer.IsFinished)
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
                    if (CanEnemySeePlayer(transform.position) && !m_hasCharged)
                    {
                        InitializeAttack();
                    }
                    else
                    {
                        InitializeReposition();
                        SetState(ENEMY_STATE.REPOSITION);
                    }
                }
                break;
            case ENEMY_STATE.REPOSITION:
                {
                    if (m_pathFinder.IsFinished()) {
                        SetState(ENEMY_STATE.ATTACK);
                        InitializeAttack();
                    }
                    else { Reposition(); }
                }
                break;
        }
        Debug.Log(m_state);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_visionRange);
        Gizmos.color = Color.red;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.tag == "floor" || collision.tag == "platform") && m_state == ENEMY_STATE.ATTACK)
        {
            m_pathFinder.SetInitialNodeToNone();
            m_state = ENEMY_STATE.IDLE;
            m_rb2D.velocity = Vector2.zero;
            m_memoryTimer.Stop();
            m_restTimer.Run();
            m_isCharging = false;
            Debug.Log("End of charge");
        }
    }

}
