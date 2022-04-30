using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    bool m_isAttachedToPlayer;
    [SerializeField] Transform m_attackPosition;
    public enum ATTACK_ANIMATION { NO_ATTACK, ATTACK_1, ATTACK_2, ATTACK_3,AERIAL, LAST_NO_USE}
    string[] m_animationNames = {"noAttack", "attack_1", "attack_2", "attack_3", "aerial_attack"};
    Animator m_animator;
    int[] m_animationHash = new int[(int)ATTACK_ANIMATION.LAST_NO_USE];
    int m_animationCurrentHash;
    int m_attackComboNumber = 0; // represents first attack(1), second(2) and third(3) and aerial 4

    Timer m_attackTimer;
    float[] m_attackDuration;

    const float M_ATTACK_RANGE = 8;
    [SerializeField] LayerMask m_enemyLayer;
    bool m_keepAttacking;
    bool m_isAttacking;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_attackTimer = gameObject.AddComponent<Timer>();
        m_attackDuration = new float[(int)ATTACK_ANIMATION.LAST_NO_USE];
        if(GetComponentInParent<Player>() != null){
            m_isAttachedToPlayer = true;
        }
        else { m_isAttachedToPlayer = false;}
    }

    private void Start()
    {
        for(int i = 0; i < (int)ATTACK_ANIMATION.LAST_NO_USE; i++){
            m_animationHash[i] = Animator.StringToHash(m_animationNames[i]);
        }

        foreach(AnimationClip m_animationClip in m_animator.runtimeAnimatorController.animationClips)
        {
            for(int j = 0; j < (int)ATTACK_ANIMATION.LAST_NO_USE; j++){
                if(m_animationClip.name == m_animationNames[j]){
                    m_attackDuration[j] = m_animationClip.length;
                    j = (int)ATTACK_ANIMATION.LAST_NO_USE;
                }
            }
        }
    }


    private void Update() {
        if(m_attackTimer.IsFinished && m_isAttacking){
            HandleCombo();
        }
    }

    public void HandleAttack(bool p_isGrounded){
        if(GameManager.Instance.IsGamePaused){ return; }
        if(p_isGrounded){ HandleGroundAttackState(); }
        else { HandleAerialAttackState(); }
    }

    void HandleAerialAttackState(){
        if(InputManager.Instance.AttackButtonPressed && !m_isAttacking){
            m_attackComboNumber = 4;
            InitializeAttack();
            m_isAttacking = true;
        }
    }

    void HandleGroundAttackState(){
        if(InputManager.Instance.AttackButtonPressed){
            m_keepAttacking = true;
            Player.Instance.State = PLAYER_STATE.ATTACK;
            if(!m_isAttacking){
                StartCombo();
            }
        }
    }


    void HandleCombo(){
        if(!m_keepAttacking) {
            EndCombo();
            return;
        }
        else{
            m_attackComboNumber++;
            if(m_attackComboNumber > 3){ m_attackComboNumber = 1;}
            InitializeAttack();
        }
    }

    void InitializeAttack(){
        m_attackTimer.Duration = m_attackDuration[m_attackComboNumber];
        m_attackTimer.Run();
        ChangeAnimationState((ATTACK_ANIMATION)m_attackComboNumber);
        if(!m_isAttachedToPlayer){
            if(Player.Instance.IsFacingRight) { transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,transform.localScale.z);}
        else { transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y,transform.localScale.z);}
        this.transform.position = m_attackPosition.position;
        }
        m_keepAttacking = false;
    }

    void StartCombo(){
        m_isAttacking = true;
        m_attackComboNumber = 1;
        Player.Instance.State = PLAYER_STATE.ATTACK;
        InitializeAttack();
    }

    void EndCombo(){
        m_attackComboNumber = 0;
        ChangeAnimationState(ATTACK_ANIMATION.NO_ATTACK);
        m_isAttacking = false;
        m_keepAttacking = false;
        if(Player.Instance.State != PLAYER_STATE.DASH){
            Player.Instance.State = PLAYER_STATE.IDLE;
        }
    }

    void Hit()
    {
        Collider2D[] enemiesInAttackRange = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), M_ATTACK_RANGE, m_enemyLayer);
        foreach (Collider2D enemy in enemiesInAttackRange)
        {
            if (enemy.gameObject.tag == "enemy") { 
                Enemy enemyScript = enemy.gameObject.GetComponent<Enemy>();
                if(!enemyScript.IsDying){
                    enemyScript.Damage(GameManager.Instance.PlayerAttackDamage);
                }
                 
            }

            if (enemy.gameObject.tag == "spiderBoss") { 
                   Drill script = enemy.gameObject.GetComponent<Drill>();
                script.Damage(GameManager.Instance.PlayerAttackDamage);
            }

        }

        if(enemiesInAttackRange.Length == 0)
        {
            if(m_attackComboNumber == 4){
                SoundManager.Instance.PlayOnce((AudioClipName)((int)AudioClipName.PLAYER_ATTACK_3));
            }
            else
            {
                SoundManager.Instance.PlayOnce((AudioClipName)((int)AudioClipName.PLAYER_ATTACK_1 + (int)(m_attackComboNumber) -1));
            }
            
        }
    }

    void ChangeAnimationState(ATTACK_ANIMATION p_newState)
    {
        if (m_animationCurrentHash == m_animationHash[(int)p_newState]) return;   // stop the same animation from interrupting itself
        m_animator.Play(m_animationHash[(int)p_newState]);                // play the animation
        m_animationCurrentHash = m_animationHash[(int)p_newState];                // reassigning the new state
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z),M_ATTACK_RANGE);
    }

    public bool IsAttacking { get { return m_isAttacking;}}

}
