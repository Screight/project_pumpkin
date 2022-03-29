using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float m_speed = 40;
    [SerializeField] float m_maxAliveTime = 5;
    [SerializeField] int m_damage = 1;
    // PLAYER COLLISION
    [SerializeField] float m_playerInvulnerableDuration = 0.5f;
    [SerializeField] float m_playerNoControlDuration = 0.5f;
    [SerializeField] Vector2 m_pushAwayPlayerVelocity = new Vector2(50.0f, 100.0f);
    [SerializeField] AudioClipName m_audioClip;
    private Rigidbody2D m_rb2D;

    private void Awake() { m_rb2D = GetComponent<Rigidbody2D>(); }

    public void Shoot(int p_direction)
    {
        m_rb2D.velocity = new Vector2(p_direction * m_speed, m_rb2D.velocity.y);
        SoundManager.Instance.PlayOnce(m_audioClip);
        Destroy(gameObject,m_maxAliveTime);
    }

    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (p_collider.gameObject.layer == (int)UNITY_LAYERS.OBSTACLE) { Destroy(gameObject); }
        else if (p_collider.tag == "Player")
        {
            float distanceToEnemyX = p_collider.gameObject.transform.position.x - transform.position.x;
            float distanceToEnemyY = p_collider.gameObject.transform.position.y - transform.position.y;
            Vector2 direction = new Vector2(distanceToEnemyX / Mathf.Abs(distanceToEnemyX), distanceToEnemyY / Mathf.Abs(distanceToEnemyY));

            Player.Instance.HandleHostileCollision(m_pushAwayPlayerVelocity, direction, m_playerNoControlDuration, m_playerInvulnerableDuration, m_damage);
            Destroy(this.gameObject);
        }
    }
}