using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE, MOVE, DASH, JUMP, FALL, BOOST, LAND, ATTACK, DEATH, CAST, GROUNDBREAKER, HURT }
public enum PLAYER_ANIMATION { IDLE, RUN, DASH, JUMP, FALL, BOOST, LAND, HIT, GROUNDBREAKER, GROUNDBREAKER_LOOP, LAST_NO_USE}

public class Player : MonoBehaviour
{
    [SerializeField] Transicion m_transicionScript;

    float m_reducedMovementSpeed = 30.0f;
    bool m_hasUsedDash = false;
    string m_objectGroundedTo;
    SpriteRenderer m_spriteRenderer;
    bool m_isUsingGroundBreaker = false;

    int m_health = 5;

    bool m_isInvulnerable = false;
    Timer m_invulnerableTimer;

    bool m_canIMove = false;
    Timer m_noControlTimer;

    Skill_Pilar m_skills;

    Animator m_animator;
    int m_currentState;

    string m_idleAnimationName      = "idle";
    string m_runAnimationName       = "run";
    string m_dashAnimationName      = "dash";
    string m_boostAnimationName     = "boost";
    string m_jumpAnimationName      = "jump";
    string m_fallAnimationName      = "fall";
    string m_landAnimationName      = "land";
    string m_attack_1_AnimationName = "attack_1";
    string m_attack_2_AnimationName = "attack_2";
    string m_attack_3_AnimationName = "attack_3";
    string m_hitAnimationName       = "hit";
    string m_groundbreakerAnimationName       = "groundbreaker";
    string m_groundbreakerLoopAnimationName       = "groundbreakerLoop";
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
    [SerializeField] float m_dashDuration = 3/6f;
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
    const float M_ATTACK_RANGE = 8;
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
        m_skills = gameObject.GetComponent<Skill_Pilar>();

        m_invulnerableTimer = gameObject.AddComponent<Timer>();
        m_invulnerableTimer.Duration = 1f;
        m_noControlTimer = gameObject.AddComponent<Timer>();
        m_noControlTimer.Duration = 0.5f;

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_objectGroundedTo = "";
    }

    private void Start()
    {
        m_isFacingRight = true;
        m_keepAttackingID = Animator.StringToHash("nextAttack");

        m_animationHash[(int)PLAYER_ANIMATION.IDLE]     = Animator.StringToHash(m_idleAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.RUN]      = Animator.StringToHash(m_runAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.DASH]     = Animator.StringToHash(m_dashAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.BOOST]    = Animator.StringToHash(m_boostAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.JUMP]     = Animator.StringToHash(m_jumpAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.FALL]     = Animator.StringToHash(m_fallAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.LAND]     = Animator.StringToHash(m_landAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.HIT]      = Animator.StringToHash(m_hitAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.GROUNDBREAKER]      = Animator.StringToHash(m_groundbreakerAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.GROUNDBREAKER_LOOP]      = Animator.StringToHash(m_groundbreakerLoopAnimationName);
    }

    private void Update()
    {
        if (Time.timeScale == 0) { return; } //Game Paused

        if (m_invulnerableTimer.IsFinished && m_isInvulnerable && m_state != PLAYER_STATE.DASH)
        {
            m_isInvulnerable = false;
            m_spriteRenderer.color = Color.white;
            Physics2D.IgnoreLayerCollision(6, 7, false);
        }

        if (m_noControlTimer.IsFinished) { m_canIMove = true; }

        switch (m_state)
        {
            default: break;
            case PLAYER_STATE.IDLE:
                {
                    Move(PLAYER_STATE.IDLE);
                    Jump();
                    Dash();
                } break;
            case PLAYER_STATE.MOVE:
                {
                    Move(PLAYER_STATE.IDLE);
                    Jump();
                    Dash();
                } break;
            case PLAYER_STATE.DASH:     { Dash(); } break;
            case PLAYER_STATE.BOOST:
                {
                    Move(PLAYER_STATE.BOOST);
                    Dash();
                }
                break;
            case PLAYER_STATE.JUMP:     
                {
                    Move(PLAYER_STATE.JUMP);
                    Dash();
                } break;
            case PLAYER_STATE.FALL:     
                {
                    Move(PLAYER_STATE.FALL);
                    Dash();
                } break;
            case PLAYER_STATE.LAND:
                {
                    Move(PLAYER_STATE.LAND);
                    Jump();
                    Dash();
                }
                break;
            case PLAYER_STATE.ATTACK: { 
                    Move(PLAYER_STATE.ATTACK); } break;
        }
    }



    void Jump()
    {
          if (InputManager.Instance.JumpButtonPressed && m_isGrounded && m_objectGroundedTo != "enemy") {
            m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
            m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, m_initialVelocityY);

            SetPlayerState(PLAYER_STATE.BOOST);
            ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.BOOST]);
            m_isGrounded = false;
          }
    }

    void Dash()
    {
        if (m_state == PLAYER_STATE.DASH)
        {
            m_dashCurrentTime += Time.deltaTime;
            if (m_dashCurrentTime > m_dashDuration)
            {
                m_dashCurrentTime = 0;
                m_state = PLAYER_STATE.IDLE;
                m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
                ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.DASH]);
                Physics2D.IgnoreLayerCollision(6,7,false);
            }
        }

        if (InputManager.Instance.DashButtonPressed && m_state != PLAYER_STATE.DASH && m_noControlTimer.IsFinished && !m_hasUsedDash)
        {
            m_rb2D.velocity = new Vector2(FacingDirection() * m_dashSpeed, 0);
            ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.DASH]);
            m_state = PLAYER_STATE.DASH;
            m_rb2D.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(6, 7, true);
            m_hasUsedDash = true;
        }
    }

    void Move(PLAYER_STATE p_defaultState)
    {
        float horizontalAxisValue = Input.GetAxisRaw("Horizontal");
        if (horizontalAxisValue != 0 && m_canIMove)
        {
            if (!m_isFacingRight && horizontalAxisValue > 0)    { FlipX(); }
            if (m_isFacingRight && horizontalAxisValue < 0)     { FlipX(); }

            if (m_isGrounded && m_state != PLAYER_STATE.LAND && m_state != PLAYER_STATE.ATTACK) {
                m_state = PLAYER_STATE.MOVE;
                ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.RUN]);
            }
        }
        else
        {
            m_state = p_defaultState;
            if(m_state != PLAYER_STATE.ATTACK)
            {
                ChangeAnimationState(m_animationHash[(int)p_defaultState]);
            }
        }

        if (m_canIMove)
        {
            m_direction = horizontalAxisValue;
            m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);
        }

        if (m_rb2D.velocity.y < 0 && m_state != PLAYER_STATE.ATTACK)
        {
            m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
            SetPlayerState(PLAYER_STATE.FALL);
            SetPlayerAnimation(PLAYER_ANIMATION.FALL);
        }

        if (m_rb2D.velocity.y < -200) { m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, -200.0f); }
    }

    public void SetPlayerState(PLAYER_STATE value) { m_state = value; }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;       
    }

    public int FacingDirection()
    {
        if (m_isFacingRight) return 1;
        else return -1;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy" && !m_isInvulnerable)
        {
            float velocityX = -50*(collision.gameObject.transform.position - transform.position).normalized.x / Mathf.Abs((collision.gameObject.transform.position - transform.position).normalized.x);
            float velocityY = 100;

            m_rb2D.velocity = new Vector2(velocityX, velocityY);
            m_isInvulnerable = true;
            m_invulnerableTimer.Run();
            m_noControlTimer.Duration = 0.5f;
            m_noControlTimer.Run();
            m_canIMove = false;
            m_spriteRenderer.color = Color.black;
            m_state = PLAYER_STATE.JUMP;
            m_isGrounded = false;
            Physics2D.IgnoreLayerCollision(6, 7, true);
            GameManager.Instance.ModifyHealthUI(false);
            ModifyHP(-1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemyProjectile" && !m_isUsingGroundBreaker && !m_isInvulnerable && m_state != PLAYER_STATE.DASH)
        {
            float velocityX = -50 * (collision.gameObject.transform.position - transform.position).normalized.x / Mathf.Abs((collision.gameObject.transform.position - transform.position).normalized.x);
            float velocityY = 100;

            m_rb2D.velocity = new Vector2(velocityX, velocityY);
            m_isInvulnerable = true;
            m_invulnerableTimer.Run();
            m_noControlTimer.Duration = 0.2f;
            m_noControlTimer.Run();
            m_canIMove = false;
            collision.gameObject.SetActive(false);
            m_spriteRenderer.color = Color.black;
            m_state = PLAYER_STATE.JUMP;
            m_isGrounded = false;
            GameManager.Instance.ModifyHealthUI(false);
            ModifyHP(-1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "spike")
        {
            m_health--;
            m_transicionScript.LocalCheckpointTransition();
        }
    }

    public void ModifyHP(int p_healthModifier)
    {
        m_health += p_healthModifier;
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
        set { m_state = value;
            if(m_state == PLAYER_STATE.ATTACK) { m_rb2D.velocity = new Vector2(m_speed, m_rb2D.velocity.y); }
        }
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

    public bool IsUsingGroundBreaker
    {
        set { m_isUsingGroundBreaker = value; }
    }

    public string ObjectGroundedTo { set { m_objectGroundedTo = value; } }

    public Vector3 Speed { get { return m_rb2D.velocity; } }

    public void ReduceSpeed()
    {
        if (IsGrounded) { m_speed = 0; }
        else { m_speed = m_reducedMovementSpeed; }
    }

    public void SetToNormalSpeed() { m_speed = 60; }

    public bool HasUsedDash
    {
        set { m_hasUsedDash = value; }
    }

    #endregion
}