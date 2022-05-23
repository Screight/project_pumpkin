using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField] SPIDER_BOSS_DAMAGEABLE_PARTS m_part;
    private SpiderBoss m_spiderBoss;
    [SerializeField] Vector2 m_pushAwayVelocity = new Vector2(50.0f,0);
    [SerializeField] float m_noControlDuration = 0.4f;
    [SerializeField] float m_invulnerableDuration = 0.2f;
    [SerializeField] int m_damage;
    private Color m_damageColor = new Color(0.6415094f, 0.2929156f, 0.3313433f);
    [SerializeField] ParticleSystem[] sparkles;

    Collider2D m_collider;
    private bool m_canDamagePlayer = false;
    private bool m_canBeDamaged = false;
    private SpriteRenderer m_sprite;
    private float m_damageDuration = 0.2f;
    private Timer m_event;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        m_spiderBoss = FindObjectOfType<SpiderBoss>();
        m_sprite = GetComponent<SpriteRenderer>();
        m_event = gameObject.AddComponent<Timer>();
        m_event.Duration = m_damageDuration;
    }

    private void Update()
    {
        if (!m_event.IsFinished)
        {
           m_sprite.color = Color.Lerp(Color.red, Color.white, m_event.CurrentTime);
            
        }
        else { m_sprite.color = Color.white; }
    }

    private void OnTriggerStay2D(Collider2D p_collider)
    {
        if (!p_collider.CompareTag("Player") || !m_canDamagePlayer) { return; }
        m_canDamagePlayer = false;
        Vector2 direction = new Vector2(1, 1);
        if (Player.Instance.transform.position.x < transform.position.x)
        {
            direction.x = -1;
        }
        else { direction.x = 1; }
        Player.Instance.HandleHostileCollision(m_pushAwayVelocity, direction, m_noControlDuration, m_invulnerableDuration, m_damage);
        if (m_part != SPIDER_BOSS_DAMAGEABLE_PARTS.HEAD)
        {
            m_spiderBoss.HasHitPlayer = true;
        }
    }

    public void DestroyDrill()
    {
        m_canBeDamaged = false;
        m_canDamagePlayer = false;
        if (m_part != SPIDER_BOSS_DAMAGEABLE_PARTS.HEAD)
        {
            m_sprite.color = m_damageColor;
            for (int i = 0; i < sparkles.Length; i++) { sparkles[i].Play(); }
        }
    }

    public void Damage(float p_damage)
    {
        if (!m_canBeDamaged) { return; }
        m_spiderBoss.Damage(p_damage, m_part);
        Debug.Log(m_part + "DAMAGED");
        if(m_part == SPIDER_BOSS_DAMAGEABLE_PARTS.HEAD)
        {
            SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_HIT);
        }
        m_sprite.material.color = Color.red;
        m_event.Restart();
    }

    public bool CanDamagePlayer { set { m_canDamagePlayer = value; } }
    public bool CanBeDamaged { set { m_canBeDamaged = value; } }

    public void InitializeExplosion()
    {
        gameObject.SetActive(false);
        Debug.Log("INITIALIZING EXPLOSION");
    }

    public void Reset()
    {
        m_canBeDamaged = false;
        m_canDamagePlayer = false;
        m_sprite.color = new Color(255, 255, 255);
    }
}