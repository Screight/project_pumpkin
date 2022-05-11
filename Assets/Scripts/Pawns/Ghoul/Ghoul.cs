using UnityEngine;

public enum GHOUL_STATE { IDLE, CHASE, ATTACK, HIT, DIE}

public class Ghoul : Enemy
{
    Rigidbody2D m_rb2D;
    GHOUL_STATE m_ghoulState;
    ENEMY_STATE m_state;

    GameObject player;
    float m_playerPosX;
    bool hasCharged;
    Timer chargeTimer;

    [SerializeField] float m_speed = 80.0f;
    bool m_isFacingRight;
    bool m_isGrounded;
    bool m_isHittingWall = false;
    bool m_playerIsNear;
    bool m_playerIsAtRange;

    [SerializeField] Transform m_leftPatrolPosition;
    [SerializeField] Transform m_rightPatrolPosition;
    Timer m_eventTimer;
    [SerializeField] float m_chargeDuration = 0.5f;
    [SerializeField] float m_chargeSpeed;
    [SerializeField] float m_restDuration = 1.0f;
    [SerializeField] float m_chargeDistance = 50.0f;
    float m_attackDuration;

    private void OnEnable() {
        if(AnimationManager.Instance != null)
        {
            InitializePatrol();
        }
        
    }

    protected override void Awake()
    {
        base.Awake();

        m_rb2D = GetComponent<Rigidbody2D>();
        m_health = 5;

        m_isGrounded = true;
        m_playerIsNear = false;
        m_playerIsAtRange = false;
        hasCharged = false;

        Physics2D.IgnoreLayerCollision(7, 7, true);
        m_attackDuration = m_chargeDistance / m_chargeSpeed;
    }

    protected override void Start()
    {
        base.Start();

        m_isFacingRight = false;
        chargeTimer = gameObject.AddComponent<Timer>();
        chargeTimer.Duration = 1;
        m_eventTimer = gameObject.AddComponent<Timer>();
        player = Player.Instance.gameObject;
        InitializePatrol();
        m_dieAnimationDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.GHOUL_DIE);
        m_hitAnimationDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.GHOUL_HIT);

    }

    protected override void Update()
    {
        base.Update();
        if (Time.timeScale == 0) { return; } //Game Paused

        switch (m_state)
        {
            default: { break; }
            case ENEMY_STATE.PATROL:
                { HandlePatrol(); }
                break;
            case ENEMY_STATE.CHASE:
                { HandleChase(); }
                break;
            case ENEMY_STATE.CHARGE:
                { HandleCharge(); }
                break;
            case ENEMY_STATE.ATTACK:
                { HandleAttack(); }
                break;
            case ENEMY_STATE.REST:
                { HandleRest(); }
                break;
        }
    }

    void InitializePatrol()
    {
        m_state = ENEMY_STATE.PATROL;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_MOVE, false);
        float direction = (m_leftPatrolPosition.position.x - transform.position.x) / Mathf.Abs((m_leftPatrolPosition.position.x - transform.position.x));
        m_rb2D.velocity = new Vector2(direction * m_speed, m_rb2D.velocity.y);
        FaceToDirection(direction);
    }

    void HandlePatrol()
    {
        if (m_playerIsNear && !Player.Instance.IsInvulnerable)
        {
            InitializeChase();
            return;
        }
        Patrol();
    }

    void InitializeChase()
    {
        m_state = ENEMY_STATE.CHASE;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_MOVE, false);
    }

    void Patrol()
    {
        if (transform.position.x > m_rightPatrolPosition.position.x)
        {
            m_rb2D.velocity = new Vector2(-m_speed, 0);
            FaceToDirection(-1);
        }
        else if (transform.position.x < m_leftPatrolPosition.position.x)
        {
            m_rb2D.velocity = new Vector2(m_speed, 0);
            FaceToDirection(1);
        }
    }

    void HandleChase(){
        if(!m_playerIsNear){
            InitializePatrol();
        }
        else if(m_playerIsAtRange){
            if(!m_isHittingWall){
                InitializeCharge();
            }
            else {InitializeRest();}
        }
        else{
            Chase();
        }
    }

    void Chase(){
        float direction = (player.transform.position.x - transform.position.x) / Mathf.Abs(player.transform.position.x - transform.position.x);
        m_rb2D.velocity = new Vector2(direction * m_speed, m_rb2D.velocity.y);
        FaceToDirection(direction);
    }
    void InitializeCharge(){
        FaceToDirection(player.transform.position.x - transform.position.x);
        m_state = ENEMY_STATE.CHARGE;
        m_rb2D.velocity = Vector2.zero;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_CHARGE, false);
        m_eventTimer.Duration = m_chargeDuration;
        m_eventTimer.Run();

    }

    void HandleCharge(){
        if(m_eventTimer.IsFinished){
            InitializeAttack();
        }
    }

    void InitializeAttack(){
        m_state = ENEMY_STATE.ATTACK;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_ATTACK, false);
        float direction = (player.transform.position.x - transform.position.x) / Mathf.Abs(player.transform.position.x - transform.position.x);
        FaceToDirection(direction);
        m_rb2D.velocity = new Vector2(direction * m_chargeSpeed, 0);
        m_eventTimer.Duration = m_attackDuration;
        m_eventTimer.Run();
    }

    void HandleAttack()
    {
        if (m_eventTimer.IsFinished) { InitializeRest(); }
    }

    void InitializeRest()
    {
        FaceToDirection(player.transform.position.x - transform.position.x);
        m_state = ENEMY_STATE.REST;
        m_rb2D.velocity = Vector2.zero;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_IDLE, false);
        m_eventTimer.Duration = m_restDuration;
        m_eventTimer.Stop();
        m_eventTimer.Run();
    }

    void HandleRest()
    {
        if (m_eventTimer.IsFinished)
        {
            if (m_playerIsAtRange)
            {
                if (m_playerIsNear && !m_isHittingWall)
                {
                    InitializeCharge();
                }
                if (!m_isHittingWall)
                {
                    InitializeChase();
                }
                else { InitializeRest(); }
            }
            else { InitializePatrol(); }
        }
    }

    void Idle()
    {
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_IDLE, false);
        m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y);

        if (m_playerIsNear)
        {
            if (m_isGrounded) { m_ghoulState = GHOUL_STATE.CHASE; }
            else
            {
                if (player.transform.position.x > transform.position.x && !m_isFacingRight) { FlipX(); }
                if (player.transform.position.x < transform.position.x && m_isFacingRight) { FlipX(); }
            }
        }
    }
    void Move(GHOUL_STATE p_defaultState)
    {
        if(!m_isGrounded) { return;}
        if (player.transform.position.x > transform.position.x && !m_isFacingRight) { FlipX(); }
        if (player.transform.position.x < transform.position.x && m_isFacingRight) { FlipX(); }
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_MOVE, false);
        //Player Near but Unnaccesible
        if ((m_playerIsNear || m_playerIsAtRange) && !m_isGrounded) { m_ghoulState = p_defaultState; }

        if (m_playerIsNear) // Is Near
        {           
            if (m_playerIsAtRange)  //Is At Range
            {
                if (m_isGrounded) // Is Grounded
                {
                    m_playerPosX = player.transform.position.x;
                    chargeTimer.Run();
                    m_ghoulState = GHOUL_STATE.ATTACK;
                    AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_IDLE, false);
                }
            }           
            else {m_rb2D.velocity = new Vector2(FacingDirection() * m_speed, m_rb2D.velocity.y); } //Nar but not at Range = Chase        
        }
        else { m_ghoulState = p_defaultState; } // Not Near
    }
    void Attack(GHOUL_STATE p_defaultState)
    {
        if(!m_isGrounded) { return ;}
        float distance = Mathf.Abs(player.transform.position.x - transform.position.x);

        m_rb2D.velocity = Vector2.zero;
        if (chargeTimer.IsFinished) { hasCharged = true; }

        if ((m_isFacingRight && transform.position.x <= m_playerPosX + 15) || (!m_isFacingRight && transform.position.x >= m_playerPosX - 15) || m_isHittingWall)
        {
            if (m_isHittingWall)
            {
                m_ghoulState = p_defaultState;
                chargeTimer.Stop();
                hasCharged = false;
            }
            else if (hasCharged && m_isGrounded)
            {

                AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_ATTACK, false);
                m_rb2D.velocity = new Vector2(FacingDirection() * m_speed * 2, m_rb2D.velocity.y);
                if (!m_isGrounded) { m_ghoulState = p_defaultState; chargeTimer.Stop(); hasCharged = false; }
            }
            else if (!m_isGrounded)
            {
                m_ghoulState = p_defaultState;
                chargeTimer.Stop();
                hasCharged = false;
            }
        }
        else { m_ghoulState = p_defaultState; chargeTimer.Stop(); hasCharged = false; }
    }

    protected override void EndHit()
    {
        base.EndHit();
        if (m_state == ENEMY_STATE.DEATH) { return; }
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_IDLE, false);
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), false);
        m_rb2D.gravityScale = 40;
        if (m_playerIsAtRange)
        {
            if (m_playerIsNear && !m_isHittingWall) { InitializeCharge(); }
            if (!m_isHittingWall)
            {
                InitializeChase();
            }
            else { InitializeRest(); }
        }
        else { InitializePatrol(); }
    }

    void FaceToDirection(float p_direction)
    {
        if (p_direction >= 0 && !m_isFacingRight)
        {
            FlipX();
        }
        else if (p_direction < 0 && m_isFacingRight)
        {
            FlipX();
        }
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

    public override void Damage(float p_damage)
    {
        //m_ghoulState = GHOUL_STATE.HIT;
        m_state = ENEMY_STATE.HIT;
        m_rb2D.velocity = Vector2.zero;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_HIT, true);
        base.Damage(p_damage);
        if (m_health <= 0)
        {
            //m_ghoulState = GHOUL_STATE.DIE;
            m_state = ENEMY_STATE.DEATH;
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.GHOUL_DIE, false);
            Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), true);
        }
    }

    public override void Reset()
    {
        base.Reset();
        m_rb2D.gravityScale = 40;
        //m_ghoulState = GHOUL_STATE.IDLE;
        InitializePatrol();
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), false);
    }

    public ENEMY_STATE State 
    { 
        set { m_state = value; } 
        get { return m_state; } 
    }

    protected override bool OnCollisionStay2D(Collision2D p_collider)
    {
        if (base.OnCollisionStay2D(p_collider)) { return false; }

        if (m_state == ENEMY_STATE.ATTACK)
        {
            m_eventTimer.Stop();
            InitializeRest();
        }
        return true;
    }

    #region Accessors
    public bool IsGrounded { set { m_isGrounded = value; } }
    public bool IsHittingWall { set { m_isHittingWall = value; } }
    public bool IsPlayerNear { set { m_playerIsNear = value; } }
    public bool IsPlayerAtRange { set { m_playerIsAtRange = value; } }
    public bool IsFacingRight { get { return m_isFacingRight; } }
    #endregion
}