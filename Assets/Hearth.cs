using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : MonoBehaviour
{
    Player m_player;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && m_player.HealthPoints < m_player.TotalHealthPoints)
        {
            m_player.ModifyHP(1);
            GameManager.Instance.ModifyHealthUI(true);
            this.gameObject.SetActive(false);
        }
    }

}
