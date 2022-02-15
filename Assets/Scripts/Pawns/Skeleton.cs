using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKELETON_STATE { MOVE, CHASE, DIE, ATTACK }

public class Skeleton : MonoBehaviour
{
    SKELETON_STATE m_state;
    Rigidbody2D m_rb2D;

    const float m_boneCooldown = 2000.0f;
    private float m_actualBoneCooldown;
    public GameObject Bone;

    [SerializeField] Transform left_limit;
    [SerializeField] Transform right_limit;

    [SerializeField] float m_speed = 50.0f;
    float m_direction;

    Animator m_animator;
    int m_skeletonState;
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
    }

    void Start()
    {
        m_isFacingRight = false;
    }

    void Update()
    {
        switch (m_state)
        {
            default:break;
            case SKELETON_STATE.MOVE:
                { Move(); }
                break;
            case SKELETON_STATE.CHASE:
                {/*Chase();*/}
                break;
            case SKELETON_STATE.ATTACK:
                {/*Attack();*/}
                break;
            case SKELETON_STATE.DIE:
                {/*Die();*/}
                break;
        }

        //fire
        if (m_actualBoneCooldown >= m_boneCooldown)
        {
            m_actualBoneCooldown = 0;
            GameObject fire = Instantiate(Bone, transform.position + new Vector3(3.0f, 0.0f, 0.0f), transform.rotation);
            Destroy(fire, 3);
        }
    }

    void Move()
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

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime * 1000;
        m_actualBoneCooldown += 1.0f * delta;
    }
}
