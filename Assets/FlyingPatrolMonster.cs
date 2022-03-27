using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPatrolMonster : Enemy
{
    enum ENEMY_STATE { PATROL, IDLE, HIT, DEAD }
    enum ANIMATION_STATE {}
    
    [SerializeField] float m_speed = 40;
    Rigidbody2D m_rb2D;
    [SerializeField] Transform m_patrolPoint_1;
    [SerializeField] Transform m_patrolPoint_2;
    ENEMY_STATE m_state;
    PathFinderTest m_pathFinder;
    bool m_isFacingRight;
    bool m_isGoingFrom1To2;
    bool m_isInitialized = false;

    protected override void Awake() {
        base.Awake();
        m_rb2D = GetComponent<Rigidbody2D>();
        m_pathFinder = GetComponent<PathFinderTest>();
    }

    protected override void Start() {
        base.Start();
        m_isFacingRight = true;
        m_isGoingFrom1To2 = true;
        
        m_state = ENEMY_STATE.PATROL;
        m_pathFinder.SetSpeed(m_speed);
    }

    void Update()
    {
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

}
