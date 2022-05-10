using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPatrolMonster : Enemy
{
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
        m_collider = GetComponent<Collider2D>();
        m_dieAnimationDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.PATROL_BAT_DIE);
        m_hitAnimationDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.PATROL_BAT_HIT);
    }

    protected override void Start() {
        base.Start();
        m_isFacingRight = true;
        m_isGoingFrom1To2 = true;
        
        m_state = ENEMY_STATE.PATROL;
        m_pathFinder.SetSpeed(m_speed);

        m_dieAnimationDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.PATROL_BAT_DIE);
        m_hitAnimationDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.PATROL_BAT_HIT);


    }

    protected override void Update()
    {   
        base.Update();
        if(!m_isInitialized) {
            InitializePatrol(); 
            m_isInitialized = true;
        }

        switch(m_state){
            default: break;
            case ENEMY_STATE.PATROL:
                Patrol();
            break;

        }
        transform.position = new Vector3(transform.position.x, transform.position.y, Player.Instance.transform.position.z);
    }


    void InitializePatrol()
    {
        m_pathFinder.SetInitialNode(transform.position);
        //m_pathFinder.SnapToClosestNode();
        m_pathFinder.SetTargetNode(m_patrolPoint_2.position);
        m_isGoingFrom1To2 = true;
        FaceToDirection();
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PATROL_BAT_MOVE, false);
    }

    void Patrol()
    {
        
        m_pathFinder.NavigateToTargetPosition();
        if (m_pathFinder.IsFinished()) {
            SwapPatrolTarget();
        }
        FaceToDirection();
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
        base.Damage(p_damage);
        if(m_health <= 0) {
            m_rb2D.velocity = Vector2.zero;
            //m_rb2D.gravityScale = 20;
            m_state = ENEMY_STATE.DEATH;
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.PATROL_BAT_DIE, true);
            return ;
        }
        else{
            m_state = ENEMY_STATE.HIT;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PATROL_BAT_HIT, true);
        }
        
        
    }

    protected override void EndHit(){
        base.EndHit();
        if(m_health <= 0) { 
            m_state = ENEMY_STATE.DEATH;
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.PATROL_BAT_DIE, false);
            m_state = ENEMY_STATE.DEATH;
            return;
        }
        m_rb2D.velocity = Vector2.zero;
        InitializePatrol();
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PATROL_BAT_MOVE, false);
    }

    void FaceToDirectionRestrictedY(){
        if(m_pathFinder.GetDirection().y == 0){
            if(m_pathFinder.GetDirection().x == 1 && !m_isFacingRight){
                FlipX();
            }
            else if(m_pathFinder.GetDirection().x == -1 && m_isFacingRight){
                FlipX();
            }
        }
    }

    void FaceToDirection(){
        
        if(m_pathFinder.GetDirection().x == 1 && !m_isFacingRight){
            FlipX();
        }
        else if(m_pathFinder.GetDirection().x == -1 && m_isFacingRight){
            FlipX();
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
        InitializePatrol();
        m_collider.enabled = true;
        m_pathFinder.SetInitialNodeToNone();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(m_patrolPoint_1.position.x, m_patrolPoint_1.position.y, transform.position.z),3);
        Gizmos.DrawSphere(new Vector3(m_patrolPoint_2.position.x, m_patrolPoint_2.position.y, transform.position.z),3);
    }

}
