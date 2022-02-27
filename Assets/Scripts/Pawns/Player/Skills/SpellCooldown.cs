using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellCooldown : MonoBehaviour
{
    [SerializeField] Image m_fireballCooldownUI;
    [SerializeField] Image m_pilarCooldownUI;
    [SerializeField] Image m_groundbreakerCooldownUI;

    public void FillFireballCooldownUI(float p_percentage) { m_fireballCooldownUI.fillAmount = 1 - p_percentage; }
    public void FillPilarCooldownUI(float p_percentage) { m_pilarCooldownUI.fillAmount = 1 - p_percentage; }
    public void FillGroundbreakerCooldownUI(float p_percentage) { m_groundbreakerCooldownUI.fillAmount = 1 - p_percentage; }
}