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
    private bool m_hasReturned;

    [SerializeField] float m_speed = 50.0f;
    [SerializeField] bool m_isFacingRight;
    bool m_isGrounded;
    bool m_playerIsNear;
    bool m_playerIsAtRange;

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
    }

    void Update()
    {
        switch (m_ghoulState)
        {
            default:break;
            case GHOUL_STATE.IDLE:      { Idle(); } break;
            case GHOUL_STATE.CHASE:     { Move(); } break;
            case GHOUL_STATE.ATTACK:    { Attack(); } break;
            case GHOUL_STATE.HIT:       { } break;
            case GHOUL_STATE.DIE:       { Die(); } break;
        }
    }

    void Idle()
    {

    }
    void Move()
    {

    }
    void Attack()
    {

    }
    void Die()
    {

    }
}