using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKELETON_STATE { MOVE, CHASE, DIE, ATTACK }

public class Skeleton : MonoBehaviour {

    SKELETON_STATE m_state;
    Rigidbody2D m_rb2D;
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
    }

    void Start() {
        m_isFacingRight = false;
    }

    void Update() {
        switch (m_state) {
            case SKELETON_STATE.MOVE:
                {
                    Move();
                }
                break;
            case SKELETON_STATE.CHASE:
                {
                    //Chase();
                }
                break;
            case SKELETON_STATE.ATTACK:
                {
                    //Attack();
                }
                break;

            case SKELETON_STATE.DIE:
                {
                    //Die();
                }
                break;
        }
    }

    void Move() {

        if (m_isFacingRight) {
            m_direction = 1;
        }
        else { m_direction = -1; }
        m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);

        if (m_isGrounded == false) {
            FlipX();
            m_isGrounded = true;
        }
        if (transform.position.x < left_limit.position.x) {
            transform.position = new Vector3(left_limit.position.x, transform.position.y, transform.position.z);
            FlipX();
        }
        if (transform.position.x > right_limit.position.x) {
            transform.position = new Vector3(right_limit.position.x, transform.position.y, transform.position.z);
            FlipX();
        }
    }

    void FlipX() {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }

    public bool IsGrounded {
        set {
            m_isGrounded = value;
        }
    }
}
