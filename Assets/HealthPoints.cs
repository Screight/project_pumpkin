using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] Image m_firstHearth;
    [SerializeField] Image m_secondHearth;
    [SerializeField] Image m_thirdHearth;

    [SerializeField] Sprite m_fullHearth;
    [SerializeField] Sprite m_emptyHearth;

    Image[] p_hearths;
    int m_currentNumberOfHearths = 3;

    private void Start()
    {
        p_hearths = new Image[]{ m_firstHearth, m_secondHearth, m_thirdHearth};
        p_hearths[0].sprite = m_fullHearth;
        p_hearths[1].sprite = m_fullHearth;
        p_hearths[2].sprite = m_fullHearth;
    }

    public void GainHealth()
    {
        if (m_currentNumberOfHearths < 3)
        {
            p_hearths[m_currentNumberOfHearths].sprite = m_fullHearth;
            m_currentNumberOfHearths++;
        }
    }
    public void LoseHearth()
    {
        if(m_currentNumberOfHearths > 0)
        {
            p_hearths[m_currentNumberOfHearths - 1].sprite = m_emptyHearth;
            m_currentNumberOfHearths--;
        }
    }

}
