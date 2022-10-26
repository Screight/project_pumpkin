using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : Enemy
{
    Rigidbody2D m_rb2D;

    Animator m_animator;
    bool m_isPlayerInVisionRange = false;
    bool m_isPlayerInAttackRange = false;
    bool m_isHittingWall = false;
    bool m_isGrounded = false;

    bool m_isFacingRight = false;

    StateMachine m_stateMachine;

    #region States

    StateSkeletonPatrol m_patrolState;
    StateSkeletonChase m_chaseState;
    StateSkeletonAttack m_attackState;

    #endregion

    [SerializeField] Transform m_patrolPointLeft;
    [SerializeField] Transform m_patrolPointRight;

    [SerializeField] float m_speed;
    [SerializeField] float m_attackCooldown = 2.0f;

    [SerializeField] GameObject m_projectilePrefab;
    [SerializeField] Transform m_attackPosition;

    [SerializeField] AudioSource m_flamesAudioSrc;
    [SerializeField] AudioSource m_growlingAudioSrc;
    private Timer m_growlTimer;

    protected override void Awake()
    {
        base.Awake();

        m_animator = GetComponent<Animator>();
        m_rb2D = GetComponent<Rigidbody2D>();

        m_stateMachine = new StateMachine();

        if(m_patrolPointLeft.position.x > m_patrolPointRight.position.x){
            Transform temp = m_patrolPointLeft;
            m_patrolPointLeft = m_patrolPointRight;
            m_patrolPointRight = temp;
        }
        m_patrolState = new StateSkeletonPatrol(m_stateMachine, this, "Patrol State", ANIMATION.SKELETON_MOVE, SKELETON_STATE.PATROL, m_patrolPointLeft, m_patrolPointRight);
        m_chaseState = new StateSkeletonChase(m_stateMachine, this, "Chase State", ANIMATION.SKELETON_MOVE, SKELETON_STATE.CHASE);
        m_attackState = new StateSkeletonAttack(m_stateMachine, this, "Attack State", ANIMATION.SKELETON_ATTACK, SKELETON_STATE.ATTACK, m_attackCooldown);

    }

    protected override void Start()
    {
        base.Start();
        m_stateMachine.Initialize(m_patrolState, this) ;
        m_growlTimer = gameObject.AddComponent<Timer>();
        m_growlTimer.Duration = 3.0f;
    }

    protected override void Update()
    {
        base.Update();
        if (m_isStateMachineActive)
        {
            m_stateMachine.CurrentState.LogicUpdate();
        }

        //GROWL SFX
        if (m_growlTimer.IsFinished && !m_growlingAudioSrc.isPlaying && m_health > 0)
        {
            int randNum = Random.Range(0, 2);
            if (randNum == 0) { m_growlingAudioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SKELLY_GROWL_1)); }
            else { m_growlingAudioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SKELLY_GROWL_2)); }

            m_growlTimer.Duration = Random.Range(5, 7);
            m_growlTimer.Run();
        }

    }

    public void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }
    public int FacingDirection()
    {
        if (m_isFacingRight) { return 1; }
        else { return -1; }
    }

    void LaunchFireBall()
    {
        Projectile m_boneArrowScript = Instantiate(m_projectilePrefab, m_attackPosition.position, Quaternion.identity).GetComponent<Projectile>();
        m_boneArrowScript.Shoot(FacingDirection());
        m_boneArrowScript.gameObject.transform.SetParent(gameObject.transform.parent);
    }

    void CheckPlayerPosition()
    {
        if (m_isPlayerInAttackRange) {

            Vector2 pawnPosition = transform.position;
            Vector2 targetPosition = Player.Instance.transform.position;

            float direction = (targetPosition.x - pawnPosition.x) / Mathf.Abs(targetPosition.x - pawnPosition.x);
            if (direction != FacingDirection())
            {
                FlipX();
                VelocityX *= -1;
            }
            return;
        }
        else if (m_isPlayerInVisionRange) { ChangeState(m_chaseState); }
        else { ChangeState(m_patrolState); }
    }

    public override void Damage(float p_damage)
    {
        m_isStateMachineActive = false;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SKELETON_HIT, false);
        m_growlingAudioSrc.Stop();
        base.Damage(p_damage);
        //m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y);
        if (m_health <= 0)
        {
            AnimationManager.Instance.PlayAnimation(this, ANIMATION.SKELETON_DIE, false);
            //Stop SFX
            m_growlTimer.Stop();
            m_flamesAudioSrc.Stop();
            m_growlingAudioSrc.Stop();
            m_isDying = true;
        }
    }

    public float Speed { get { return m_speed; } }

    #region Accessors
    public bool IsPlayerInVisionRange {
        get { return m_isPlayerInVisionRange; }
        set { m_isPlayerInVisionRange = value; } 
    }
    public bool IsPlayerInAttackRange {
        get { return m_isPlayerInAttackRange; }
        set { m_isPlayerInAttackRange = value; }
    }
    public bool IsHittingWall {
        get { return m_isHittingWall; }   
        set { m_isHittingWall = value; } }
    public float VelocityX {
        get { return m_rb2D.velocity.x; }
        set {
            m_rb2D.velocity = new Vector2(value, m_rb2D.velocity.y);
            Debug.Log(m_rb2D.velocity);
        }
    }
    public float VelocityY {
        get { return m_rb2D.velocity.y; }
        set { m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, value); }
    }
    public bool IsGrounded
    {
        get { return m_isGrounded; }
        set { m_isGrounded = value; }
    }

    public Rigidbody2D Rigidbody2D { get { return m_rb2D; } }
    public SKELETON_STATE State { get { return m_stateMachine.CurrentState.StateEnum; } }
    
    public void ChangeState(State p_state)
    {
        m_stateMachine.ChangeState(p_state);
    }
    public State PatrolState { get { return m_patrolState; } }
    public State ChaseState { get { return m_chaseState; } }
    public State AttackState { get { return m_attackState; } }

    #endregion
}
