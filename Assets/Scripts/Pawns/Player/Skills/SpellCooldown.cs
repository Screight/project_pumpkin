using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellCooldown : MonoBehaviour
{
    [SerializeField] GameObject m_fireballUI;
    [SerializeField] GameObject m_groundbreakerUI;
    [SerializeField] GameObject m_pilarUI;
    [SerializeField] Image m_fireballCooldownUI;
    [SerializeField] Image m_pilarCooldownUI;
    [SerializeField] Image m_groundbreakerCooldownUI;

    private void Awake()
    {
    }

    private void Start()
    {
        if (!GameManager.Instance.GetIsSkillAvailable(SKILLS.FIRE_BALL))
        {
            m_fireballUI.SetActive(false);
        }
        if (!GameManager.Instance.GetIsSkillAvailable(SKILLS.GROUNDBREAKER))
        {
            m_groundbreakerUI.SetActive(false);
        }
        if (!GameManager.Instance.GetIsSkillAvailable(SKILLS.PILAR))
        {
            m_pilarUI.SetActive(false);
        }
    }

    public void FillFireballCooldownUI(float p_percentage) { m_fireballCooldownUI.fillAmount = 1 - p_percentage; }
    public void FillPilarCooldownUI(float p_percentage) { m_pilarCooldownUI.fillAmount = 1 - p_percentage; }
    public void FillGroundbreakerCooldownUI(float p_percentage) { m_groundbreakerCooldownUI.fillAmount = 1 - p_percentage; }

    public void SetFireballUI(bool p_isActive) { m_fireballUI.SetActive(p_isActive); }

    public void SetGroundbreakerUI(bool p_isActive) {  m_groundbreakerUI.SetActive(p_isActive);  }

    public void SetPilarUI(bool p_isActive) { m_pilarUI.SetActive(p_isActive); }
     
}