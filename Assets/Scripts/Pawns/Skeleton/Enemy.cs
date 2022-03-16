using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int m_health;
    protected Vector2 m_spawnPos;

    protected virtual void Awake()
    {
        m_spawnPos = transform.position;
    }

    public virtual void Damage(int p_damage)
    {
        m_health -= p_damage;

        if (m_health <= 0) { SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_KILL); }
        else { SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_HIT); }
    }

    public void Reset() { transform.position = m_spawnPos; }
}