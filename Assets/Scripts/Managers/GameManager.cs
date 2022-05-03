using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject m_panel;
    // Player stats
    [SerializeField] int PLAYER_MAX_HEALTH;
    SpellCooldown m_spellCooldown;
    int m_playerHealth;
    float m_playerAttackDamage = 1;

    static GameManager m_instance;
    [SerializeField] HealthPoints m_healthUI;
    bool[] m_isSkillAvailable;
    bool m_isPlayerInvincible = false;
    bool m_isGamePaused = false;

    // SPIDER BOSS ------------------
    SpiderBossTrigger m_spiderBossTrigger;
    bool m_isPlayerInSpiderBossFight = false;
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            Initiate();
        }
        else { Destroy(gameObject); }
    }

    void Initiate()
    {
        m_playerHealth = PLAYER_MAX_HEALTH;
        m_isSkillAvailable = new bool[(int)SKILLS.LAST_NO_USE];
        for (int i = 0; i < m_isSkillAvailable.Length; i++) { m_isSkillAvailable[i] = false; }
        m_spellCooldown = GameObject.FindObjectOfType<SpellCooldown>();
        m_panel.SetActive(false);
        m_spiderBossTrigger = GameObject.FindObjectOfType<SpiderBossTrigger>();
    }

    static public GameManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    public int PlayerMaxHealth{
        get { return PLAYER_MAX_HEALTH; }
    }

    public void ModifyPlayerHealth(int p_amount)
    {
        m_playerHealth += p_amount;
        if (m_playerHealth <= 0 && !m_isPlayerInvincible)
        {
            m_playerHealth = 0;
            Player.Instance.HandleDeath();
            if(m_isPlayerInSpiderBossFight){
                HandlePlayerDeathInSpiderBossBattle();
            }
        }
        else if (m_playerHealth > PLAYER_MAX_HEALTH) { m_playerHealth = PLAYER_MAX_HEALTH; }

        m_healthUI.SetHealth(m_playerHealth);
    }

    public void GainExtraHeart(){
        PLAYER_MAX_HEALTH++;
        m_healthUI.GainExtraHeart();
    }

    public void RestorePlayerToFullHealth()
    {
        m_playerHealth = PLAYER_MAX_HEALTH;
        m_healthUI.SetHealth(m_playerHealth);
    }

    public float PlayerAttackDamage { get { return m_playerAttackDamage; } }
    public int PlayerHealth { get { return m_playerHealth;}}

    public bool GetIsSkillAvailable(SKILLS p_skill)
    {
        return m_isSkillAvailable[(int)p_skill];
    }

    public void SetIsSkillAvailable(SKILLS p_skill, bool p_value)
    {
        m_isSkillAvailable[(int)p_skill] = p_value;
        switch (p_skill)
        {
            default: break;
            case SKILLS.FIRE_BALL:
                { m_spellCooldown.SetFireballUI(p_value); }
                break;
            case SKILLS.GROUNDBREAKER:
                { m_spellCooldown.SetGroundbreakerUI(p_value); }
                break;
            case SKILLS.PILAR:
                { m_spellCooldown.SetPilarUI(p_value); }
                break;
        }
    }

    public bool PlayerInvincible
    {
        get { return m_isPlayerInvincible; }
        set { m_isPlayerInvincible = value; }
    }

    public bool IsGamePaused {
        get { return m_isGamePaused; }
        set {
            m_isGamePaused = value;
            if(m_isGamePaused == false){
                InputManager.Instance.PauseInputFor1Frame();
            }
            m_panel.SetActive(m_isGamePaused);
        }
    }

    public void HandlePlayerDeathInSpiderBossBattle(){
        m_spiderBossTrigger.HandlePlayerDeath();
    }

    public bool IsPlayerInSpiderBossFight{
        set { m_isPlayerInSpiderBossFight = value; }
    }

}