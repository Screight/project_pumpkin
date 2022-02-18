using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKELETON_STATE { MOVE, CHASE, DIE, ATTACK }
public enum SKELETON_ANIMATION { MOVE, RELOAD, ATTACK, DIE, LAST_NO_USE }

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
    string m_attackAnimationName    = "Attack";
    string m_dieAnimationName       = "Die";
    int[] m_animationHash = new int[(int)SKELETON_ANIMATION.LAST_NO_USE];

    const float m_boneCooldown = 2000.0f;
    private float m_actualBoneCooldown;
    public GameObject Bone;
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

    private void Awake() {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_skeletonState = Animator.StringToHash("state");

        m_state = SKELETON_STATE.MOVE;
        m_direction = -1;
        m_isGrounded = true;
        m_actualBoneCooldown = 0.0f;

        m_spawnPosX = transform.position.x;
        m_hasReturned = true;
    }

    void Start()
    {
        m_animationHash[(int)SKELETON_ANIMATION.MOVE] = Animator.StringToHash(m_moveAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.RELOAD] = Animator.StringToHash(m_reloadAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.ATTACK] = Animator.StringToHash(m_attackAnimationName);
        m_animationHash[(int)SKELETON_ANIMATION.DIE] = Animator.StringToHash(m_dieAnimationName);

        player = GameObject.FindGameObjectWithTag("Player");
        m_isFacingRight = false;
    }

    void Update()
    {
        PlayerSkeletonDist = Mathf.Abs(player.transform.position.x - transform.position.x);
        Debug.Log(m_state);

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
                { Attack(SKELETON_STATE.CHASE); }
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
            if (PlayerSkeletonDist < 200)
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
        }
    }
    void Chase(SKELETON_STATE p_defaultState)
    {
        m_hasReturned = false;
        if (m_isFacingRight) { m_direction = 1; }
        else { m_direction = -1; }

        m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);

        if (m_isGrounded == false || PlayerSkeletonDist > 200) 
        { FlipX();  m_state = p_defaultState; }
    }
    void Attack(SKELETON_STATE p_defaultState)
    {
        if (!m_isGrounded) { m_state = p_defaultState; }

        if (m_actualBoneCooldown >= m_boneCooldown)
        {
            m_actualBoneCooldown = 0;
            GameObject fire = Instantiate(Bone,new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            Destroy(fire, 3);
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
}
