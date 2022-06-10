using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapInterface : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI m_hearts;
    int m_numberOfHearts = 0;
    [SerializeField] TMPro.TextMeshProUGUI m_attack;
    int m_numberOfAttacks = 0;

    [SerializeField] GameObject m_fireSpirit;
    [SerializeField] GameObject m_darkSpirit;
    [SerializeField] GameObject m_groundbreakerRune;

    private void Awake() {
        m_fireSpirit.SetActive(false);
        m_darkSpirit.SetActive(false);
        m_groundbreakerRune.SetActive(false);
    }

    public void UpdateSpirits(){

        m_fireSpirit.SetActive(GameManager.Instance.GetIsSkillAvailable(SKILLS.FIRE_BALL));
        m_darkSpirit.SetActive(GameManager.Instance.GetIsSkillAvailable(SKILLS.DASH));
        m_groundbreakerRune.SetActive(GameManager.Instance.GetIsSkillAvailable(SKILLS.GROUNDBREAKER));
    }

    public void AddHeart(){
        m_numberOfHearts++;
        m_hearts.text = m_numberOfHearts + "/3";
    }

    public void AddAttack(){
        m_numberOfAttacks++;
        m_attack.text =  m_numberOfAttacks + "/3";
    }

}
