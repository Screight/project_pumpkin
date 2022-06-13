using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNewController : MonoBehaviour
{

    #region Components
    BoxCollider2D m_boxCollider;
    Rigidbody2D m_rb2d;
    Animator m_animator;
    #endregion

    #region State Variables
    [SerializeField] PlayerData m_playerData;
    PlayerStateMachine m_stateMachine;
    PlayerIdleState m_idleState;
    PlayerMoveState m_moveState;
    PlayerDashState m_dashState;
    PlayerJumpState m_jumpState;
    PlayerAirState m_airState;
    PlayerGroundebreakerState m_groundbreakerState;
    PlayerFireballState m_fireballState;
    #endregion

    #region Check Transforms
    [SerializeField] Transform m_groundCheck;
    [Tooltip("Starting position of fireball")]
    [SerializeField] Transform m_firaballSpawn;
    #endregion

    #region Other Variables
    Vector2 m_workSpace;
    Vector2 m_currentSpeed;
    int m_facingDirection;
    #endregion

    #region Unity Callback Functions
    private void Awake() {
        m_boxCollider = GetComponent<BoxCollider2D>();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        m_stateMachine = new PlayerStateMachine();
        m_idleState = new PlayerIdleState(this, m_stateMachine, m_playerData, "idle");
        m_moveState = new PlayerMoveState(this, m_stateMachine, m_playerData, "move");
        m_dashState = new PlayerDashState(this, m_stateMachine, m_playerData, "dash");
        m_jumpState = new PlayerJumpState(this, m_stateMachine, m_playerData, "jump");
        m_airState = new PlayerAirState(this, m_stateMachine, m_playerData, "air");
        m_groundbreakerState = new PlayerGroundebreakerState(this, m_stateMachine, m_playerData, "groundbreaker");
        m_fireballState = new PlayerFireballState(this, m_stateMachine, m_playerData, "idle");

        m_playerData.Initialize();
    }
    private void Start() {
        m_stateMachine.Initialize(m_idleState);
        m_facingDirection = 1;
    }

    private void Update() {
        m_currentSpeed = m_rb2d.velocity;
        m_stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        m_stateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion
    
    #region Set Functions
    public void SetVelocityX(float p_velocity){
        m_workSpace.x = p_velocity;
        m_workSpace.y = m_currentSpeed.y;
        m_rb2d.velocity = m_workSpace;
        m_currentSpeed = m_workSpace;
    }

    public void SetVelocityY(float p_velocity){
        m_workSpace.y = p_velocity;
        m_workSpace.x = m_currentSpeed.x;
        m_rb2d.velocity = m_workSpace;
        m_currentSpeed = m_workSpace;
    }

    public void SetGravity(float p_gravity){
        m_rb2d.gravityScale = p_gravity / Physics2D.gravity.y;
    }

    #endregion

    #region Check Functions

    public void CheckIfShouldFlip(float p_xInput){
        if(p_xInput > 0 && m_facingDirection < 0 || p_xInput < 0 && m_facingDirection > 0){
            FlipX();
        }
    }

    public bool CheckIfGrounded(){
        //return Physics2D.OverlapBox (m_groundCheck.position, m_playerData.m_groundBox, m_playerData.m_groundMask);
        return Physics2D.OverlapCircle(m_groundCheck.position, 0.3f, m_playerData.m_groundMask);
    }

    #endregion

    #region Other Functions
    void FlipX(){
        m_facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void InstantiateFireball(){
        Fireball script = Instantiate(m_playerData.m_fireball, m_firaballSpawn.position, Quaternion.identity).GetComponent<Fireball>();
        script.Fire(m_facingDirection);
    }

    #endregion

    #region Getters and Setters
    public Animator Animator{
        get { return m_animator; }
    }
    public PlayerIdleState IdleState{
        get { return m_idleState; }
    }

    public PlayerMoveState MoveState{
        get { return m_moveState; }
    }

    public PlayerDashState DashState{
        get { return m_dashState; }
    }

    public PlayerJumpState JumpState{
        get { return m_jumpState; }
    }

    public PlayerAirState AirState{
        get { return m_airState; }
    }

    public PlayerGroundebreakerState GroundbreakerState{
        get { return m_groundbreakerState; }
    }

    public PlayerFireballState FireballState{
        get { return m_fireballState; }
    }

    public Vector2 CurrentVelocity{
        get { return m_currentSpeed; }
    }

    public int FacingDirection{
        get { return m_facingDirection; }
        set { m_facingDirection = value; }
    }
    #endregion

}
