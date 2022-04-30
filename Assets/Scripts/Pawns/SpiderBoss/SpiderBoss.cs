using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderBoss : AnimatedCharacter
{
    [SerializeField] GameObject[] m_spiderEggs;
    Spider[] m_spiderEggsScript;
    SPIDER_BOSS_STATE m_state;
    [SerializeField] Transform m_acidAttackPosition;
    [SerializeField] GameObject m_acidBall;
    float m_detectionRange = 48.0f;
    [SerializeField] float m_biteRange = 16.0f;

        
    [SerializeField] float m_speed = 20.0f;
    [SerializeField] float m_speedVertical = 30.0f;
    [SerializeField] Transform m_spawnEggPosition;
    [SerializeField] Drill m_leftDrill;
    [SerializeField] Drill m_rightDrill;
    [SerializeField] Drill m_headScript;
    [SerializeField] GameObject m_head;
    Timer m_eventTimer;
    float m_drillDuration;
    float m_recoverFromTerrainDuration;
    float m_biteDuration;
    [SerializeField] float m_acidAttackCooldown;
    bool m_hasRoared = false;
    bool m_hasFiredAcidAttack = false;
    float m_roarDuration;
    float m_eggSpawnCooldown = 2.0f;
    float m_normalRecoverDuration;
    float m_acidAttackDuration;
    bool m_hasReachedSpawnEggPosition = false;
    bool m_hasDrillBeenDestroyed = false;
    bool m_hasEclosionedEggs = false;
    bool m_hasHitPlayer = false;

    [SerializeField] int m_minNumberOfAcidAttacks = 1;
    [SerializeField] int m_maxNumberOfAcidAttacks = 3;
    int m_currentNumberOfAcidAttacks = 0;
    [SerializeField] int m_numberOfDrillAttacks = 3;
    int m_numberOfAttacks = 0;
    Vector2 m_initialPosition;
    float m_direction = 0;
    int m_numberOfEggActivation = 0;

    [SerializeField] Image m_healthBar;
    [SerializeField] float m_drillMaxHealth = 33.0f;
    [SerializeField] float m_eyeMaxHealth = 10.0f;
    float m_maxHealth;
    const int NUMBER_OF_EYES = 6;
    [SerializeField] Sprite[] m_eyesSprite = new Sprite[NUMBER_OF_EYES + 1];
    [SerializeField] SpriteRenderer m_headRenderer;
    int m_numberOfEyesLeft = NUMBER_OF_EYES;
    bool m_hasEyeBeenDestroyed = false;

    float[] m_partsHealth = new float[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.LAT_NO_USE];
    bool[] m_isPartDestroyed = new bool[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.LAT_NO_USE];

    protected override void Awake() {
        base.Awake();
        m_eventTimer = gameObject.AddComponent<Timer>();
    }

    private void Start() {

        m_drillDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_ATTACK_RIGHT);
        m_recoverFromTerrainDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_RIGHT);
        m_biteDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_ATTACK_BITE);
        m_roarDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_ROAR);
        m_acidAttackDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_ATTACK_SPIT);
        m_normalRecoverDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_LEFT);

        InitializeChaseState();
        m_partsHealth[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.HEAD] = NUMBER_OF_EYES * m_eyeMaxHealth;
        m_partsHealth[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.LEFT_DRILL] = m_drillMaxHealth;
        m_partsHealth[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.RIGHT_DRILL] = m_drillMaxHealth;
        m_healthBar.fillAmount = 1;

        for(int i = 0; i  < (int)SPIDER_BOSS_DAMAGEABLE_PARTS.LAT_NO_USE; i++){
            m_maxHealth += m_partsHealth[i];
        }

        m_initialPosition =  transform.position;

        m_spiderEggsScript = new Spider[m_spiderEggs.Length];
        for(int i = 0; i < m_spiderEggs.Length; i++){
            m_spiderEggsScript[i] = m_spiderEggs[i].GetComponentInChildren<Spider>();
        }

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
            case SPIDER_BOSS_STATE.ATTACK_ACID:
                HandleAcidAttack();
            break;
            case SPIDER_BOSS_STATE.RETURN_TO_CENTER:
                HanldeReturnToCenter();
            break;
            case SPIDER_BOSS_STATE.EGG_SPAWN:
                HandleEggSpawn();
            break;
            case SPIDER_BOSS_STATE.RECOVER_NORMAL:
                HandleNormalRecoverState();
            break;
        }
    }

    void InitializeIdleState(){
        m_state = SPIDER_BOSS_STATE.IDLE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_IDLE, false);
    }

    void HandleIdleState(){

    }

    void InitializeEggSpawn(){
        m_state = SPIDER_BOSS_STATE.EGG_SPAWN;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_MOVE, false);
        m_hasEclosionedEggs = true;
    }

    void HandleEggSpawn(){
        if(transform.position.y < m_spawnEggPosition.position.y && !m_hasReachedSpawnEggPosition){
            transform.position += new Vector3(0, m_speed * Time.deltaTime, 0);
            return ;
        }
        else if (!m_hasReachedSpawnEggPosition){
            m_hasReachedSpawnEggPosition = true;
            transform.position = new Vector3(transform.position.x, m_spawnEggPosition.position.y, transform.position.z);
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ROAR, false);
            m_eventTimer.Duration = m_roarDuration;
            m_eventTimer.Restart();
        }

        if(m_eventTimer.IsFinished){
            SpawnEggs();
            InitializeReturnToCenter();
            m_hasReachedSpawnEggPosition = false;
        }

    }

    void SpawnEggs(){

        if(m_numberOfEggActivation == 0){
           for(int i = 0; i < 3; i++){
            m_spiderEggsScript[i].Hatch();
            }
            m_numberOfEggActivation++;
        }
        else if(m_numberOfEggActivation == 1){
            for(int i = 3; i < m_spiderEggs.Length; i++){
            m_spiderEggsScript[i].Hatch();
            m_numberOfEggActivation++;
            }
        }

        
    }

    void InitializeChaseState(){
        m_state = SPIDER_BOSS_STATE.CHASE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_MOVE_RIGHT, false);
    }

    void HandleChaseState(){
        if(m_numberOfAttacks >= m_numberOfDrillAttacks || m_hasEyeBeenDestroyed){
            InitializeAcidAttack();
            m_numberOfAttacks = 0;
            m_hasEyeBeenDestroyed = false;
            // ECLOSION EGGS
            return;
        }
        if(IsPlayerInRange(m_detectionRange)){
            InitializeAttackState();
            return ;
        }
        m_direction = (Player.Instance.transform.position.x - transform.position.x)/Mathf.Abs(Player.Instance.transform.position.x - transform.position.x);
        transform.position += new Vector3(m_direction*m_speed*Time.deltaTime, 0, 0);
    }

    void InitializeAttackState(){
        if(IsPlayerInRange(m_biteRange) || m_isPartDestroyed[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.LEFT_DRILL] && m_isPartDestroyed[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.RIGHT_DRILL]){
            InitializeBiteState();
            return ;
        }
        InitializeDrillState();
        m_numberOfAttacks++;
    }

    void InitializeBiteState(){
        m_state = SPIDER_BOSS_STATE.ATTACK_BITE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_BITE, false);
        m_eventTimer.Duration = m_biteDuration;
        m_eventTimer.Restart();
        if(m_isPartDestroyed[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.LEFT_DRILL] && m_isPartDestroyed[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.RIGHT_DRILL]){
            m_headScript.CanBeDamaged = true;
        }
        m_headScript.CanDamagePlayer = true;
    }

    void InitializeDrillState(){
        m_state = SPIDER_BOSS_STATE.ATTACK_DRILL;
        m_eventTimer.Duration = m_drillDuration;
        m_eventTimer.Restart();

        if(m_partsHealth[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.LEFT_DRILL] <= 0){
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_RIGHT, false);
            m_rightDrill.CanDamagePlayer = true;
            return ;
        }
        else if(m_partsHealth[(int)SPIDER_BOSS_DAMAGEABLE_PARTS.RIGHT_DRILL] <= 0){
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_LEFT, false);
            return ;
        }

        if(Player.Instance.transform.position.x < transform.position.x){
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_LEFT, false);
            return ;
        }
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_RIGHT, false);
    }

    void ActivateLeftDrill(){
        if(!m_leftDrill.gameObject.activeInHierarchy){ return ;}
        m_leftDrill.CanDamagePlayer = true;
        m_leftDrill.CanBeDamaged = false;
    }
    void ActivateRightDrill(){
        if(!m_rightDrill.gameObject.activeInHierarchy){ return ;}
        m_rightDrill.CanDamagePlayer = true;
        m_rightDrill.CanBeDamaged = false;
    }

    void HandleDrillState(){
        if(m_eventTimer.IsFinished){
            if(m_animationState == ANIMATION.SPIDER_BOSS_ATTACK_RIGHT){
                m_rightDrill.CanDamagePlayer = false;
                m_rightDrill.CanBeDamaged = true;
            }
            else{
                m_leftDrill.CanDamagePlayer = false; 
                m_leftDrill.CanBeDamaged = true;
            }

            if(m_hasHitPlayer){
                InitializeNormalRecoverState();
                m_hasHitPlayer = false;
            }
            else{
                InitializeRecoverTerrainState();
            }
        }
    }

    void InitializeNormalRecoverState(){
        m_state = SPIDER_BOSS_STATE.RECOVER_NORMAL;
        m_eventTimer.Duration = m_normalRecoverDuration;
        m_eventTimer.Restart();
        if(m_animationState == ANIMATION.SPIDER_BOSS_ATTACK_LEFT){
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_LEFT, false);
        }
        else{
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_RIGHT, false);
        }
    }

    void HandleNormalRecoverState(){
        if(m_eventTimer.IsFinished){
            InitializeChaseState();
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
            if(m_hasDrillBeenDestroyed){

                m_hasDrillBeenDestroyed = false;
                InitializeReturnToCenter();

                if(m_animationState == ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_LEFT){
                    m_leftDrill.CanDamagePlayer = false;
                }else{
                     m_rightDrill.CanDamagePlayer = false;
                }

                return ;
            }
            if(m_animationState == ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_LEFT){
                m_leftDrill.CanDamagePlayer = false;
            }else{
                m_rightDrill.CanDamagePlayer = false;
            }
            InitializeChaseState();
        }
    }

    void InitializeAcidAttack(){
        m_state = SPIDER_BOSS_STATE.ATTACK_ACID;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ROAR, false);
        m_eventTimer.Duration = m_roarDuration;
        m_eventTimer.Restart();
        m_currentNumberOfAcidAttacks = Random.Range(m_minNumberOfAcidAttacks, m_maxNumberOfAcidAttacks + 1);
    }

    void HandleAcidAttack(){
        if(m_hasRoared){
            Vector2 spiderToPlayer = Player.Instance.transform.position - m_head.transform.position;

        float angle = 360 / (2 * Mathf.PI) * Mathf.Atan(spiderToPlayer.y / spiderToPlayer.x);

        if(Player.Instance.transform.position.x < m_head.transform.position.x){
            angle += - 90;
        }
        else{
            angle += + 90;
        }

        m_head.transform.eulerAngles = new Vector3(0,0,angle);
        }

        if(!m_hasRoared && m_eventTimer.IsFinished){
            m_hasRoared = true;
            m_eventTimer.Duration = m_acidAttackCooldown;
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_SPIT, false);
            m_eventTimer.Run();
            return ;
        }

        if(m_hasRoared && m_eventTimer.IsFinished){
            if(m_numberOfAttacks >= m_currentNumberOfAcidAttacks){
                InitializeChaseState();
                m_numberOfAttacks = 0;
                m_head.transform.eulerAngles = Vector3.zero;
            }
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_ATTACK_SPIT, false);
            m_hasFiredAcidAttack = false;
            m_eventTimer.Run();
        }

    }

    public void FireAcidAttack(){
        AcidBall script = Instantiate(m_acidBall, m_acidAttackPosition.transform.position, Quaternion.identity).GetComponent<AcidBall>();
        Vector2 direction = (Player.Instance.transform.position - script.gameObject.transform.position).normalized;
        script.Initialize(direction);
        m_hasFiredAcidAttack = true;
        m_numberOfAttacks++;
    }

    public void InitializeReturnToCenter(){
        m_state = SPIDER_BOSS_STATE.RETURN_TO_CENTER;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_BOSS_MOVE_RIGHT, false);

        if(transform.position.x == m_initialPosition.x && transform.position.y == m_initialPosition.y){
            if(!m_hasEclosionedEggs){
                InitializeEggSpawn();
            }
            else{
                InitializeChaseState();
            }
            return ;
        }

        if(transform.position.x > m_initialPosition.x){
            m_direction = -1;
            return ;
        }
        else if(transform.position.x < m_initialPosition.x) {
            m_direction = 1;
            return ;
        }
        
        if(transform.position.y > m_initialPosition.y){
            m_direction = -1;
        }
        else if(transform.position.y < m_initialPosition.y) { m_direction = 1;}
    }

    void HanldeReturnToCenter(){
        if(transform.position.x != m_initialPosition.x){
            ReturnToCenterX();
        }
        else {
            ReturnToCenterY();
        }
        
        if( transform.position.x ==  m_initialPosition.x && transform.position.y == m_initialPosition.y){
            m_direction = 0;
            if(!m_hasEclosionedEggs){
                InitializeEggSpawn();
            }
            else{
                InitializeAcidAttack();
                m_hasEclosionedEggs = false;
            }
            
        }
        
        
    }

    void ReturnToCenterX(){
        if(m_direction == 1 && transform.position.x > m_initialPosition.x || m_direction == -1 && transform.position.x < m_initialPosition.x){
            transform.position = new Vector3(m_initialPosition.x, transform.position.y, transform.position.z);
            if(transform.position.y > m_initialPosition.y){
            m_direction = -1;
            }
            else{ m_direction = 1;}
            return;
        }
        transform.position += new Vector3(m_direction * m_speed * Time.deltaTime, 0, 0);
    }

    void ReturnToCenterY(){
        if(m_direction == 1 && transform.position.y > m_initialPosition.y || m_direction == -1 && transform.position.y < m_initialPosition.y){
            transform.position = new Vector3(transform.position.x, m_initialPosition.y, transform.position.z);
            return ;
        }
        transform.position += new Vector3(0, m_direction * m_speed * Time.deltaTime, 0);
    }

    bool IsPlayerInRange(float p_range){
        if(Mathf.Abs((Player.Instance.transform.position.x - transform.position.x)) <= p_range){
            return true;
        }
        return false;
    }

    public void Damage(float p_amount, SPIDER_BOSS_DAMAGEABLE_PARTS p_part){

        if(p_part == SPIDER_BOSS_DAMAGEABLE_PARTS.HEAD && m_hasEyeBeenDestroyed){
            return ;
        }

        if(m_partsHealth[(int)p_part] > 0){
            m_partsHealth[(int)p_part] -= p_amount;
        }

        if(p_part == SPIDER_BOSS_DAMAGEABLE_PARTS.HEAD  && !m_hasEyeBeenDestroyed && m_partsHealth[(int)p_part] < (m_numberOfEyesLeft - 1) * m_eyeMaxHealth){
            m_numberOfEyesLeft--;
            m_headRenderer.sprite = m_eyesSprite[NUMBER_OF_EYES - m_numberOfEyesLeft];
            Debug.Log(m_numberOfEyesLeft + "EYES LEFT");

            switch(m_numberOfEyesLeft){
                case 1:
                    m_hasEyeBeenDestroyed = true;
                break;
                case 2:
                    m_hasEyeBeenDestroyed = true;
                break;
                case 4:
                    m_hasEyeBeenDestroyed = true;
                break;
            }

        }

        if(m_partsHealth[(int)p_part] == 0 && !m_isPartDestroyed[(int)p_part]){
            
            m_isPartDestroyed[(int)p_part] = true;

            if(p_part != SPIDER_BOSS_DAMAGEABLE_PARTS.HEAD){
                m_hasDrillBeenDestroyed = true;
            }

            m_hasDrillBeenDestroyed = true;
            switch(p_part){
                case SPIDER_BOSS_DAMAGEABLE_PARTS.LEFT_DRILL:
                    m_leftDrill.InitializeExplosion();
                break;
                case SPIDER_BOSS_DAMAGEABLE_PARTS.RIGHT_DRILL:
                    m_rightDrill.InitializeExplosion();
                break;
                case SPIDER_BOSS_DAMAGEABLE_PARTS.HEAD:
                    InitializeIdleState();
                    Debug.Log("END OF COMBAT");
                break;
            }
        }

        float health = 0;
        
        for(int i = 0; i < (int)SPIDER_BOSS_DAMAGEABLE_PARTS.LAT_NO_USE;i++){
            health += m_partsHealth[i];

        }

        m_healthBar.fillAmount = health / m_maxHealth;
    }

    public bool HasHitPlayer {
        set { m_hasHitPlayer = value; }
    }

}
