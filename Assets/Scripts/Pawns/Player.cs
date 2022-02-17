using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE, MOVE, DASH, JUMP, FALL, ATTACK, DEATH, CAST, GROUNDBREAKER }
public enum PLAYER_ANIMATION { IDLE, RUN, DASH, JUMP, FALL, BOOST, LAND, ATTACK_1, ATTACK_2, ATTACK_3, LAST_NO_USE}

public class Player : MonoBehaviour
{
    Skills m_skills;

    Animator m_animator;
    int m_currentState;

    string m_idleAnimationName = "idle";
    string m_runAnimationName = "run";
    string m_dashAnimationName = "dash";
    string m_boostAnimationName = "boost";
    string m_jumpAnimationName = "jump";
    string m_fallAnimationName = "fall";
    string m_landAnimationName = "land";
    string m_attack_1_AnimationName = "attack_1";
    string m_attack_2_AnimationName = "attack_2";
    string m_attack_3_AnimationName = "attack_3";

    int[] m_animationHash = new int[(int)PLAYER_ANIMATION.LAST_NO_USE];

    void ChangeAnimationState(int p_newState)
    {
        if (m_currentState == p_newState) return;   // stop the same animation from interrupting itself
        m_animator.Play(p_newState);                // play the animation
        m_currentState = p_newState;                // reassigning the new state
    }

    PLAYER_STATE m_state;

    Rigidbody2D m_rb2D;
    [SerializeField] float m_speed = 50.0f;
    float m_direction;
    bool m_canMoveHorizontal = true;

    float m_dashCurrentTime;
    [SerializeField] float m_dashDistance = 100;
    [SerializeField] float m_dashDuration = 0.5f;
    float m_dashSpeed = 200.0f;

    public float m_maxHeight = 10.0f;
    public float m_timeToPeak1 = 1f;
    public float m_timeToPeak2 = 1f;
    float m_gravity1;
    float m_gravity2;
    float m_initialVelocityY;
    bool m_isGrounded;
    
    int m_playerStateID;
    bool m_isFacingRight;

    float m_attackDuration = 0;
    float m_currentAttackDuration = 0;
    [SerializeField] Transform m_attackPosition;
    [SerializeField] LayerMask m_enemyLayer;
    const float M_ATTACK_RANGE = 16;
    bool m_keepAttacking = false;
    int m_keepAttackingID;
    int m_attackComboCount;

    private void Awake()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_playerStateID = Animator.StringToHash("state");
        
        m_state = PLAYER_STATE.IDLE;
        m_direction = 0;

        m_dashCurrentTime = 0;
        m_dashSpeed = m_dashDistance / m_dashDuration;

        m_gravity1 = -2 * m_maxHeight / (m_timeToPeak1 * m_timeToPeak1);
        m_gravity2 = -2 * m_maxHeight / (m_timeToPeak2 * m_timeToPeak2);
        m_initialVelocityY = 2 * m_maxHeight / m_timeToPeak1;

        m_isGrounded = false;

        m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
        m_skills = gameObject.GetComponent<Skills>();
    }

    private void Start()
    {
        m_isFacingRight = true;
        m_keepAttackingID = Animator.StringToHash("nextAttack");

        m_animationHash[(int)PLAYER_ANIMATION.IDLE] = Animator.StringToHash(m_idleAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.RUN] = Animator.StringToHash(m_runAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.DASH] = Animator.StringToHash(m_dashAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.BOOST] = Animator.StringToHash(m_boostAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.JUMP] = Animator.StringToHash(m_jumpAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.FALL] = Animator.StringToHash(m_fallAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.LAND] = Animator.StringToHash(m_landAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.ATTACK_1] = Animator.StringToHash(m_attack_1_AnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.ATTACK_2] = Animator.StringToHash(m_attack_2_AnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.ATTACK_3] = Animator.StringToHash(m_attack_3_AnimationName);
    }

    private void Update()
    {
        switch (m_state)
        {
            default: break;
            case PLAYER_STATE.IDLE:
                {
                    Move(PLAYER_STATE.IDLE);
                    Attack();
                    Dash();
                    Jump();
                }
                break;

            case PLAYER_STATE.MOVE:
                {
                    Move(PLAYER_STATE.IDLE);
                    Attack();
                    Jump();
                    Dash();
                }
                break;

            case PLAYER_STATE.DASH:
                {
                    Dash();
                }
                break;
            case PLAYER_STATE.JUMP:
                {
                    Move(PLAYER_STATE.JUMP);
                }
                break;
            case PLAYER_STATE.FALL:
                {
                    Move(PLAYER_STATE.FALL);
                }
                break;
            case PLAYER_STATE.ATTACK:
                {
                    Attack();
                }
                break;
            case PLAYER_STATE.CAST:
                break;
            case PLAYER_STATE.GROUNDBREAKER:
                break;

            case PLAYER_STATE.DEATH: { } break;
        }
    }

    void Attack()
    {
        if (InputManager.Instance.AttackButtonPressed)
        {
            m_keepAttacking = true;
        }

        if (!m_isGrounded) { m_keepAttacking = false; }

        if (m_keepAttacking && m_currentAttackDuration == 0)
        {
            if (!m_isGrounded) { return; }
            if(m_state != PLAYER_STATE.ATTACK) { SetPlayerState(PLAYER_STATE.ATTACK); }
            m_rb2D.velocity = Vector2.zero;
            ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.ATTACK_1 + m_attackComboCount]);
            
            Collider2D[] enemiesInAttackRange = Physics2D.OverlapCircleAll(m_attackPosition.position, M_ATTACK_RANGE, m_enemyLayer);
            m_keepAttacking = false;
            foreach (Collider2D enemy in enemiesInAttackRange)
            {
                Debug.Log(enemy.gameObject.name + " hitted.");
            }
        }
        else if (m_state == PLAYER_STATE.ATTACK)
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                m_currentAttackDuration += Time.deltaTime;
            }
            else
            {
                if (m_keepAttacking && m_attackComboCount < 2)
                {
                    m_currentAttackDuration = 0;
                    m_attackComboCount++;
                }
                else
                {
                    m_currentAttackDuration = 0;
                    m_keepAttacking = false;
                    SetPlayerState(PLAYER_STATE.IDLE);
                    ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.IDLE]);
                    m_attackComboCount = 0;
                }
            }
        }
    }

    void Jump()
    {
          if (InputManager.Instance.JumpButtonPressed && m_isGrounded) {
            m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
            m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, m_initialVelocityY);

            SetPlayerState(PLAYER_STATE.JUMP);
            ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.JUMP]);
            m_isGrounded = false;
          }
    }

    void Dash()
    {
        if (m_state == PLAYER_STATE.DASH)
        {
            m_dashCurrentTime += Time.deltaTime;
            Debug.Log(m_dashCurrentTime);
            if (m_dashCurrentTime > m_dashDuration)
            {
                m_dashCurrentTime = 0;
                m_state = PLAYER_STATE.IDLE;
                ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.DASH]);
            }
        }

        if (InputManager.Instance.DashButtonPressed && m_state != PLAYER_STATE.DASH)
        {
            m_rb2D.velocity = new Vector2(FacingDirection() * m_dashSpeed, m_rb2D.velocity.y);

            ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.DASH]);
            m_state = PLAYER_STATE.DASH;
        }
    }

    void Move(PLAYER_STATE p_defaultState)
    {
        float horizontalAxisValue = Input.GetAxisRaw("Horizontal");
        if (horizontalAxisValue != 0)
        {
            if (!m_isFacingRight && horizontalAxisValue > 0)    { FlipX(); }
            if (m_isFacingRight && horizontalAxisValue < 0)     { FlipX(); }

            if (m_isGrounded) {
                m_state = PLAYER_STATE.MOVE;
                ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.RUN]);

            }
        }
        else
        {
            m_state = p_defaultState;
            ChangeAnimationState(m_animationHash[(int)p_defaultState]);
        }

        m_direction = horizontalAxisValue;

        m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);

        if (m_rb2D.velocity.y < 0)
        {
            SetPlayerState(PLAYER_STATE.FALL);
            SetPlayerAnimation(PLAYER_ANIMATION.FALL);
        }

    }

    public void SetPlayerState(PLAYER_STATE value)
    {
        m_state = value;
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }

    int FacingDirection()
    {
        if (m_isFacingRight) return 1;
        else return -1;
    }

    #region Accessors

    public bool IsFacingRight
    {
        get { return m_isFacingRight; }
    }

    public void SetPlayerAnimation(PLAYER_ANIMATION p_animation)
    {
        ChangeAnimationState(m_animationHash[(int)p_animation]);
    }

    public PLAYER_STATE State
    {
        get { return m_state; }
        set { m_state = value; }
    }

    public float Gravity1
    {
        get { return m_gravity1; }
    }

    public float Gravity2
    {
        get { return m_gravity2; }
    }

    public bool IsGrounded
    {
        set { m_isGrounded = value; }
        get { return m_isGrounded; }
    }

    public bool CanMoveHorizontal
    {
        set { m_canMoveHorizontal = value; }
    }

    #endregion
}
