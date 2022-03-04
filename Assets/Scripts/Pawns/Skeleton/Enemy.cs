using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int skeletonHealth;
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
        skeletonHealth -= p_damage;
        
        if (skeletonHealth <= 0) { 
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