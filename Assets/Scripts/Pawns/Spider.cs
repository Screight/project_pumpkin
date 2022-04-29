using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    Rigidbody2D m_rb2d;
    ENEMY_STATE m_state;
    [SerializeField] float m_speed;
    [SerializeField] Transform m_leftPatrolPoint;
    [SerializeField] Transform m_rightPatrolPoint;

    [SerializeField] float m_minimumDistanceToEclosion = 50.0f;
    [SerializeField] bool m_isEclosionAutomatic = false;
    [SerializeField] float m_eclosionDuration = 2.0f;
    Timer m_eventTimer;


    protected override void Awake() {
        base.Awake();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_eventTimer = gameObject.AddComponent<Timer>();
    }

    protected override void Start() {
        base.Start();

        if(m_leftPatrolPoint.position.x > m_rightPatrolPoint.transform.position.x){
            Transform provitional = m_leftPatrolPoint;
            m_leftPatrolPoint = m_rightPatrolPoint;
            m_rightPatrolPoint = provitional;
        }
        InitializeEclosion();
        
    }

    protected override void Update() {
        base.Update();

        switch(m_state){
            case ENEMY_STATE.PATROL:
            {
                HandlePatrol();
            }
            break;
            case ENEMY_STATE.EGG:
            {
                HandleEclosion();
            }
            break;
        }

    }

    void InitializeEclosion(){
        m_state = ENEMY_STATE.EGG;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_EGG, false);
        if(m_isEclosionAutomatic){
            m_eventTimer.Duration = m_eclosionDuration;
            m_eventTimer.Run();
        }
    }

    void HandleEclosion(){
        if(m_isEclosionAutomatic && m_eventTimer.IsFinished){
            Hatch();
        }
        else if(!m_isEclosionAutomatic){
            if(Mathf.Abs(Player.Instance.transform.position.x - transform.position.x) < m_minimumDistanceToEclosion){
                Hatch();
            }
        }
    }

    void Hatch(){
        // INSTANTIATE PARTICLES
        InitializePatrol();
    }

    void InitializePatrol(){
        m_state = ENEMY_STATE.PATROL;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_WALK, false);
        m_rb2d.velocity = new Vector2(m_speed, 0);
    }

    void HandlePatrol(){
        if(transform.position.x > m_rightPatrolPoint.transform.position.x ){
            transform.position = new Vector3(m_rightPatrolPoint.position.x, transform.position.y, transform.position.z);
            m_rb2d.velocity = new Vector2(-m_rb2d.velocity.x, m_rb2d.velocity.y);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if(transform.position.x < m_leftPatrolPoint.transform.position.x){
            transform.position = new Vector3(m_leftPatrolPoint.position.x, transform.position.y, transform.position.z);
            m_rb2d.velocity = new Vector2(-m_rb2d.velocity.x, m_rb2d.velocity.y);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

}
