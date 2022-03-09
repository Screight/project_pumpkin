using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_STATE { MOVE, CHASE, DIE, ATTACK, HIT, AIR }
public enum ENEMY_ANIMATION { MOVE, RELOAD, FIRE, DIE, HIT, LAST_NO_USE }

public class Enemy : MonoBehaviour
{
    protected int m_health;
    protected ENEMY_STATE m_state;

    Vector3 m_initialPosition;
    ENEMY_STATE m_initialState;

    protected virtual void Awake()
    {
        m_initialPosition = transform.localPosition;
        m_initialState = ENEMY_STATE.MOVE;
        Debug.Log("Base Awake called");
    }

    public virtual void Damage(int p_damage)
    {
        m_health -= p_damage;
        
        if (m_health <= 0) { 
            m_state = ENEMY_STATE.DIE;
            SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_KILL);
        } else
        {
            SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_HIT);
        }
    }

    public void Reset()
    {
        transform.localPosition = m_initialPosition;
        m_state = m_initialState;
    }
}