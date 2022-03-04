using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Fireball : MonoBehaviour
{
    [SerializeField] SpellCooldown m_spellCooldownScript;

    Timer m_cooldownTimer;
    Player m_playerScript;

    float m_cooldown = 3.0f;

    bool m_isFireBallAvailable = true;

    Fireball m_fireBallScript;
    [SerializeField] GameObject m_fireBall;

    private void Awake()
    {
        m_cooldownTimer = gameObject.AddComponent<Timer>();
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_fireBallScript = m_fireBall.GetComponent<Fireball>();
    }

    private void Update()
    {
        if (m_cooldownTimer.IsRunning)
        {
            m_spellCooldownScript.FillFireballCooldownUI(m_cooldownTimer.CurrentTime / m_cooldownTimer.Duration);
        }
        else
        {
            m_spellCooldownScript.FillFireballCooldownUI(1);
        }
    }

    void Start()
    {
        m_cooldownTimer.Duration = m_cooldown;
        m_fireBall.SetActive(false);
    }

    public void Fireball(bool p_isFireballUnlocked)
    {
        if(!p_isFireballUnlocked) {
            return;
            Debug.Log("hola");
        }
        if (InputManager.Instance.Skill1ButtonPressed && m_cooldownTimer.IsFinished && m_isFireBallAvailable)
        {
            m_fireBall.SetActive(true);
            m_isFireBallAvailable = false;
            m_fireBallScript.Fire();
            m_cooldownTimer.Run();
        }
    }

    public bool IsFireBallAvailable { set { m_isFireBallAvailable = value; } }
}