using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPatrolMonster : Enemy
{
    enum ENEMY_STATE { PATROL, IDLE, HIT, DEAD }
    enum ANIMATION_STATE {MOVE, DIE, HIT, LAST_NO_USE}
    string m_moveAnimationName      = "move";
    string m_hitAnimationName = "hit";
    string m_dieAnimationName    = "die";
    int[] m_animationHash = new int[(int)ANIMATION_STATE.LAST_NO_USE];
    int m_currentAnimationHash;
    ANIMATION_STATE m_animationState;    
     void ChangeAnimationState(ANIMATION_STATE p_animationState)
    {
        int newAnimationHash = m_animationHash[(int)p_animationState];

        if (m_currentAnimationHash == newAnimationHash) return;   // stop the same animation from interrupting itself
        m_animator.Play(newAnimationHash);                // play the animation
        m_currentAnimationHash = newAnimationHash;                // reassigning the new state
        m_animationState = p_animationState;
    }

    [SerializeField] LayerMask m_obstacleLayer;
    [SerializeField] float m_speed = 40;
    Rigidbody2D m_rb2D;
    [SerializeField] Transform m_patrolPoint_1;
    [SerializeField] Transform m_patrolPoint_2;
    ENEMY_STATE m_state;
    PathFinder m_pathFinder;
    bool m_isFacingRight;
    bool m_isGoingFrom1To2;
    bool m_isInitialized = false;

    protected override void Awake() {
        base.Awake();
        m_rb2D = GetComponent<Rigidbody2D>();
        m_pathFinder = GetComponent<PathFinder>();
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider2D>();
    }

    protected override void Start() {
        base.Start();
        m_isFacingRight = true;
        m_isGoingFrom1To2 = true;
        
        m_state = ENEMY_STATE.PATROL;
        m_pathFinder.SetSpeed(m_speed);

        m_animationHash[(int)ANIMATION_STATE.MOVE] = Animator.StringToHash(m_moveAnimationName);
        m_animationHash[(int)ANIMATION_STATE.DIE] = Animator.StringToHash(m_dieAnimationName);
        m_animationHash[(int)ANIMATION_STATE.HIT] = Animator.StringToHash(m_hitAnimationName);


    }

    protected override void Update()
    {   
        base.Update();
        if(!m_isInitialized) {
            InitializePatrol(); 
            m_isInitialized = true;
        }

        FaceToDirection();

        switch(m_state){
            default: break;
            case ENEMY_STATE.PATROL:
                Patrol();
            break;

        }
    }


    void InitializePatrol()
    {
        m_pathFinder.SetInitialNode(transform.position);
        //m_pathFinder.SnapToClosestNode();
        m_pathFinder.SetTargetNode(m_patrolPoint_2.position);
        m_isGoingFrom1To2 = true;
        ChangeAnimationState(ANIMATION_STATE.MOVE);
    }

    void Patrol()
    {
        m_pathFinder.NavigateToTargetPosition();
        if (m_pathFinder.IsFinished()) {
             SwapPatrolTarget(); }
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

    public override void Damage(float p_damage)
    {
        if(m_state == ENEMY_STATE.DEAD) { return ;}
        m_state = ENEMY_STATE.HIT;
        ChangeAnimationState(ANIMATION_STATE.HIT);
        base.Damage(p_damage);
        
    }

    protected override void EndHit(){
        base.EndHit();
        if(m_health <= 0) { 
            m_state = ENEMY_STATE.DEAD;
            ChangeAnimationState(ANIMATION_STATE.DIE);
            m_state = ENEMY_STATE.DEAD;
            return;
        }
        m_state = ENEMY_STATE.PATROL;
        ChangeAnimationState(ANIMATION_STATE.MOVE);
    }

    void FaceToDirection(){
        if(m_pathFinder.GetDirection().y == 0){
            if(m_pathFinder.GetDirection().x == 1 && !m_isFacingRight){
                FlipX();
            }
            else if(m_pathFinder.GetDirection().x == -1 && m_isFacingRight){
                FlipX();
            }
        }
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }

    void FaceToPosition(float p_positionToFaceTo)
    {
        int direction = 1;
        if (p_positionToFaceTo - transform.position.x < 0) { direction = -1; };
        if (direction != FacingDirection()) { FlipX(); }
    }

        int FacingDirection()
    {
        if (m_isFacingRight) { return 1; }
        else { return -1; }
    }

    public override void Reset(){
        base.Reset();
        m_collider.enabled = true;
        m_state = ENEMY_STATE.PATROL;
        ChangeAnimationState(ANIMATION_STATE.MOVE);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(m_patrolPoint_1.position.x, m_patrolPoint_1.position.y, transform.position.z),3);
        Gizmos.DrawSphere(new Vector3(m_patrolPoint_2.position.x, m_patrolPoint_2.position.y, transform.position.z),3);
    }

}
