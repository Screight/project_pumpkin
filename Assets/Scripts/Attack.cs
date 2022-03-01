using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Player m_player;

    public enum ATTACK_ANIMATION { NO_ATTACK,ATTACK_1, ATTACK_2, ATTACK_3, LAST_NO_USE}
    string m_noAttack_AnimationName = "noAttack";
    string m_attack_1_AnimationName = "attack_1";
    string m_attack_2_AnimationName = "attack_2";
    string m_attack_3_AnimationName = "attack_3";
    Animator m_animator;
    int[] m_animationHash = new int[(int)ATTACK_ANIMATION.LAST_NO_USE];
    int m_currentState;
    int m_attackComboNumber = 0; // represents first attack(0), second(1) and third(2)

    Timer m_windowToComboTimer;
    float m_windowToComboDuration = 1f;

    const float M_ATTACK_RANGE = 8;
    [SerializeField] LayerMask m_enemyLayer;
    bool m_keepAttacking;
    bool m_isAttacking;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_windowToComboTimer = gameObject.AddComponent<Timer>();
        m_player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        m_animationHash[(int)ATTACK_ANIMATION.NO_ATTACK] = Animator.StringToHash(m_noAttack_AnimationName);
        m_animationHash[(int)ATTACK_ANIMATION.ATTACK_1] = Animator.StringToHash(m_attack_1_AnimationName);
        m_animationHash[(int)ATTACK_ANIMATION.ATTACK_2] = Animator.StringToHash(m_attack_2_AnimationName);
        m_animationHash[(int)ATTACK_ANIMATION.ATTACK_3] = Animator.StringToHash(m_attack_3_AnimationName);

        m_windowToComboTimer.Duration = m_windowToComboDuration;
    }

    private void Update()
    {
        switch (m_player.State)
        {
            case PLAYER_STATE.IDLE:
                Attack1();
                break;
            case PLAYER_STATE.ATTACK:
                Attack1();
                break;
            case PLAYER_STATE.MOVE:
                Attack1();
                break;
            case PLAYER_STATE.LAND:
                Attack1();
                break;
            case PLAYER_STATE.BOOST:
                //Attack1();
                break;
            case PLAYER_STATE.JUMP:
                //Attack1();
                break;
            case PLAYER_STATE.FALL:
                //Attack1();
                break;
            default: break;

        }
    }

    void Attack1()
    {
        
        if (InputManager.Instance.AttackButtonPressed)
        {
            if (m_player.State != PLAYER_STATE.ATTACK && m_player.IsGrounded) {
                m_player.State = PLAYER_STATE.ATTACK;
                m_player.ReduceSpeed();
            }

            m_keepAttacking = true;
            if (!m_isAttacking /*&& m_windowToComboTimer.IsFinished*/)
            {
                TransitionToNextComboAttack();
                m_windowToComboTimer.Run();
                m_isAttacking = true;
            }
        }

        if (m_player.State == PLAYER_STATE.ATTACK)
        {
            if (m_player.IsGrounded)
            {
                m_player.ReduceSpeed();
            }
            else
            {
                m_player.SetToNormalSpeed();
            }

        }

        //if (m_windowToComboTimer.IsRunning && !m_isAttacking && m_keepAttacking)
        //{
        //    TransitionToNextComboAttack();
        //}
        //else if (m_windowToComboTimer.IsFinished)
        //{
        //    m_keepAttacking = false;
        //}

    }

    void Hit()
    {
        float offset = 2 * m_player.FacingDirection();

        Collider2D[] enemiesInAttackRange = Physics2D.OverlapCircleAll(new Vector2(transform.position.x - offset, transform.position.y), M_ATTACK_RANGE, m_enemyLayer);
        foreach (Collider2D enemy in enemiesInAttackRange)
        {
            if (enemy.gameObject.tag == "enemy") { enemy.gameObject.GetComponent<Enemy>().Damage(1); }
            Debug.Log("Enemy hit");
        }

        if(enemiesInAttackRange.Length == 0)
        {

            SoundManager.Instance.PlayOnce((AudioClipName)((int)AudioClipName.PLAYER_ATTACK_1 + (int)(m_attackComboNumber) -1), 1f);
        }
    }

    void TransitionToNextComboAttack()
    {
        if (m_keepAttacking)
        {
            if(m_attackComboNumber < 3) {
                m_attackComboNumber++;
            }
            else { m_attackComboNumber = 1; }
            ChangeAnimationState(m_animationHash[m_attackComboNumber]);
            m_keepAttacking = false;
            m_windowToComboTimer.Stop();
            m_windowToComboTimer.Restart();
        }
        else
        {
            m_isAttacking = false;
            m_keepAttacking = false;
            m_player.State = PLAYER_STATE.IDLE;
            ChangeAnimationState(m_animationHash[0]);
            m_attackComboNumber = 0;
            m_player.SetToNormalSpeed();
        }
        
    }

    void ChangeAnimationState(int p_newState)
    {
        if (m_currentState == p_newState) return;   // stop the same animation from interrupting itself
        m_animator.Play(p_newState);                // play the animation
        m_currentState = p_newState;                // reassigning the new state
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z),M_ATTACK_RANGE);
    }

}
