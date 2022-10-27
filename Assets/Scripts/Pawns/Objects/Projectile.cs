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
    [SerializeField] AudioClip m_shootSound;
    AudioSource m_source;
    private Rigidbody2D m_rb2D;
    [SerializeField] AudioClip m_impactSound;

    SpriteRenderer m_renderer;
    [SerializeField] ParticleSystem m_impactParticle;
    Collider2D m_collision;

    private void Awake() 
    {
        m_rb2D = GetComponent<Rigidbody2D>();
        m_source = GetComponent<AudioSource>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_collision = GetComponent<Collider2D>();
    }

    public void Shoot(int p_direction)
    {
        m_rb2D.velocity = new Vector2(p_direction * m_speed, m_rb2D.velocity.y);
        if(m_shootSound == null){
            SoundManager.Instance.PlayOnce(AudioClipName.SKELLY_SHOOT);
        }
        else{
            m_source.PlayOneShot(m_shootSound);
        }
        
        Destroy(gameObject,m_maxAliveTime);
    }

    public void Shoot(Vector2 p_direction){
        m_rb2D.velocity = p_direction * m_speed;
    }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if (p_collider.gameObject.layer == (int)UNITY_LAYERS.OBSTACLE)
        {
            m_source.PlayOneShot(m_impactSound);
            m_renderer.enabled = false;

            if(m_impactParticle != null)
            {
                m_impactParticle.Play();
            }
            
            m_collision.enabled = false;
            m_rb2D.simulated = false;
            Destroy(gameObject, 5.0f);
        }
        else if (p_collider.gameObject.CompareTag("Player") && Player.Instance.CanPlayerGetHit())
        {
            float distanceToEnemyX = p_collider.gameObject.transform.position.x - transform.position.x;
            float distanceToEnemyY = p_collider.gameObject.transform.position.y - transform.position.y;
            Vector2 direction = new Vector2(distanceToEnemyX / Mathf.Abs(distanceToEnemyX), distanceToEnemyY / Mathf.Abs(distanceToEnemyY));

            Player.Instance.HandleHostileCollision(m_pushAwayPlayerVelocity, direction, m_playerNoControlDuration, m_playerInvulnerableDuration, m_damage);
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collision2D p_collision) {
        if (p_collision.gameObject.layer == (int)UNITY_LAYERS.OBSTACLE)
        {
            m_source.PlayOneShot(m_impactSound);
            m_renderer.enabled = false;
            m_impactParticle.Play();
            m_collision.enabled = false;
            m_rb2D.simulated = false;
            Destroy(gameObject, 5.0f);
        }
        else if (p_collision.gameObject.CompareTag("Player") && Player.Instance.CanPlayerGetHit())
        {
            float distanceToEnemyX = p_collision.gameObject.transform.position.x - transform.position.x;
            float distanceToEnemyY = p_collision.gameObject.transform.position.y - transform.position.y;
            Vector2 direction = new Vector2(distanceToEnemyX / Mathf.Abs(distanceToEnemyX), distanceToEnemyY / Mathf.Abs(distanceToEnemyY));

            Player.Instance.HandleHostileCollision(m_pushAwayPlayerVelocity, direction, m_playerNoControlDuration, m_playerInvulnerableDuration, m_damage);
            Destroy(gameObject);
        }
    }

}