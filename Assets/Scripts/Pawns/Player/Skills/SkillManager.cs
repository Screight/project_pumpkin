using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] SpellCooldown m_spellsUI;
    static SkillManager m_instance;

    Skill_Fireball m_skillFireball;
    Skill_Pilar m_skillPilar;
    Skill_Groundbreaker m_groundbreaker;

    Player m_playerScript;

    private void Awake()
    {
        if (m_instance == null) {
            m_instance = this; 
            Initialize();
        }
        else { Destroy(this.gameObject); }

        
    }

    private void Initialize(){
        m_skillPilar = GetComponent<Skill_Pilar>();
        m_skillFireball = GetComponent<Skill_Fireball>();
        m_groundbreaker = GetComponent<Skill_Groundbreaker>();

        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        switch (m_playerScript.State)
        {
            case PLAYER_STATE.MOVE: 
                {
                    m_skillFireball.Fireball();
                    //m_skillPilar.Pilar();
                } break;
            case PLAYER_STATE.IDLE: 
                {
                    m_skillFireball.Fireball();
                    //m_skillPilar.Pilar();
                    m_groundbreaker.Groundbreaker();
                } break;
            case PLAYER_STATE.JUMP: 
                {
                    m_skillFireball.Fireball();
                    m_groundbreaker.Groundbreaker();
                } break;
            case PLAYER_STATE.FALL: 
                {
                    m_skillFireball.Fireball();
                    m_groundbreaker.Groundbreaker();
                } break;
            case PLAYER_STATE.BOOST:
                {
                    m_skillFireball.Fireball();
                    m_groundbreaker.Groundbreaker();
                }
                break;
            case PLAYER_STATE.LAND:
                {
                    m_skillFireball.Fireball();
                    //m_skillPilar.Pilar();
                }
                break;
            case PLAYER_STATE.CAST: 
                {
                    //m_skillPilar.Pilar(); 
                }
                break;
            case PLAYER_STATE.GROUNDBREAKER:
                {
                    m_groundbreaker.Groundbreaker();
                }
                break;
        }
    }


    static public SkillManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    public void ResetSkillStates()
    {
        m_groundbreaker.ResetGroundbreakerState();
    }

}