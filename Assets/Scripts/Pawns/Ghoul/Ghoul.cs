using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GHOUL_STATE { IDLE, CHASE, ATTACK, DIE, HIT, AIR }
public enum GHOUL_ANIMATION { IDLE, MOVE, ATTACK, DIE, HIT, LAST_NO_USE }

public class Ghoul : Enemy
{
    Rigidbody2D m_rb2D;
    Collider2D m_collider2D;
    Animator m_animator;
    GHOUL_STATE m_ghoulState;
    int m_currentState;

    string m_idleAnimationName = "Idle";
    string m_moveAnimationName = "Move";
    string m_attackAnimationName = "Attack";
    string m_dieAnimationName = "Die";
    string m_hitAnimationName = "Hit";
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
        m_animator = GetComponent<Animator>();
        m_health = 5;

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}