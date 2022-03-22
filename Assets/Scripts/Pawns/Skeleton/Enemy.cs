using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Player m_playerScript;
    [SerializeField] protected float m_health = 3;
    [SerializeField] protected int m_damage = 1;
    protected Vector2 m_spawnPos;

    /// PLAYER COLLITION
    [SerializeField] float m_playerInvulnerableDuration = 0.5f;
    [SerializeField] float m_playerNoControlDuration = 0.5f;
    [SerializeField] Vector2 m_pushAwayPlayerVelocity = new Vector2(50.0f, 100.0f);
    protected virtual void Awake()
    {
        m_spawnPos = transform.position;
        
    }

    protected virtual void Start() {
        m_playerScript = Player.Instance;
    }

    public virtual void Damage(float p_damage)
    {
        m_health -= p_damage;
        if (m_health <= 0) { SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_KILL); }
        else { SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_HIT); }
    }
    private void OnCollisionStay2D(Collision2D p_collider)
    {
        if (p_collider.gameObject.tag == "Player" && !m_playerScript.IsInvulnerable)
        {
            float distanceToEnemyX = p_collider.gameObject.transform.position.x- transform.position.x;
            float distanceToEnemyY = p_collider.gameObject.transform.position.y - transform.position.y;
            Vector2 direction = new Vector2(distanceToEnemyX/Mathf.Abs(distanceToEnemyX), distanceToEnemyY/Mathf.Abs(distanceToEnemyY));

            m_playerScript.HandleHostileCollition(m_pushAwayPlayerVelocity, direction, m_playerNoControlDuration, m_playerInvulnerableDuration, m_damage);
        }
    }

    public void Reset() { transform.position = m_spawnPos; }
}