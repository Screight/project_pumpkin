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

    bool m_isFireballUnlocked = false;
    bool m_isPilarUnlocked = false;
    bool m_isGroundbreakerUnlocked = false;

    Player m_playerScript;

    private void Awake()
    {
        if (m_instance == null) { m_instance = this; }
        else { Destroy(this.gameObject); }

        m_skillPilar = GetComponent<Skill_Pilar>();
        m_skillFireball = GetComponent<Skill_Fireball>();
        m_groundbreaker = GetComponent<Skill_Groundbreaker>();

        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start() {
        //UnlockFireball();
        UnlockPilar();
        UnlockGroundbreaker();
    }

    private void Update()
    {
        switch (m_playerScript.State)
        {
            case PLAYER_STATE.MOVE: 
                {
                    m_skillFireball.Fireball(m_isFireballUnlocked);
                    m_skillPilar.Pilar(m_isPilarUnlocked);
                } break;
            case PLAYER_STATE.IDLE: 
                {
                    m_skillFireball.Fireball(m_isFireballUnlocked);
                    m_skillPilar.Pilar(m_isPilarUnlocked);
                    m_groundbreaker.Groundbreaker(m_isGroundbreakerUnlocked);
                } break;
            case PLAYER_STATE.JUMP: 
                {
                    m_skillFireball.Fireball(m_isFireballUnlocked);
                    m_groundbreaker.Groundbreaker(m_isGroundbreakerUnlocked);
                } break;
            case PLAYER_STATE.FALL: 
                {
                    m_skillFireball.Fireball(m_isFireballUnlocked);
                    m_groundbreaker.Groundbreaker(m_isGroundbreakerUnlocked);
                } break;
            case PLAYER_STATE.BOOST:
                {
                    m_skillFireball.Fireball(m_isFireballUnlocked);
                    m_groundbreaker.Groundbreaker(m_isGroundbreakerUnlocked);
                }
                break;
            case PLAYER_STATE.LAND:
                {
                    m_skillFireball.Fireball(m_isFireballUnlocked);
                    m_skillPilar.Pilar(m_isPilarUnlocked);
                }
                break;
            case PLAYER_STATE.CAST: { m_skillPilar.Pilar(m_isPilarUnlocked); } break;
            case PLAYER_STATE.GROUNDBREAKER:
                {
                    m_groundbreaker.Groundbreaker(m_isGroundbreakerUnlocked);
                }
                break;
        }
    }

    public void UnlockFireball() { 
        m_isFireballUnlocked = true;
        m_spellsUI.ActivateFireballUI();
    }
    public void UnlockGroundbreaker()
    {
        m_isGroundbreakerUnlocked = true;
        m_spellsUI.ActivateGroundbreakerUI();
    }
    public void UnlockPilar()
    {
        m_isPilarUnlocked = true;
        m_spellsUI.ActivatePilarUI();
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

    public bool IsFireballUnlocked { get { return m_isFireballUnlocked; } }
    public bool IsGroundbreakerUnlocked { get { return m_isGroundbreakerUnlocked; } }
    public bool IsPilarUnlocked { get { return m_isPilarUnlocked; } }

}