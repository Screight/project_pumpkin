using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKELETON_STATE { MOVE, CHASE, DIE, ATTACK, HIT, AIR }
public enum SKELETON_ANIMATION { MOVE, RELOAD, FIRE, DIE, HIT, ATTACK, LAST_NO_USE }

public class Skeleton : Enemy
{   
    [SerializeField] GameObject m_arrow;
    Rigidbody2D m_rb2D;
    Timer boneTimer;
    SKELETON_STATE m_skeletonState;
    int m_currentState;

    string m_moveAnimationName      = "Move";
    string m_reloadAnimationName    = "Reload";
    string m_fireAnimationName      = "Fire";
    string m_hitAnimationName       = "hit";
    string m_attackAnimationName       = "attack";
    string m_dieAnimationName       = "Die";
    int[] m_animationHash = new int[(int)SKELETON_ANIMATION.LAST_NO_USE];

    [SerializeField] GameObject prefabBone;
    GameObject Bone;
    BoneScript m_boneScript;
    private float boneCooldown = 2000.0f;

    [SerializeField] GameObject player;
    float m_playerPosX;

    [SerializeField] Transform left_limit;
    [SerializeField] Transform right_limit;
    private bool m_hasReturned;

    [SerializeField] float m_speed = 50.0f;
    [SerializeField] bool m_isFacingRight;
    bool m_isGrounded;
    bool m_isHittingWall = false;
    bool m_playerIsNear;
    bool m_playerIsAtRange;
    bool m_isAttacking = false;
    [SerializeField] Transform m_attackPosition;

    protected override void Awake() 
    {
        base.Awake();

        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_health = 3;

        m_isGrounded = true;
        m_playerIsNear = false;
        m_playerIsAtRange = false;

        m_hasReturned = true;

        Bone = Instantiate(prefabBone, new Vector3(0, 0, 0), Quaternion.identity);

        Physics2D.IgnoreLayerCollision(7, 7, true);
    }

    protected override void Start()
    {
        base.Start();
        m_animationHash[(int)SKELETON_ANIMATION.MOVE] = Animator.StringToHash(m_moveAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.RELOAD] = Animator.StringToHash(m_reloadAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.FIRE] = Animator.StringToHash(m_fireAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.DIE] = Animator.StringToHash(m_dieAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.HIT] = Animator.StringToHash(m_hitAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.ATTACK] = Animator.StringToHash(m_attackAnimationName);

        player = GameObject.FindGameObjectWithTag("Player");
        m_isFacingRight = false;

        m_boneScript = Bone.GetComponent<BoneScript>();
        boneTimer = gameObject.AddComponent<Timer>();
        Bone.SetActive(false);
        boneTimer.Duration = 2;
    }

    protected override void Update()
    {
        base.Update();
        switch (m_skeletonState)
        {
            default:break;
            case SKELETON_STATE.MOVE:       { Move(SKELETON_STATE.MOVE); }      break;
            case SKELETON_STATE.CHASE:      { Chase(SKELETON_STATE.MOVE); }     break;
            case SKELETON_STATE.ATTACK:     { Attack(SKELETON_STATE.CHASE); }   break;
            case SKELETON_STATE.HIT:        { }                                 break;
            case SKELETON_STATE.DIE:        { }                          break;            
        }

        float delta = Time.fixedDeltaTime * 1000;
        boneCooldown += 1.0f * delta;
    }

    void Move(SKELETON_STATE p_defaultState)
    {
        ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.MOVE]);
        if (m_hasReturned)
        {
            if(m_isHittingWall){
                FlipX();
                m_isHittingWall = false;
            }
            m_rb2D.velocity = new Vector2(FacingDirection() * m_speed, m_rb2D.velocity.y);

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
            if (m_playerIsNear)
            {
                m_hasReturned = false;
                m_skeletonState = SKELETON_STATE.CHASE;
            }
        }
        else
        {
            if(m_isHittingWall){
                FlipX();
                m_isHittingWall = false;
            }
            if (m_isFacingRight && transform.position.x > m_spawnPos.x)         { FlipX(); }
            else if(!m_isFacingRight && transform.position.x < m_spawnPos.x)    { FlipX(); }

            //BackToSpawn
            if (transform.position.x < m_spawnPos.x - 4.0f || transform.position.x > m_spawnPos.x + 4.0f)
            {
                m_rb2D.velocity = new Vector2(FacingDirection() * m_speed, m_rb2D.velocity.y);
            }
            else { m_hasReturned = true; }

            if (m_playerIsNear)
            {
                m_hasReturned = false;
                m_skeletonState = SKELETON_STATE.CHASE;
            }
        }
    }
    void Chase(SKELETON_STATE p_defaultState)
    {
        //Player Near but Unnaccesible
        if (m_playerIsNear && !m_playerIsAtRange && !m_isGrounded) 
        { 
            m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y);
            ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.RELOAD]);
        }
        else if (m_playerIsNear)
        {
            //Ready to Attack
            if (m_playerIsAtRange) { m_skeletonState = SKELETON_STATE.ATTACK; }
            //Chasing
            if (player.transform.position.x > transform.position.x && !m_isFacingRight) { FlipX(); }
            if (player.transform.position.x < transform.position.x && m_isFacingRight)  { FlipX(); }
            m_rb2D.velocity = new Vector2(FacingDirection() * (m_speed + 6), m_rb2D.velocity.y);
        }
        else { m_skeletonState = p_defaultState; }
    }
    void Attack(SKELETON_STATE p_defaultState)
    {
        if (player.transform.position.x > transform.position.x && !m_isFacingRight) { FlipX(); }
        if (player.transform.position.x < transform.position.x && m_isFacingRight)  { FlipX(); }

        if(!m_isAttacking){
            if (m_playerIsAtRange && m_isGrounded)
            {        
                m_rb2D.velocity = Vector2.zero;
                ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.ATTACK]);
                m_isAttacking = true;
            }
            else {
            m_skeletonState = p_defaultState;
            ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.MOVE]);
            }
        }
        
    }

    void LaunchFireBall(){
        Projectile m_boneArrowScript  = Instantiate(m_arrow, m_attackPosition.position, Quaternion.identity).GetComponent<Projectile>();
        m_boneArrowScript.Shoot(FacingDirection());
        m_isAttacking = false;
    }

    void ChangeAnimationState(int p_newState)
    {
        if (m_currentState == p_newState && m_currentState != m_animationHash[(int)SKELETON_ANIMATION.HIT]) { return; }
        
        if (m_currentState == p_newState && m_currentState == m_animationHash[(int)SKELETON_ANIMATION.HIT]) {
            m_animator.Play(p_newState,-1,0);
        }
        else
        {
            m_animator.Play(p_newState);
            m_currentState = p_newState;
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
        m_skeletonState = SKELETON_STATE.HIT;
        ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.HIT]);
        base.Damage(p_damage);
        m_rb2D.velocity = new Vector2(0, m_rb2D.velocity.y);
        if(m_health <= 0) { m_skeletonState = SKELETON_STATE.DIE;
            ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.DIE]);
            Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), true);
        }
    }

    public SKELETON_STATE State { set { m_skeletonState = value; } get { return m_skeletonState; } }

    protected override void EndHit()
    {
        base.EndHit();
        m_skeletonState = SKELETON_STATE.MOVE;
        ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.MOVE]);
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), false);
    }

    public override void Reset(){
        base.Reset();
        m_rb2D.gravityScale = 40;
        EndHit();
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), false);
    }

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

    public bool IsHittingWall {
        set { m_isHittingWall = value;}
    }

    #endregion

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(new Vector3(left_limit.position.x, left_limit.position.y , transform.position.z), 3);
        Gizmos.DrawSphere(new Vector3(right_limit.position.x, left_limit.position.y , transform.position.z), 3);
    }

}