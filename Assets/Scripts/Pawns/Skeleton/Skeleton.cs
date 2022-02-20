using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKELETON_STATE { MOVE, CHASE, DIE, ATTACK }
public enum SKELETON_ANIMATION { MOVE, RELOAD, FIRE, DIE, LAST_NO_USE }

public class Skeleton : MonoBehaviour
{
    void ChangeAnimationState(int p_newState)
    {
        if (m_skeletonState == p_newState) { return; }  // stop the same animation from interrupting itself
        m_animator.Play(p_newState);                    // play the animation
        m_skeletonState = p_newState;                   // reassigning the new state
    }
    SKELETON_STATE m_state;
    Rigidbody2D m_rb2D;
    Animator m_animator;
    int m_skeletonState;

    string m_moveAnimationName      = "Move";
    string m_reloadAnimationName    = "Reload";
    string m_fireAnimationName    = "Fire";
    string m_dieAnimationName       = "Die";
    int[] m_animationHash = new int[(int)SKELETON_ANIMATION.LAST_NO_USE];

    public GameObject Bone;
    Timer m_fireTimer;
    float m_boneCooldown = 1.0f;
    float m_currentTime = 0.0f;

    [SerializeField] GameObject player;
    float m_playerPosX;
    float PlayerSkeletonDist = 0;

    [SerializeField] Transform left_limit;
    [SerializeField] Transform right_limit;
    private float m_spawnPosX;
    private bool m_hasReturned;

    [SerializeField] float m_speed = 50.0f;
    float m_direction;
    bool m_isFacingRight;
    bool m_isGrounded;
    bool m_playerIsNear;

    private void Awake() {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_skeletonState = Animator.StringToHash("state");

        m_state = SKELETON_STATE.MOVE;
        m_direction = -1;
        m_isGrounded = true;
        m_playerIsNear = false;

        m_spawnPosX = transform.position.x;
        m_hasReturned = true;
    }

    void Start()
    {
        m_animationHash[(int)SKELETON_ANIMATION.MOVE] = Animator.StringToHash(m_moveAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.RELOAD] = Animator.StringToHash(m_reloadAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.FIRE] = Animator.StringToHash(m_fireAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.DIE] = Animator.StringToHash(m_dieAnimationName);

        player = GameObject.FindGameObjectWithTag("Player");
        m_fireTimer = gameObject.AddComponent<Timer>();
        m_isFacingRight = false;
    }

    void Update()
    {
        PlayerSkeletonDist = Mathf.Abs(player.transform.position.x - transform.position.x);

        switch (m_state)
        {
            default:break;
            case SKELETON_STATE.MOVE:
                { Move(SKELETON_STATE.MOVE); }
                break;
            case SKELETON_STATE.CHASE:
                { Chase(SKELETON_STATE.MOVE); }
                break;
            case SKELETON_STATE.ATTACK:
                { Attack(SKELETON_STATE.MOVE); }
                break;
            case SKELETON_STATE.DIE:
                { Die(); }
                break;
        }
    }

    void Move(SKELETON_STATE p_defaultState)
    {
        if (m_hasReturned == true)
        {
            if (m_isFacingRight) { m_direction = 1; }
            else { m_direction = -1; }

            m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);

            if (m_isGrounded == false) { FlipX(); m_isGrounded = true; }

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
                m_state = SKELETON_STATE.CHASE;
            }
        }
        else
        {
            if (m_isFacingRight) { m_direction = 1; }
            else { m_direction = -1; }

            if (transform.position.x >= m_spawnPosX + 3.0f || transform.position.x <= m_spawnPosX - 3.0f)
            {
                m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);
            }
            else { m_hasReturned = true; }

            if (m_playerIsNear)
            {
                m_hasReturned = false;
                m_state = SKELETON_STATE.CHASE;
            }
        }
    }
    void Chase(SKELETON_STATE p_defaultState)
    {
        if (PlayerSkeletonDist < 100)
        {
            m_state = SKELETON_STATE.ATTACK;
        }

        if (m_isFacingRight) { m_direction = 1; }
        else { m_direction = -1; }

        m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);

        if (m_isGrounded == false || PlayerSkeletonDist > 200) 
        { 
            FlipX();  m_state = p_defaultState; 
        }
    }
    void Attack(SKELETON_STATE p_defaultState)
    {
        ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.RELOAD]);
        m_rb2D.velocity = Vector2.zero;
        if (PlayerSkeletonDist < 100)
        {   
            ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.FIRE]);
            GameObject fire = Instantiate(Bone, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            Destroy(fire, 3);

            m_currentTime = 0;
        }
        else
        {
            m_state = p_defaultState;
            ChangeAnimationState(m_animationHash[(int)SKELETON_ANIMATION.MOVE]);
        }
    }
    void Die()
    {

    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }

    public bool IsGrounded
    {
        set { m_isGrounded = value; }
    }
    public bool IsPlayerNear
    {
        set { m_playerIsNear = value; }
    }
    private void FixedUpdate()
    {
        m_currentTime = Time.fixedDeltaTime;
    }
}
