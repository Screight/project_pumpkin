using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : Enemy
{
    Rigidbody2D m_rb2D;
    Collider2D m_collider2D;
    Animator m_animator;
    int m_ghoulState;

    string m_idleAnimationName = "Idle";
    string m_moveAnimationName = "Move";
    string m_attackAnimationName = "Attack";
    string m_dieAnimationName = "Die";
    string m_hitAnimationName = "Hit";
    int[] m_animationHash = new int[(int)ENEMY_ANIMATION.LAST_NO_USE];

    [SerializeField] GameObject player;
    float m_playerPosX;
    private float m_spawnPosX;
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
        m_ghoulState = Animator.StringToHash("state");
        m_health = 5;

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
