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
        if (!SkillManager.Instance.IsFireballUnlocked)
        {
            m_fireballUI.SetActive(false);
        }
        if (!SkillManager.Instance.IsGroundbreakerUnlocked)
        {
            m_groundbreakerUI.SetActive(false);
        }
        if (!SkillManager.Instance.IsPilarUnlocked)
        {
            m_pilarUI.SetActive(false);
        }
    }

    public void FillFireballCooldownUI(float p_percentage) { m_fireballCooldownUI.fillAmount = 1 - p_percentage; }
    public void FillPilarCooldownUI(float p_percentage) { m_pilarCooldownUI.fillAmount = 1 - p_percentage; }
    public void FillGroundbreakerCooldownUI(float p_percentage) { m_groundbreakerCooldownUI.fillAmount = 1 - p_percentage; }

    public void ActivateFireballUI() { m_fireballUI.SetActive(true); }

    public void ActivateGroundbreakerUI() {  m_groundbreakerUI.SetActive(true);  }

    public void ActivatePilarUI() { m_pilarUI.SetActive(true); }
     
}