using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GHOUL_STATE { IDLE, CHASE, ATTACK, HIT, DIE}
public enum GHOUL_ANIMATION { IDLE, MOVE, ATTACK, HIT, DIE, LAST_NO_USE }

public class Ghoul : Enemy
{
    Rigidbody2D m_rb2D;
    Collider2D m_collider2D;
    Animator m_animator;
    GHOUL_STATE m_ghoulState;
    int m_currentState;

    string m_idleAnimationName      = "Idle";
    string m_moveAnimationName      = "Move";
    string m_attackAnimationName    = "Attack";
    string m_hitAnimationName       = "Hit";
    string m_dieAnimationName       = "Die";
    int[] m_animationHash = new int[(int)GHOUL_ANIMATION.LAST_NO_USE];

    [SerializeField] GameObject player;
    float m_playerPosX;
    bool hasCharged;
    Timer chargeTimer;

    [SerializeField] float m_speed = 80.0f;
    [SerializeField] bool m_isFacingRight;
    public bool m_isGrounded;
    public bool m_playerIsNear;
    public bool m_playerIsAtRange;

    protected override void Awake()
    {
        base.Awake();

        m_rb2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
        m_animator = GetComponent<Animator>();
        m_health = 5;

        m_isGrounded = true;
        m_playerIsNear = false;
        m_playerIsAtRange = false;
        hasCharged = false;

        Physics2D.IgnoreLayerCollision(7, 7, true);
    }

    void Start()
    {
        m_animationHash[(int)GHOUL_ANIMATION.IDLE]      = Animator.StringToHash(m_idleAnimationName);
        m_animationHash[(int)GHOUL_ANIMATION.MOVE]      = Animator.StringToHash(m_moveAnimationName);
        m_animationHash[(int)GHOUL_ANIMATION.ATTACK]    = Animator.StringToHash(m_attackAnimationName);
        m_animationHash[(int)GHOUL_ANIMATION.HIT]       = Animator.StringToHash(m_hitAnimationName);
        m_animationHash[(int)GHOUL_ANIMATION.DIE]       = Animator.StringToHash(m_dieAnimationName);

        player = GameObject.FindGameObjectWithTag("Player");
        m_isFacingRight = false;
        chargeTimer = gameObject.AddComponent<Timer>();
        chargeTimer.Duration = 1;
    }

    void Update()
    {
        if (Time.timeScale == 0) { return; } //Game Paused
        switch (m_ghoulState)
        {
            default:break;
            case GHOUL_STATE.IDLE:      { Idle(); } break;
            case GHOUL_STATE.CHASE:     { Move(GHOUL_STATE.IDLE); } break;
            case GHOUL_STATE.ATTACK:    { Attack(GHOUL_STATE.CHASE); } break;
            case GHOUL_STATE.HIT:       { } break;
            case GHOUL_STATE.DIE:       { Die(); } break;
        }
    }

    void Idle()
    {
        ChangeAnimationState(m_animationHash[(int)GHOUL_ANIMATION.IDLE]);
        m_rb2D.velocity = Vector2.zero;

        if (m_playerIsNear) { m_ghoulState = GHOUL_STATE.CHASE; }
    }
    void Move(GHOUL_STATE p_defaultState)
    {
        if (player.transform.position.x > transform.position.x && !m_isFacingRight) { FlipX(); }
        if (player.transform.position.x < transform.position.x && m_isFacingRight) { FlipX(); }
        ChangeAnimationState(m_animationHash[(int)GHOUL_ANIMATION.MOVE]);
        //Player Near but Unnaccesible
        if (m_playerIsNear && !m_playerIsAtRange && !m_isGrounded)
        {
            m_ghoulState = p_defaultState;
        }
        else if (m_playerIsNear)
        {
            //Ready to Attack
            if (m_playerIsAtRange && m_isGrounded)
            {
                m_playerPosX = player.transform.position.x;
                chargeTimer.Run();
                m_ghoulState = GHOUL_STATE.ATTACK;
                ChangeAnimationState(m_animationHash[(int)GHOUL_ANIMATION.IDLE]);
            }
            //Chasing
            m_rb2D.velocity = new Vector2(FacingDirection() * m_speed, m_rb2D.velocity.y);
        }
        else { m_ghoulState = p_defaultState; }
    }
    void Attack(GHOUL_STATE p_defaultState)
    {
        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);

        m_rb2D.velocity = Vector2.zero;
        if (chargeTimer.IsFinished) { hasCharged = true; }

        if ((m_isFacingRight && transform.position.x <= m_playerPosX+15) || (!m_isFacingRight && transform.position.x >= m_playerPosX-15))
        {
            if (hasCharged && m_isGrounded)
            {
                m_animator.speed = 55 / distance;

                ChangeAnimationState(m_animationHash[(int)GHOUL_ANIMATION.ATTACK]);
                m_rb2D.velocity = new Vector2(FacingDirection() * m_speed * 2, m_rb2D.velocity.y);
                if (!m_isGrounded) { m_ghoulState = p_defaultState; chargeTimer.Stop(); hasCharged = false; m_animator.speed = 1; }
            }else if(!m_isGrounded) { m_ghoulState = p_defaultState; chargeTimer.Stop(); hasCharged = false; m_animator.speed = 1; }
        }
        else { m_ghoulState = p_defaultState; chargeTimer.Stop(); hasCharged = false; m_animator.speed = 1; }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    void ChangeAnimationState(int p_newState)
    {
        if (m_currentState == p_newState && m_currentState != m_animationHash[(int)GHOUL_ANIMATION.HIT]) { return; }
        if (m_currentState == p_newState && m_currentState == m_animationHash[(int)GHOUL_ANIMATION.HIT]) { m_animator.Play(p_newState, -1, 0); }
        else
        {
            m_animator.Play(p_newState);
            m_currentState = p_newState;
        }
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
        m_isGrounded = true;
    }
    public int FacingDirection()
    {
        if (m_isFacingRight) { return 1; }
        else { return -1; }
    }

    public override void Damage(int p_damage)
    {
        m_ghoulState = GHOUL_STATE.HIT;
        ChangeAnimationState(m_animationHash[(int)GHOUL_ANIMATION.HIT]);
        base.Damage(p_damage);
    }

    public GHOUL_STATE State 
    { 
        set { m_ghoulState = value; } 
        get { return m_ghoulState; } 
    }

    #region Accessors
    public bool IsGrounded { set { m_isGrounded = value; } }
    public bool IsPlayerNear { set { m_playerIsNear = value; } }
    public bool IsPlayerAtRange { set { m_playerIsAtRange = value; } }
    public bool IsFacingRight { get { return m_isFacingRight; } }
    #endregion
}