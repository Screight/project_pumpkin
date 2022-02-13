using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PLAYER_STATE { IDLE, MOVING, DASH, DEATH, JUMP, ATTACK }

public class Player : MonoBehaviour
{
    
    PLAYER_STATE m_state;

    Rigidbody2D m_rb2D;
    [SerializeField] float m_speed = 50.0f;
    float m_direction;

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

    Animator m_animator;
    int m_playerState;
    bool m_isFacingRight;

    CircleCollider2D m_attackRangeCollider;
    float m_attackDuration = 0;
    float m_currentAttackDuration = 0;

    private void Awake()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_playerState = Animator.StringToHash("state");

        
        m_state = PLAYER_STATE.IDLE;
        m_direction = 0;

        m_dashCurrentTime = 0;
        m_dashSpeed = m_dashDistance / m_dashDuration;

        m_gravity1 = -2 * m_maxHeight / (m_timeToPeak1 * m_timeToPeak1);
        m_gravity2 = -2 * m_maxHeight / (m_timeToPeak2 * m_timeToPeak2);
        m_initialVelocityY = 2 * m_maxHeight / m_timeToPeak1;

        m_isGrounded = false;

        m_attackRangeCollider = GetComponentInChildren<CircleCollider2D>();
    }

    private void Start()
    {
        m_isFacingRight = true;
        m_attackRangeCollider.enabled = false;

    }

    private void Update()
    {
        switch (m_state)
        {
            case PLAYER_STATE.IDLE:
                {
                    
                    Move(PLAYER_STATE.IDLE);
                    Attack_1();
                    Dash();
                    Jump();
                }
                break;

            case PLAYER_STATE.MOVING:
                {
                    Move(PLAYER_STATE.IDLE);
                    Attack_1();
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
                    Dash();
                }
                break;
            case PLAYER_STATE.ATTACK:
                {
                    Attack_1();
                }
                break;

            case PLAYER_STATE.DEATH: { } break;
        }
    }

    void Attack_1()
    {
        
        if (InputManager.Instance.AttackButtonPressed)
        {
            if (!m_isGrounded) { return; }
            if (m_state != PLAYER_STATE.ATTACK)
            {
                SetPlayerState(PLAYER_STATE.ATTACK);
                m_attackRangeCollider.enabled = true;
                m_attackDuration = m_animator.GetCurrentAnimatorStateInfo(0).length;
                
            }
        }
        else if (m_state == PLAYER_STATE.ATTACK)
        {
            if (m_currentAttackDuration < m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
            {
                m_currentAttackDuration += Time.deltaTime;
            }
            else
            {
                m_currentAttackDuration = 0;
                if (m_attackRangeCollider.enabled == true) { m_attackRangeCollider.enabled = false; }
                SetPlayerState(PLAYER_STATE.IDLE);
            }
        }
        
        
    }

    void Attack_2()
    {
        m_attackRangeCollider.enabled = true ;
    }

    void Attack_3()
    {
        m_attackRangeCollider.enabled = true;
    }

    void Jump()
    {
          if (InputManager.Instance.JumpButtonPressed && m_isGrounded)
        {
            m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
            m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, m_initialVelocityY);
            SetPlayerState(PLAYER_STATE.JUMP);
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
                m_animator.SetInteger(m_playerState, (int)m_state);
            }
        }

        if (InputManager.Instance.DashButtonPressed && m_state != PLAYER_STATE.DASH)
        {
            m_rb2D.velocity = new Vector2(FacingDirection() * m_dashSpeed, m_rb2D.velocity.y);

            m_state = PLAYER_STATE.DASH;
            m_animator.SetInteger(m_playerState, (int)m_state);
        }

    }

    void Move(PLAYER_STATE p_defaultState)
    {
        float horizontalAxisValue = Input.GetAxisRaw("Horizontal");
        if (horizontalAxisValue != 0)
        {

            if (!m_isFacingRight && horizontalAxisValue > 0)
            {
                FlipX();
            }

            if (m_isFacingRight && horizontalAxisValue < 0)
            {
                FlipX();
            }

            m_state = PLAYER_STATE.MOVING;
            m_animator.SetInteger(m_playerState, (int)m_state);
        }
        else
        {
            m_animator.SetBool(m_playerState, false);
            m_state = p_defaultState;
            m_animator.SetInteger(m_playerState, (int)m_state);
        }

        m_direction = horizontalAxisValue;
        m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);

    }

    public void SetPlayerState(PLAYER_STATE value)
    {
        m_state = value;
        m_animator.SetInteger(m_playerState, (int)value);
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

    public float Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

    public float Gravity1
    {
        get
        {
            return m_gravity1;
        }
    }

    public float Gravity2
    {
        get
        {
            return m_gravity2;
        }
    }

    public bool IsGrounded
    {
        set
        {
            m_isGrounded = value;
        }
    }

}
