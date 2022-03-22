using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImproved : MonoBehaviour
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

    void ChangeAnimationState(PLAYER_ANIMATION p_animationState)
    {
        int newAnimationHash = m_animationHash[(int)m_animationState];

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
    [SerializeField] float m_speedX = 60;
    bool m_canMove = true;

    /// JUMP
    [SerializeField] float m_maxHeight = 10.0f;
    [SerializeField] float m_timeToPeak1 = 1.0f;
    [SerializeField] float m_timeToPeak2 = 1.0f;
    float m_gravity1;
    float m_gravity2;
    float m_initialVelocityY;
    bool m_isGrounded;
    float m_maxFallingSpeed = -200;

    /// DASH
    bool m_hasUsedDash;

    Timer m_dashTimer;
    [SerializeField] float m_dashDuration = 3/6f;
    [SerializeField] float m_dashDistance = 100.0f;
    
    float m_dashSpeed = 200.0f;
    [SerializeField] DashDust m_dashDustScript;


    private void Awake() {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        m_dashSpeed = m_dashDistance / m_dashDuration;
        m_dashTimer = gameObject.AddComponent<Timer>();
        
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
        if(InputManager.Instance.DashButtonPressed){ InitializeDash();}
    }

    void InitializeDash(){
        SoundManager.Instance.PlayOnce(AudioClipName.DASH);

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
            ChangeAnimationState(PLAYER_ANIMATION.DASH);
            Physics2D.IgnoreLayerCollision(6,7,false);
        }
    }

    void Move(){
        if(m_direction != 0 && m_canMove){
            m_rb2D.velocity = new Vector2(m_direction * m_speedX, m_rb2D.velocity.y);
            FacePlayerToMovementDirection();
            if(m_isGrounded && m_state != PLAYER_STATE.LAND && m_state != PLAYER_STATE.ATTACK){
            m_state = PLAYER_STATE.MOVE;
            ChangeAnimationState(PLAYER_ANIMATION.RUN);
            }
        }
        else{
            m_rb2D.velocity = new Vector2(0,m_rb2D.velocity.y);
            if(m_isGrounded){
                m_state = PLAYER_STATE.IDLE;
                ChangeAnimationState(PLAYER_ANIMATION.IDLE);
            }
        }
    }

    void Jump(){
        m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
        m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, m_initialVelocityY);

        m_state = PLAYER_STATE.BOOST;
        ChangeAnimationState(PLAYER_ANIMATION.BOOST);
    }

    void HandleIdleState(){ HandleMoveState(); }
    void HandleLandState() { HandleMoveState(); }
    

    void HandleDashState(){ HandleDash();}
    void HandleJumpState(){
        m_direction = (int)Input.GetAxisRaw("Horizontal");
        Move();
        if(InputManager.Instance.DashButtonPressed){ InitializeDash();}
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

    int FacingDirection(){ 
        if(m_isFacingRight){ return 1;} 
        else return -1;
    }

    #region Accessors

    public bool IsFacingRight { get { return m_isFacingRight; } }

    public int GetFacingDirection() {
        if(m_isFacingRight){ return 1;}
        else return -1;
    }

    public void SetPlayerAnimation(PLAYER_ANIMATION p_animation)
    {
        ChangeAnimationState(m_animationHash[(int)p_animation]);
    }
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
        if (IsGrounded) { m_speedX = 0; }
        else { m_speedX = m_reducedMovementSpeed; }
    }

    public void SetToNormalSpeed() { m_speedX = 60; }

    public bool HasUsedDash { set { m_hasUsedDash = value; } }

    #endregion

}
