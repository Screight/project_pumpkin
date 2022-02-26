using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundbreaker : MonoBehaviour
{
    float m_maxSpeed = -300;
    Player m_player;
    Rigidbody2D m_rb2D;

    [SerializeField] LayerMask m_enemyLayer;

    bool m_isUsingGroundBreaker = false;

    private void Awake()
    {
        m_player = GetComponent<Player>();
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (InputManager.Instance.Skill2ButtonPressed && !m_player.IsGrounded)
        {
            m_rb2D.velocity = new Vector2(0, m_maxSpeed);
            m_player.SetPlayerState(PLAYER_STATE.GROUNDBREAKER);
            m_rb2D.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(6, 7, true);
            m_player.IsUsingGroundBreaker = true;
            m_isUsingGroundBreaker = true;
        }

        if (m_player.IsGrounded && m_isUsingGroundBreaker)
        {
            Collider2D[] enemiesInAttackRange = Physics2D.OverlapCircleAll(transform.position, 16, m_enemyLayer);
            foreach (Collider2D enemy in enemiesInAttackRange)
            {
                if (enemy.gameObject.tag == "enemy")
                {
                    enemy.GetComponent<Skeleton>().State = ENEMY_STATE.AIR;
                    enemy.gameObject.GetComponent<Enemy>().Damage(1);
                    float velocityX;
                    if(transform.position.x == enemy.transform.position.x) { velocityX = 0; }
                    else { velocityX = -50 * (transform.position - enemy.transform.position).normalized.x / Mathf.Abs((transform.position - enemy.transform.position).normalized.x);};
                    float velocityY = 100;
                    enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX, velocityY);
                }
                Physics2D.IgnoreLayerCollision(6, 7, false);
                m_isUsingGroundBreaker = false;
                m_player.IsUsingGroundBreaker = false;
            }
        }

    }

}
                