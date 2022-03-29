using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Player stats
    [SerializeField] int PLAYER_MAX_HEALTH;
    int m_playerHealth;
    float m_playerAttackDamage = 1;

    static GameManager m_instance;
    [SerializeField] HealthPoints m_healthUI;
    Player m_player;

    private void Awake()
    {
        if(m_instance == null) { 
            m_instance = this; 
            Initiate();
        }
        else { Destroy(this.gameObject); }
    }

    void Initiate()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_playerHealth = PLAYER_MAX_HEALTH;
    }

    static public GameManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    public void ModifyPlayerHealth(int p_amount)
    {
        m_playerHealth += p_amount;
        if (m_playerHealth < 0) { m_playerHealth = 0; }
        else if (m_playerHealth > PLAYER_MAX_HEALTH) { m_playerHealth = PLAYER_MAX_HEALTH; }

        m_healthUI.SetHealth(m_playerHealth);
    }

    public float PlayerAttackDamage { get { return m_playerAttackDamage; } }

}
