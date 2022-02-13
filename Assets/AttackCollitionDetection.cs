using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollitionDetection : MonoBehaviour
{
    Player m_player;
    CircleCollider2D m_attackCollider;

    private void Awake()
    {
        m_player = GetComponentInParent<Player>();
        m_attackCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            m_attackCollider.enabled = false;
            Debug.Log("Enemy damaged");       
            
        }
    }

}
