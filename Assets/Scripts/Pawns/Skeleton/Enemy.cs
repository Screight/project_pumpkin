using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int skeletonHealth;
    protected SKELETON_STATE m_state;

    public void Damage(int p_damage)
    {
        skeletonHealth -= p_damage;
        if (skeletonHealth <= 0) { m_state = SKELETON_STATE.DIE; }
    }
}
