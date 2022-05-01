using UnityEngine;

public class Player : AnimatedCharacter
{
    [SerializeField] Attack m_attackScript;

    static Player m_instance;
    public static Player Instance
    {
        get { return m_instance; }
        private set { }
    }

    bool m_isBeingScripted = false;

    PLAYER_STATE m_state = PLAYER_STATE.IDLE;

    /// MOVEMENT
    Rigidbody2D m_rb2D;
    float m_direction = 0;
    Collider2D m_collider;
    bool m_isFacingRight = true;
    [SerializeField] float m_normalMovementSpeed = 60;
    [SerializeField] float m_reducedMovementSpeed = 30;
    float m_currentSpeedX;
    bool m_canPerformAction = true;

    /// JUMP
    [SerializeField] float m_maxHeight = 10.0f;
    [SerializeField] float m_timeToPeak1 = 1.0f;
    [SerializeField] float m_timeToPeak2 = 1.0f;
    float m_gravity1;
    float m_gravity2;
    float m_initialVelocityY;
    bool m_isGrounded;
    float m_maxFallingSpeed = 200;
    [SerializeField] float m_moveTowardsOneWayPlatform = 40;

    /// DASH
    bool m_hasUsedDash = false;
    Timer m_dashTimer;
    [SerializeField] float m_dashDuration = 3/6f;
    [SerializeField] float m_dashDistance = 100.0f;
    float m_dashSpeed = 200.0f;
    [SerializeField] DashDust m_dashDustScript;

    /// DAMAGE CONTROL AND REACTION

    bool m_isInvulnerable = false;
    Timer m_invulnerableTimer;
    Timer m_noControlTimer;

    Timer m_blinkTimer;
    bool m_hasBlinked = false;
    [SerializeField] Vector2 m_pushAwayOnProjectileHitVelocity = new Vector2(-50.0f, 100.0f);

    /// TODO: ORGANIZE THIS VARIABLES
    string m_objectGroundedTo;
    SpriteRenderer m_spriteRenderer;
    [SerializeField] Transicion m_transicionScrDamageipt;
    bool m_isUsingGroundBreaker = false;
    bool m_isInsideActiveInteractiveZone = false;

    Timer m_eventTimer;
    float m_deathDuration;
    float m_hurtDuration;
    bool m_eventStart = false;

    /// END OF VARIABLES
    protected override void Awake() {

        base.Awake();

        if (m_instance == null) { m_instance = this; }
        else { Destroy(gameObject); }

        m_rb2D = GetComponent<Rigidbody2D>();

        m_dashSpeed = m_dashDistance / m_dashDuration;
        m_dashTimer = gameObject.AddComponent<Timer>();

        m_invulnerableTimer = gameObject.AddComponent<Timer>();
        m_noControlTimer = gameObject.AddComponent<Timer>();
        m_blinkTimer = gameObject.AddComponent<Timer>();

        m_gravity1 = -2 * m_maxHeight / (m_timeToPeak1 * m_timeToPeak1);
        m_gravity2 = -2 * m_maxHeight / (m_timeToPeak2 * m_timeToPeak2);
        m_initialVelocityY = 2 * m_maxHeight / m_timeToPeak1;

        m_isGrounded = false;

        m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_collider = GetComponent<Collider2D>();
        m_eventTimer = gameObject.AddComponent<Timer>();
    }

    private void Start()
    {
        m_dashTimer.Duration = m_dashDuration;
        m_currentSpeedX = m_normalMovementSpeed;
        m_deathDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.PLAYER_DEATH);
        m_hurtDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.PLAYER_HIT);
    }

    private void Update()
    {
        if(GameManager.Instance.IsGamePaused){ return ;}
        CheckIfFalling();

        if(m_isBeingScripted){
            if(m_eventTimer.IsFinished && m_eventStart){
                m_isBeingScripted = false;
                InitializeIdleState();
                m_eventStart = false;
            }
            return;
        }

        if (m_invulnerableTimer.IsRunning)
        {
            if (m_blinkTimer.IsFinished)
            {
                if (!m_hasBlinked) { m_spriteRenderer.color = new Color(1, 1, 1, 0.5f); }
                else { m_spriteRenderer.color = new Color(1, 1, 1, 1); }
                m_hasBlinked = !m_hasBlinked;
                m_blinkTimer.Run();
            }
        }

        if (m_invulnerableTimer.IsFinished && m_isInvulnerable && m_state != PLAYER_STATE.DASH)
        {
            if (m_state != PLAYER_STATE.DEATH) { m_state = PLAYER_STATE.IDLE; }

            m_direction = 0;
            m_isInvulnerable = false;
            m_spriteRenderer.color = new Color(255, 255, 255, 255);
            Physics2D.IgnoreLayerCollision(6, 7, false);
            m_invulnerableTimer.Stop();
            m_blinkTimer.Stop();
            m_hasBlinked = false;
        }

        if (m_noControlTimer.IsFinished) { m_canPerformAction = true; }

        switch (m_state)
        {
            default: break;
            case PLAYER_STATE.IDLE:     { HandleIdleState(); } break;
            case PLAYER_STATE.MOVE:     { HandleMoveState(); } break;
            case PLAYER_STATE.BOOST:    { HandleBoostState(); } break;
            case PLAYER_STATE.JUMP:     { HandleJumpState(); } break;
            case PLAYER_STATE.FALL:     { HandleFallState(); } break;
            case PLAYER_STATE.LAND:     { HandleLandState(); } break;
            case PLAYER_STATE.DASH:     { HandleDashState(); } break;
            case PLAYER_STATE.ATTACK:   { m_attackScript.HandleAttack(m_isGrounded); } break;
            case PLAYER_STATE.HURT: { HandleHurtState(); } break;
            case PLAYER_STATE.DEATH: { HandleDeathState(); } break;
        }
    }

    void HandleMoveState()
    {
        m_direction = InputManager.Instance.HorizontalAxisFlat;
        Move();
        if (InputManager.Instance.JumpButtonPressed && m_isGrounded && !m_isInsideActiveInteractiveZone)
        {
            Jump();
        }
        else if (InputManager.Instance.DashButtonPressed && !m_hasUsedDash && GameManager.Instance.GetIsSkillAvailable(SKILLS.DASH)) { InitializeDash(); }
        m_attackScript.HandleAttack(m_isGrounded);
    }

    void InitializeDash()
    {
        SoundManager.Instance.PlayOnce(AudioClipName.DASH);
        m_state = PLAYER_STATE.DASH;
        m_hasUsedDash = true;

        m_rb2D.velocity = new Vector2(FacingDirection() * m_dashSpeed, 0);
        m_rb2D.gravityScale = 0;

        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_DASH, false);
        m_dashDustScript.ActivateDashDustAnimation(new Vector3(transform.position.x - 12 * FacingDirection(), transform.position.y, transform.position.z), m_isFacingRight);

        Physics2D.IgnoreLayerCollision(6, 7, true);
        m_dashTimer.Run();
    }

    void HandleDash()
    {
        if (m_dashTimer.IsFinished)
        {
            m_state = PLAYER_STATE.IDLE;
            m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
            m_rb2D.velocity = new Vector2(0, 0);
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_IDLE, false);
            Physics2D.IgnoreLayerCollision(6, 7, false);
        }
    }

    void Move()
    {
        if (!m_canPerformAction) { return; }
        if (m_direction != 0)
        {
            
            m_rb2D.velocity = new Vector2((int)m_direction * m_currentSpeedX, m_rb2D.velocity.y);
            FacePlayerToMovementDirection();
            if (m_isGrounded && m_state != PLAYER_STATE.LAND && m_state != PLAYER_STATE.ATTACK)
            {
                m_state = PLAYER_STATE.MOVE;
                AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_RUN, false);
            }
        }
        else
        {
            m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y);
            if (m_isGrounded && m_state != PLAYER_STATE.LAND)
            {
                m_state = PLAYER_STATE.IDLE;
                AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_IDLE, false);
            }
        }
    }

    void Jump()
    {
        m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
        m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, m_initialVelocityY);

        m_isGrounded = false;
        m_state = PLAYER_STATE.BOOST;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_BOOST, false);
    }

    void InitializeIdleState(){
        m_state = PLAYER_STATE.IDLE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_IDLE, false);
    }
    void HandleIdleState() { HandleMoveState(); }
    void HandleLandState() { HandleMoveState(); }
    void HandleDashState() { HandleDash(); }
    void InitializeJumpState(){
        m_state = PLAYER_STATE.JUMP;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_JUMP, false);
    }
    void HandleJumpState()
    {
        m_direction = (int)Input.GetAxisRaw("Horizontal");
        Move();
        m_attackScript.HandleAttack(m_isGrounded);
        if (InputManager.Instance.DashButtonPressed && !m_hasUsedDash && GameManager.Instance.GetIsSkillAvailable(SKILLS.DASH)) { InitializeDash(); }
    }
    void HandleFallState() { HandleJumpState(); }
    void HandleBoostState() { HandleJumpState(); }

    void InitializeHurtState(){
        m_state = PLAYER_STATE.HURT;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_HIT, true);
        m_eventTimer.Duration = m_hurtDuration;
        m_eventTimer.Restart();
    }

    void HandleHurtState(){
        if(m_eventTimer.IsFinished){
            if(m_isGrounded){ InitializeIdleState();}
            else { InitializeJumpState();}
        }
    }

    void CheckIfFalling()
    {
        if (m_state == PLAYER_STATE.DASH) { return; }
        if (m_state == PLAYER_STATE.DEATH) { return; }
        if (m_state == PLAYER_STATE.GROUNDBREAKER) { return; }
        if (m_rb2D.velocity.y < 0)
        {
            m_isGrounded = false;
            m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
            if (m_state != PLAYER_STATE.ATTACK) { m_state = PLAYER_STATE.FALL; }
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_FALL, false);
            // limit falling speed
            if (m_rb2D.velocity.y < -m_maxFallingSpeed) { m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, -m_maxFallingSpeed); }
        }
    }

    void FacePlayerToMovementDirection()
    {
        if (!m_isFacingRight && m_direction > 0)    { FlipX(); }
        if (m_isFacingRight && m_direction < 0)     { FlipX(); }
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }

    public int FacingDirection()
    {
        if (m_isFacingRight) { return 1; }
        else { return -1; }
    }

    public void HandleOneWayPlatforms()
    {
        m_isGrounded = false;
        m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, m_moveTowardsOneWayPlatform);
        m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
    }

    public void SetToGrounded(string p_objectGroundedTo)
    {
        m_isGrounded = true;
        m_objectGroundedTo = p_objectGroundedTo;
        m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
        m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, 0);

        m_hasUsedDash = false;
    }

    public void ResetPlayer(PLAYER_STATE p_state, ANIMATION p_animationState)
    {
        m_state = p_state;
        AnimationManager.Instance.PlayAnimation(this, p_animationState, false);
        m_isGrounded = false;
        m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
        m_rb2D.velocity = Vector2.zero;
        SkillManager.Instance.ResetSkillStates();
    }

    public void StopPlayerMovement() { m_rb2D.velocity = Vector2.zero; }

    public void HandleBoostToJumpTransition()
    {
        if (m_state == PLAYER_STATE.BOOST) { m_state = PLAYER_STATE.JUMP; }
    }
    public void ScriptWalk(int p_direction, float m_walkDuration)
    {
        if (p_direction > 0) { m_direction = 1; }
        else { m_direction = -1; }

        m_isBeingScripted = true;
        FacePlayerToMovementDirection();
        m_state = PLAYER_STATE.MOVE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_RUN, false);
        m_rb2D.velocity = new Vector2(p_direction * m_normalMovementSpeed, 0);
        m_eventTimer.Duration = m_walkDuration;
        m_eventTimer.Restart();
        m_eventStart = true;
    }

    public void ScriptTopSuction(float p_suctionVelocity)
    {
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_JUMP, false);
        m_rb2D.gravityScale = 0;
        m_rb2D.velocity = new Vector2(0, p_suctionVelocity);
    }

    public void ScriptTopImpulse(Vector2 p_impulseVelocity)
    {
        m_rb2D.velocity = p_impulseVelocity;
        m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_JUMP, false);
        
        if (p_impulseVelocity.x > 0) { m_direction = 1; }
        else { m_direction = -1; }
        FacePlayerToMovementDirection();
    }

    public void ScriptFall()
    {
        m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_FALL, false);
        m_state = PLAYER_STATE.FALL;
    }

    public void SetPlayerToScripted()
    {
        ResetPlayer(PLAYER_STATE.IDLE, ANIMATION.PLAYER_IDLE);
        m_isBeingScripted = true;
        Physics2D.IgnoreLayerCollision(6, 7, true);
    }
    public void FacePlayerToRight()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        m_isFacingRight = true;
    }

    public void FacePlayerToLeft()
    {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        m_isFacingRight = false;
    }

    public void StopScripting()
    {
        m_isBeingScripted = false;
        ResetPlayer(m_state, m_animationState);
        m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y);
    }

    public void SetPlayerToPosition(Vector3 p_position) { transform.position = p_position; }

    /// COLLISIONS
    private void OnCollisionStay2D(Collision2D p_collider)
    {
        if (p_collider.gameObject.CompareTag("spike"))
        {
            HandleHostileCollision(new Vector2(0, 40), Vector2.up, 0.5f, 0.5f, 1);
            CheckpointsManager.Instance.MovePlayerToLocalCheckPoint();
            if (GameManager.Instance.PlayerHealth <= 0)
            {
                Instance.ResetPlayer(PLAYER_STATE.IDLE, ANIMATION.PLAYER_IDLE);
            }
        }
    }

    void HandleDeathState(){
        if(m_eventTimer.IsFinished){
            Die();
            // START TRANSITION
        }
    }

    public void HandleDeath()
    {
        m_state = PLAYER_STATE.DEATH;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_DEATH, false);
        m_eventTimer.Duration = m_deathDuration;
        m_eventTimer.Run();
    }

    //!NOT IN USE!
    void Die()
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
        CheckpointsManager.Instance.MovePlayerToGlobalCheckPoint();
        GameManager.Instance.RestorePlayerToFullHealth();
        CameraManager.Instance.SetCameraToPlayerPosition();
        CameraManager.Instance.ClampCameraToTarget();
        // RESET PLAYER
        // SCREEN TRANSITION
        // TP TO LAST CHECKPOINT (add room variable to checkpoint)
        // MOVE CAMERA
        // SET CHECKPOINT ROOM TO CURRENT ROOM
    }

    public void HandleHostileCollision(Vector2 p_pushAwayVelocity, Vector2 p_direction, float p_noControlDuration, float p_invulnerableDuration, int p_damage)
    {
        m_rb2D.velocity = new Vector2(p_direction.x * p_pushAwayVelocity.x, p_direction.y * p_pushAwayVelocity.y);

        m_isInvulnerable = true;
        m_invulnerableTimer.Duration = p_invulnerableDuration;
        m_blinkTimer.Duration = p_invulnerableDuration;
        m_invulnerableTimer.Run();
        m_noControlTimer.Duration = p_noControlDuration;
        m_noControlTimer.Run();
        m_canPerformAction = false;
        Physics2D.IgnoreLayerCollision(6, 7, true);
        m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;

        InitializeHurtState();
        GameManager.Instance.ModifyPlayerHealth(-p_damage);
    }

    public bool CanPlayerGetHit()
    {
        return !m_isUsingGroundBreaker && !m_isInvulnerable && m_state != PLAYER_STATE.DASH;
    }

    #region Accessors

    public bool IsFacingRight { get { return m_isFacingRight; } }

    public PLAYER_STATE State
    {
        get { return m_state; }
        set
        {
            m_state = value;
            if (m_state == PLAYER_STATE.ATTACK) { 
                m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y); }
        }
    }
    public bool IsGrounded
    {
        set { m_isGrounded = value; }
        get { return m_isGrounded; }
    }

    public Vector2 Speed { get { return m_rb2D.velocity;}}

    public bool IsUsingGroundBreaker { set { m_isUsingGroundBreaker = value; } }

    public string ObjectGroundedTo { 
        set { m_objectGroundedTo = value; } 
        get { return m_objectGroundedTo;}
        }

    public bool IsInvulnerable { get { return m_isInvulnerable;}}

    public bool IsInsideActiveInteractiveZone {
        set { m_isInsideActiveInteractiveZone = value; }
    }


    public void ReduceSpeed()
    {
        if (IsGrounded) { m_currentSpeedX = 0; }
        else { m_currentSpeedX = m_reducedMovementSpeed; }
    }

    public void SetToNormalSpeed() { m_currentSpeedX = m_normalMovementSpeed; }

    public bool HasUsedDash { set { m_hasUsedDash = value; } }

    public bool IsBeginScripted { get { return m_isBeingScripted; }}

    public Collider2D GetCollider(){ return m_collider;}
    #endregion

    public void StartTalking()
    {
        m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y);
        m_state = PLAYER_STATE.TALKING;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.PLAYER_IDLE, false);
    }

    public void SetGravityScaleTo0(){
        m_rb2D.gravityScale = 0;
        m_rb2D.velocity = Vector2.zero;
    }

    public void SetGravityScaleToFall(){
        m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x, collider.bounds.max.y, transform.position.z), new Vector3(transform.position.x, collider.bounds.min.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(collider.bounds.min.x, collider.bounds.max.y, transform.position.z), new Vector3(collider.bounds.min.x, collider.bounds.min.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(collider.bounds.max.x, collider.bounds.max.y, transform.position.z), new Vector3(collider.bounds.max.x, collider.bounds.min.y, transform.position.z));
    }
}