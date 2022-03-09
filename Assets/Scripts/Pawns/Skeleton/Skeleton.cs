using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skeleton : Enemy
{   
    Rigidbody2D m_rb2D;
    Collider2D m_collider2D;
    Animator m_animator;
    Timer boneTimer;
    int m_skeletonState;

    string m_moveAnimationName      = "Move";
    string m_reloadAnimationName    = "Reload";
    string m_fireAnimationName      = "Fire";
    string m_dieAnimationName       = "Die";
    string m_hitAnimationName       = "hit";
    int[] m_animationHash = new int[(int)ENEMY_ANIMATION.LAST_NO_USE];

    [SerializeField] GameObject prefabBone;
    GameObject Bone;
    BoneScript m_boneScript;
    private float boneCooldown = 2000.0f;

    [SerializeField] GameObject player;
    float m_playerPosX;

    [SerializeField] Transform left_limit;
    [SerializeField] Transform right_limit;
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
        m_skeletonState = Animator.StringToHash("state");
        m_health = 3;

        m_state = ENEMY_STATE.MOVE;
        m_isGrounded = true;
        m_playerIsNear = false;
        m_playerIsAtRange = false;

        m_spawnPosX = transform.position.x;
        m_hasReturned = true;

        m_collider2D = GetComponent<Collider2D>();
        Bone = Instantiate(prefabBone, new Vector3(0, 0, 0), Quaternion.identity);

        Physics2D.IgnoreLayerCollision(7, 7, true);
    }

    void Start()
    {
        m_animationHash[(int)ENEMY_ANIMATION.MOVE] = Animator.StringToHash(m_moveAnimationName);
        m_animationHash[(int)ENEMY_ANIMATION.RELOAD] = Animator.StringToHash(m_reloadAnimationName);
        m_animationHash[(int)ENEMY_ANIMATION.FIRE] = Animator.StringToHash(m_fireAnimationName);
        m_animationHash[(int)ENEMY_ANIMATION.DIE] = Animator.StringToHash(m_dieAnimationName);
        m_animationHash[(int)ENEMY_ANIMATION.HIT] = Animator.StringToHash(m_hitAnimationName);

        player = GameObject.FindGameObjectWithTag("Player");
        m_isFacingRight = false;

        m_boneScript = Bone.GetComponent<BoneScript>();
        boneTimer = gameObject.AddComponent<Timer>();
        Bone.SetActive(false);
        boneTimer.Duration = 2;
    }

    void Update()
    {
        switch (m_state)
        {
            default:break;
            case ENEMY_STATE.MOVE:      { Move(ENEMY_STATE.MOVE); } break;
            case ENEMY_STATE.CHASE:     { Chase(ENEMY_STATE.MOVE); } break;
            case ENEMY_STATE.ATTACK:    { Attack(ENEMY_STATE.CHASE); } break;
            case ENEMY_STATE.DIE:       { 
                    Die(); } break;
            case ENEMY_STATE.HIT:       { } break;
        }

        float delta = Time.fixedDeltaTime * 1000;
        boneCooldown += 1.0f * delta;
    }

    void Move(ENEMY_STATE p_defaultState)
    {
        ChangeAnimationState(m_animationHash[(int)ENEMY_ANIMATION.MOVE]);
        if (m_hasReturned == true)
        {
            m_rb2D.velocity = new Vector2(FacingDirection() * m_speed, m_rb2D.velocity.y);

            if (m_isGrounded == false) { FlipX(); }

            if (transform.position.x < left_limit.position.x)
            {
                transform.position = new Vector3(left_limit.position.x, transform.position.y, transform.position.z);
                FlipX();
            }
            if (transform.position.x > right_limit.position.x)
            {
                transform.position = new Vector3(right_limit.position.x, transform.position.y, transform.position.z);
                FlipX();
            }

            //Player near?
            if (m_playerIsNear == true)
            {
                m_hasReturned = false;
                m_state = ENEMY_STATE.CHASE;
            }
        }
        else
        {
            if (m_isGrounded == false) { FlipX(); }

            if (transform.position.x < m_spawnPosX - 4.0f || transform.position.x > m_spawnPosX + 4.0f)
            {
                m_rb2D.velocity = new Vector2(FacingDirection() * m_speed, m_rb2D.velocity.y);
            }
            else { m_hasReturned = true; }

            if (m_playerIsNear == true)
            {
                m_hasReturned = false;
                m_state = ENEMY_STATE.CHASE;
            }
        }
    }
    void Chase(ENEMY_STATE p_defaultState)
    {
        //Player Near but Unnaccesible
        if (m_playerIsNear && !m_playerIsAtRange && m_isGrounded == false) 
        { 
            m_rb2D.velocity = Vector2.zero;
            ChangeAnimationState(m_animationHash[(int)ENEMY_ANIMATION.RELOAD]);
        }
        else if (m_playerIsNear)
        {
            //Ready to Attack
            if (m_playerIsAtRange == true) { m_state = ENEMY_STATE.ATTACK; }
            //Chasing
            if (player.transform.position.x > transform.position.x && !m_isFacingRight) { FlipX(); }
            if (player.transform.position.x < transform.position.x && m_isFacingRight)  { FlipX(); }
            m_rb2D.velocity = new Vector2(FacingDirection() * (m_speed + 6), m_rb2D.velocity.y);
        }
        else { m_state = p_defaultState; }
    }
    void Attack(ENEMY_STATE p_defaultState)
    {
        if (player.transform.position.x > transform.position.x && !m_isFacingRight) { FlipX(); }
        if (player.transform.position.x < transform.position.x && m_isFacingRight)  { FlipX(); }

        ChangeAnimationState(m_animationHash[(int)ENEMY_ANIMATION.RELOAD]);
        m_rb2D.velocity = Vector2.zero;

        if (m_playerIsAtRange == true)
        {
            if (boneTimer.IsFinished)
            {
                //Bone.SetActive(true);
                BoneScript boneScript= Instantiate(prefabBone).GetComponent<BoneScript>();
                //SoundManager.Instance.PlayOnce(AudioClipName.ARCHER_ATTACK, 1.0f);
                boneScript.gameObject.transform.position = transform.position;
                boneScript.Shoot(FacingDirection(), AudioClipName.ARCHER_ATTACK);
                //Bone.transform.position = transform.position;
                //m_boneScript.Shoot(FacingDirection());
                boneTimer.Run();
            }
        }
        else
        {
            m_state = p_defaultState;
            ChangeAnimationState(m_animationHash[(int)ENEMY_ANIMATION.MOVE]);
        }
    }

    void Die()
    {
        ChangeAnimationState(m_animationHash[(int)ENEMY_ANIMATION.DIE]);
        m_collider2D.enabled = false;
        Destroy(gameObject, 0.5f);
    }

    void ChangeAnimationState(int p_newState)
    {
        if (m_skeletonState == p_newState && m_skeletonState != m_animationHash[(int)ENEMY_ANIMATION.HIT]) { return; }
        
        if (m_skeletonState == p_newState && m_skeletonState == m_animationHash[(int)ENEMY_ANIMATION.HIT]) {
            m_animator.Play(p_newState,-1,0);
        }
        else
        {
            m_animator.Play(p_newState);
            m_skeletonState = p_newState;
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
        m_state = ENEMY_STATE.HIT;
        ChangeAnimationState(m_animationHash[(int)ENEMY_ANIMATION.HIT]);
        base.Damage(p_damage);
    }

    void ReturnToNormalState()
    {
        m_state = ENEMY_STATE.MOVE;
        ChangeAnimationState(m_animationHash[(int)ENEMY_ANIMATION.MOVE]);
    }

    public ENEMY_STATE State { set { m_state = value; } get { return m_state; } }

    #region Accessors
    public bool IsGrounded
    {
        set { m_isGrounded = value; }
    }
    public bool IsPlayerNear
    {
        set { m_playerIsNear = value; }
    }
    public bool IsPlayerAtRange
    {
        set { m_playerIsAtRange = value; }
    }
    public bool IsFacingRight
    {
        get { return m_isFacingRight; }
    }
    #endregion
}