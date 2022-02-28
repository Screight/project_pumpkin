using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }

    static public GameManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    public void ModifyHealthUI(bool p_isGainingHP)
    {
        if (p_isGainingHP) { m_healthUI.GainHealth(); }
        else { m_healthUI.LoseHearth(); }
    }



}
