using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float m_health;
    protected Vector2 m_spawnPos;

    protected virtual void Awake()
    {
        m_health = 3;
        m_spawnPos = transform.position;
    }

    public virtual void Damage(float p_damage)
    {
        m_health -= p_damage;
        Debug.Log(m_health);
        if (m_health <= 0) { SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_KILL); }
        else { SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_HIT); }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Player.Instance.PushAway(-50.0f, 100.0f);
        }
    }

    public void Reset() { transform.position = m_spawnPos; }
}