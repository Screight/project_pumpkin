using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BAT_STATE { MOVE, CHASE, ATTACK, DIE, HIT, AIR }
public enum BAT_ANIMATION { MOVE, ATTACK, DIE, HIT, LAST_NO_USE }

public class Bat : Enemy
{
    Rigidbody2D m_rb2D;
    BAT_STATE m_batState;
    int m_currentState;

    [SerializeField] Transform m_leftLimit;
    [SerializeField] Transform m_rightLimit;
    [SerializeField] float m_patrolSpeed;
    [SerializeField] float m_chargeSpeed;
    [SerializeField] float m_attackRange;
    Vector2 m_direction;
    float m_sense;
    GameObject m_player;


    private void Awake()
    {
        base.Awake();
        m_rb2D = GetComponent<Rigidbody2D>();
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        // direction of the straight line that unites the two patrols points
        m_direction = new Vector2(m_rightLimit.position.x - m_leftLimit.position.x, m_rightLimit.position.y - m_leftLimit.position.y).normalized;
        m_sense = -1;

        // if the pawn is out of bonds in the x direction put him in the middle point of the straight line
        if(transform.position.x < m_leftLimit.position.x || transform.position.x > m_rightLimit.position.x)
        {
            transform.position = new Vector3((m_leftLimit.position.x + m_rightLimit.position.x)/2, transform.position.y, transform.position.z);
        }

        CorrectYPositionToStraightLine();

        // initiate the pawn movement
        m_rb2D.velocity = m_sense * m_direction * m_patrolSpeed;
    }

    void Update()
    {
        switch (m_batState)
        {
            default: break;
            case BAT_STATE.MOVE:
                {
                    Patrol();
                }
                break;
            case BAT_STATE.CHASE:
                {
                    Search();
                }
                break;
        }
    }

    void Patrol()
    {
        if(transform.position.x < m_leftLimit.position.x)
        {
            m_sense *= -1;
            transform.position = new Vector3(m_leftLimit.position.x, m_leftLimit.transform.position.y);
            m_rb2D.velocity = m_sense * m_direction * m_patrolSpeed;
        }
        else if(transform.position.x > m_rightLimit.position.x)
        {
            m_sense *= -1;
            transform.position = new Vector3(m_rightLimit.position.x, m_rightLimit.transform.position.y);
            m_rb2D.velocity = m_sense * m_direction * m_patrolSpeed;
        }
    }

    void Search()
    {
        Vector3 vectorToPlayer = new Vector2(m_player.transform.position.x - transform.position.x, m_player.transform.position.y - transform.position.y);
        if(vectorToPlayer.magnitude >= m_attackRange)
        {
            m_rb2D.velocity = m_patrolSpeed * vectorToPlayer.normalized;
        }
        else
        {
            //m_batState = ENEMY_STATE.ATTACK;
            m_rb2D.velocity = Vector2.zero;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(m_batState == BAT_STATE.MOVE)
        {
            m_sense *= -1;
            m_rb2D.velocity = m_sense * m_direction * m_patrolSpeed;
            CorrectYPositionToStraightLine();
        }
        
    }

    void CorrectYPositionToStraightLine()
    {
        // if the pawn is out of bounds in the y direction put him in the correct point of the straight line
        if (transform.position.y < m_leftLimit.position.y || transform.position.y > m_rightLimit.position.y)
        {
            float m_tangentDirection = m_direction.y / m_direction.x;
            transform.position = new Vector3(transform.position.x, m_leftLimit.position.y + m_tangentDirection * (transform.position.x - m_leftLimit.position.x), transform.position.z);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player") { m_batState = BAT_STATE.CHASE; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(m_leftLimit.transform.position, 5 * Vector3.one);
        Gizmos.DrawWireCube(m_rightLimit.transform.position, 5 * Vector3.one);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, 4);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 200);
    }
}