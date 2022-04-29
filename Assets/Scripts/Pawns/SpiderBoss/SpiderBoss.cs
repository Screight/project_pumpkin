using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : AnimatedCharacter
{
    SPIDER_BOSS_STATE m_state;
    [SerializeField] float m_detectionRange = 48.0f;
    [SerializeField] float m_biteRange = 16.0f;
    [SerializeField] float m_speed = 20.0f;
    [SerializeField] Drill m_leftDrill;
    [SerializeField] Drill m_rightDrill;
    Timer m_eventTimer;
    float m_drillDuration;
    float m_recoverFromTerrainDuration;
    float m_biteDuration;

    protected override void Awake() {
        base.Awake();
        m_eventTimer = gameObject.AddComponent<Timer>();
    }

    private void Start() {
        InitializeChaseState();

        m_drillDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_ATTACK_RIGHT);
        m_recoverFromTerrainDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_RIGHT);
        m_biteDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_ATTACK_BITE);
    }

    private void Update() {
        switch(m_state){
            default: break;
            case SPIDER_BOSS_STATE.IDLE:
                HandleIdleState();
            break;
            case SPIDER_BOSS_STATE.CHASE:
                HandleChaseState();
            break;
            case SPIDER_BOSS_STATE.ATTACK_DRILL:
                HandleDrillState();
            break;
            case SPIDER_BOSS_STATE.ATTACK_BITE:
                HandleBiteState();
            break;
            case SPIDER_BOSS_STATE.RECOVER_TERRAIN:
                HandleRecoverTerrainState();
            break;
        }
    }

    void InitializeIdleState(){
        m_state = SPIDER_BOSS_STATE.IDLE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_IDLE, false);
    }

    void HandleIdleState(){

    }

    void InitializeChaseState(){
        m_state = SPIDER_BOSS_STATE.CHASE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_MOVE_RIGHT, false);
    }

    void HandleChaseState(){
        if(IsPlayerInRange(m_detectionRange)){
            InitializeAttackState();
            return ;
        }
        float direction = (Player.Instance.transform.position.x - transform.position.x)/Mathf.Abs(Player.Instance.transform.position.x - transform.position.x);
        transform.position += new Vector3(direction*m_speed*Time.deltaTime, 0, 0);
    }

    void InitializeAttackState(){
        if(IsPlayerInRange(m_biteRange)){
            InitializeBiteState();
            return ;
        }
        InitializeDrillState();
    }

    void InitializeBiteState(){
        m_state = SPIDER_BOSS_STATE.ATTACK_BITE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_BITE, false);
        m_eventTimer.Duration = m_biteDuration;
        m_eventTimer.Restart();
    }

    void InitializeDrillState(){
        m_state = SPIDER_BOSS_STATE.ATTACK_DRILL;
        m_eventTimer.Duration = m_drillDuration;
        m_eventTimer.Restart();

        if(Player.Instance.transform.position.x < transform.position.x){
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_LEFT, false);
            m_leftDrill.CanDamagePlayer = true;
            return ;
        }
        m_rightDrill.CanDamagePlayer = true;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_RIGHT, false);
    }

    void HandleDrillState(){
        if(m_eventTimer.IsFinished){
            if(m_animationState == ANIMATION.SPIDER_BOSS_ATTACK_RIGHT){
                m_rightDrill.CanDamagePlayer = false;
            }
            else{ m_leftDrill.CanDamagePlayer = false; }
            InitializeRecoverTerrainState();
        }
    }

    void HandleBiteState(){
        if(m_eventTimer.IsFinished){
            InitializeChaseState();
        }
    }

    public void InitializeRecoverTerrainState(){
        m_state = SPIDER_BOSS_STATE.RECOVER_TERRAIN;
        m_eventTimer.Duration = m_recoverFromTerrainDuration;
        m_eventTimer.Restart();

        if(m_animationState == ANIMATION.SPIDER_BOSS_ATTACK_LEFT){
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_LEFT, false);
        }
        else{
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_RIGHT, false);
        }
        
    }

    void HandleRecoverTerrainState(){
        if(m_eventTimer.IsFinished){
            InitializeChaseState();
        }
    }

    bool IsPlayerInRange(float p_range){
        if(Mathf.Abs((Player.Instance.transform.position.x - transform.position.x)) <= p_range){
            return true;
        }
        return false;
    }

}
