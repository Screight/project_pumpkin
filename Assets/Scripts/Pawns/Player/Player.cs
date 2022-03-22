using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE, MOVE, DASH, JUMP, FALL, BOOST, LAND, ATTACK, DEATH, CAST, GROUNDBREAKER, HURT }
public enum PLAYER_ANIMATION { IDLE, RUN, DASH, JUMP, FALL, BOOST, LAND, HIT, GROUNDBREAKER, GROUNDBREAKER_LOOP, LAST_NO_USE}

public class Player : MonoBehaviour
{
    #region Animation

    string m_idleAnimationName      = "idle";
    string m_runAnimationName       = "run";
    string m_dashAnimationName      = "dash";
    string m_boostAnimationName     = "boost";
    string m_jumpAnimationName      = "jump";
    string m_fallAnimationName      = "fall";
    string m_landAnimationName      = "land";
    string m_hitAnimationName       = "hit";
    string m_groundbreakerAnimationName       = "groundbreaker";
    string m_groundbreakerLoopAnimationName       = "groundbreakerLoop";
    int[] m_animationHash = new int[(int)PLAYER_ANIMATION.LAST_NO_USE];

    Animator m_animator;
    PLAYER_ANIMATION m_animationState;
    int m_currentAnimationHash = 0;

    public void ChangeAnimationState(PLAYER_ANIMATION p_animationState)
    {
        int newAnimationHash = m_animationHash[(int)p_animationState];

        if (m_currentAnimationHash == newAnimationHash) return;   // stop the same animation from interrupting itself
        m_animator.Play(newAnimationHash);                // play the animation
        m_currentAnimationHash = newAnimationHash;                // reassigning the new state
    }

    #endregion 

    PLAYER_STATE m_state = PLAYER_STATE.IDLE;

    /// MOVEMENT
    Rigidbody2D m_rb2D;
    int m_direction = 0;
    bool m_isFacingRight = true;
    [SerializeField] float m_normalMovementSpeed = 60;
    [SerializeField] float m_reducedMovementSpeed = 30;
    float m_currentSpeedX;
    bool m_canMove = true;

    /// JUMP
    [SerializeField] float m_maxHeight = 10.0f;
    [SerializeField] float m_timeToPeak1 = 1.0f;
    [SerializeField] float m_timeToPeak2 = 1.0f;
    float m_gravity1;
    float m_gravity2;
    float m_initialVelocityY;
    bool m_isGrounded;
    float m_maxFallingSpeed = 200;

    /// DASH
    bool m_hasUsedDash = false;
    Timer m_dashTimer;
    [SerializeField] float m_dashDuration = 3/6f;
    [SerializeField] float m_dashDistance = 100.0f;
    float m_dashSpeed = 200.0f;
    [SerializeField] DashDust m_dashDustScript;

    /// TODO: ORGANIZE THIS VARIABLES
    string m_objectGroundedTo;
    SpriteRenderer m_spriteRenderer;
    [SerializeField] Transicion m_transicionScript;
    bool m_isUsingGroundBreaker = false;
    bool m_isInvulnerable = false;
    Timer m_invulnerableTimer;
    float pushVelX = -50.0f;
    float pushVelY = 100.0f;
    Timer m_noControlTimer;
    Timer m_blinkTimer;
    bool m_hasBlinked = false;
    [SerializeField] float m_blinkDuration = 0.2f;
/// END OF VARIABLES
    private void Awake() {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        m_dashSpeed = m_dashDistance / m_dashDuration;
        m_dashTimer = gameObject.AddComponent<Timer>();

        m_gravity1 = -2 * m_maxHeight / (m_timeToPeak1 * m_timeToPeak1);
        m_gravity2 = -2 * m_maxHeight / (m_timeToPeak2 * m_timeToPeak2);
        m_initialVelocityY = 2 * m_maxHeight / m_timeToPeak1;

        m_isGrounded = false;

        m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
        
    }

    private void Start() {
        m_animationHash[(int)PLAYER_ANIMATION.IDLE]                 = Animator.StringToHash(m_idleAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.RUN]                  = Animator.StringToHash(m_runAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.DASH]                 = Animator.StringToHash(m_dashAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.BOOST]                = Animator.StringToHash(m_boostAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.JUMP]                 = Animator.StringToHash(m_jumpAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.FALL]                 = Animator.StringToHash(m_fallAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.LAND]                 = Animator.StringToHash(m_landAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.HIT]                  = Animator.StringToHash(m_hitAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.GROUNDBREAKER]        = Animator.StringToHash(m_groundbreakerAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.GROUNDBREAKER_LOOP]   = Animator.StringToHash(m_groundbreakerLoopAnimationName);

        m_dashTimer.Duration = m_dashDuration;
        m_currentSpeedX = m_normalMovementSpeed;
    }

    private void Update() {

        CheckIfFalling();

        switch(m_state){
            default: break;
            case PLAYER_STATE.IDLE: { HandleIdleState(); } break;
            case PLAYER_STATE.MOVE: { HandleMoveState(); } break;
            case PLAYER_STATE.BOOST: { HandleBoostState(); } break;
            case PLAYER_STATE.JUMP: { HandleJumpState(); } break;
            case PLAYER_STATE.FALL: { HandleFallState(); } break;
            case PLAYER_STATE.LAND: { HandleLandState(); } break;
            case PLAYER_STATE.DASH: { HandleDashState(); } break; 
        }
    }

    void HandleMoveState(){

        m_direction = (int)Input.GetAxisRaw("Horizontal");
        Move();
        if(InputManager.Instance.JumpButtonPressed && m_isGrounded){ Jump();}
        if(InputManager.Instance.DashButtonPressed && !m_hasUsedDash){ InitializeDash();}
    }

    void InitializeDash(){
        SoundManager.Instance.PlayOnce(AudioClipName.DASH);
        m_state = PLAYER_STATE.DASH;
        m_hasUsedDash = true;

        m_rb2D.velocity = new Vector2(FacingDirection() * m_dashSpeed, 0);
        m_rb2D.gravityScale = 0;

        ChangeAnimationState(PLAYER_ANIMATION.DASH);
        m_dashDustScript.ActivateDashDustAnimation(new Vector3(transform.position.x - 12 * FacingDirection(), transform.position.y, transform.position.z), m_isFacingRight);

        Physics2D.IgnoreLayerCollision(6, 7, true);
        m_hasUsedDash = true;
        
        m_dashTimer.Run();
    }

    void HandleDash(){
        if (m_dashTimer.IsFinished)
        {
            m_state = PLAYER_STATE.IDLE;
            m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
            m_rb2D.velocity = new Vector2(0,0);
            ChangeAnimationState(PLAYER_ANIMATION.DASH);
            Physics2D.IgnoreLayerCollision(6,7,false);
        }
    }

    void Move(){
        if(m_direction != 0 && m_canMove){
            m_rb2D.velocity = new Vector2(m_direction * m_currentSpeedX, m_rb2D.velocity.y);
            FacePlayerToMovementDirection();
            if(m_isGrounded && m_state != PLAYER_STATE.LAND && m_state != PLAYER_STATE.ATTACK){
            m_state = PLAYER_STATE.MOVE;
            ChangeAnimationState(PLAYER_ANIMATION.RUN);
            }
        }
        else{
            m_rb2D.velocity = new Vector2(0,m_rb2D.velocity.y);
            if(m_isGrounded && m_state != PLAYER_STATE.LAND){
                m_state = PLAYER_STATE.IDLE;
                ChangeAnimationState(PLAYER_ANIMATION.IDLE);
            }
        }
    }

    void Jump(){
        m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
        m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, m_initialVelocityY);

        m_isGrounded = false;
        m_state = PLAYER_STATE.BOOST;
        ChangeAnimationState(PLAYER_ANIMATION.BOOST);
    }

    void HandleIdleState(){ HandleMoveState(); }
    void HandleLandState() { HandleMoveState(); }
    

    void HandleDashState(){ HandleDash();}
    void HandleJumpState(){
        m_direction = (int)Input.GetAxisRaw("Horizontal");
        Move();
        if(InputManager.Instance.DashButtonPressed && !m_hasUsedDash){ InitializeDash();}
    }

    void HandleFallState(){  HandleJumpState(); }
    void HandleBoostState(){ HandleJumpState(); }

    void CheckIfFalling(){
        if (m_rb2D.velocity.y < 0)
        {
            m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
            m_state = PLAYER_STATE.FALL;
            ChangeAnimationState(PLAYER_ANIMATION.FALL);
            // limit falling speed
            if (m_rb2D.velocity.y < -m_maxFallingSpeed) { m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, -m_maxFallingSpeed); }
        }
    }

    void FacePlayerToMovementDirection(){
        if (!m_isFacingRight && m_direction > 0)    { FlipX(); }
        if (m_isFacingRight && m_direction < 0)     { FlipX(); }
    }

    void FlipX(){
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;       
    }

    public int FacingDirection(){ 
        if(m_isFacingRight){ return 1;} 
        else return -1;
    }

    public void PushAway(float velX, float velY) { pushVelX = velX; pushVelY = velY; }

    public void SetToGrounded( string p_objectGroundedTo){
        m_isGrounded = true;
        m_objectGroundedTo = p_objectGroundedTo;
        m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
        m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, 0);

        m_hasUsedDash = false;
    }

    public void ResetPlayer()
    {
        m_state = PLAYER_STATE.IDLE;
        ChangeAnimationState((int)PLAYER_ANIMATION.IDLE);
        m_isGrounded = false;
        m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
        m_rb2D.velocity = Vector2.zero;
        SkillManager.Instance.ResetSkillStates();
    }

/// COLLITIONS
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "spike")
        {
            GameManager.Instance.ModifyPlayerHealth(-1);
            m_transicionScript.LocalCheckpointTransition();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemyProjectile" && !m_isUsingGroundBreaker && !m_isInvulnerable && m_state != PLAYER_STATE.DASH)
        {
            PushAway(-50.0f, 100.0f);
            float velocityX = pushVelX * (collision.gameObject.transform.position - transform.position).normalized.x / Mathf.Abs((collision.gameObject.transform.position - transform.position).normalized.x);
            float velocityY = pushVelY;

            m_rb2D.velocity = new Vector2(velocityX, velocityY);
            m_isInvulnerable = true;
            m_invulnerableTimer.Run();
            m_noControlTimer.Duration = 0.2f;
            m_noControlTimer.Run();
            m_canMove = false;
            Destroy(collision.gameObject);
            m_state = PLAYER_STATE.JUMP;
            m_isGrounded = false;
            GameManager.Instance.ModifyPlayerHealth(-1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy" && !m_isInvulnerable)
        {
            float velocityX = pushVelX * (collision.gameObject.transform.position - transform.position).normalized.x / Mathf.Abs((collision.gameObject.transform.position - transform.position).normalized.x);
            float velocityY = pushVelY;
            m_rb2D.velocity = new Vector2(velocityX, velocityY);

            Physics2D.IgnoreLayerCollision(6, 7, true);
            m_isInvulnerable = true;
            m_invulnerableTimer.Run();
            m_noControlTimer.Duration = 0.5f;
            m_noControlTimer.Run();
            m_canMove = false;
            m_state = PLAYER_STATE.JUMP;
            m_isGrounded = false;
            GameManager.Instance.ModifyPlayerHealth(-1);
        }
    }



    #region Accessors

    public bool IsFacingRight { get { return m_isFacingRight; } }


    public PLAYER_STATE State
    {
        get { return m_state; }
        set
        {
            m_state = value;
            if (m_state == PLAYER_STATE.ATTACK) { m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y); }
        }
    }


    public float Gravity1 { get { return m_gravity1; } }

    public float Gravity2 { get { return m_gravity2; } }
    public bool IsGrounded
    {
        set { m_isGrounded = value; }
        get { return m_isGrounded; }
    }

    public bool IsUsingGroundBreaker { set { m_isUsingGroundBreaker = value; } }

    public string ObjectGroundedTo { set { m_objectGroundedTo = value; } }

    public Vector3 Speed { get { return m_rb2D.velocity; } }

    public void ReduceSpeed()
    {
        if (IsGrounded) { m_currentSpeedX = 0; }
        else { m_currentSpeedX = m_reducedMovementSpeed; }
    }

    public void SetToNormalSpeed() { m_currentSpeedX = m_normalMovementSpeed; }

    public bool HasUsedDash { set { m_hasUsedDash = value; } }

    #endregion

}
