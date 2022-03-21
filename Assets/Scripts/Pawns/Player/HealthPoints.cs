using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] Image m_firstHeart;
    [SerializeField] Image m_secondHeart;
    [SerializeField] Image m_thirdHeart;
    [SerializeField] Image m_forthHeart;
    [SerializeField] Image m_fifthHeart;
    [SerializeField] Image m_sixthHeart;
    [SerializeField] Image m_seventhHeart;
    [SerializeField] Image m_octoHeart;

    [SerializeField] Sprite m_fullHeart;
    [SerializeField] Sprite m_emptyHeart;

    Image[] p_hearts;

    int m_maxNumberOfHearts = 8;
    int m_currentNumberOfHearts = 8;

    private void Start()
    {
        p_hearts = new Image[]{ m_firstHeart, m_secondHeart, m_thirdHeart, m_forthHeart , m_fifthHeart, m_sixthHeart, m_seventhHeart, m_octoHeart };
        p_hearts[0].sprite = m_fullHeart;
        p_hearts[1].sprite = m_fullHeart;
        p_hearts[2].sprite = m_fullHeart;
        p_hearts[3].sprite = m_fullHeart;
        p_hearts[4].sprite = m_fullHeart;
        p_hearts[5].sprite = m_fullHeart;
        p_hearts[6].sprite = m_fullHeart;
        p_hearts[7].sprite = m_fullHeart;
    }

    public void SetHealth(int p_value)
    {
        Sprite sprite;
        m_currentNumberOfHearts = p_value;
        for( int i = 0; i < m_maxNumberOfHearts; i++)
        {
            if(i < m_currentNumberOfHearts){ sprite = m_fullHeart; }
            else { sprite = m_emptyHeart; }
            p_hearts[i].sprite = sprite;
        }
    }

    public void GainHealth()
    {
        if (m_currentNumberOfHearts < 8)
        {
            p_hearts[m_currentNumberOfHearts].sprite = m_fullHeart;
            m_currentNumberOfHearts++;
        }
    }
    public void LoseHearth()
    {
        if(m_currentNumberOfHearts > 0)
        {
            p_hearts[m_currentNumberOfHearts - 1].sprite = m_emptyHeart;
            m_currentNumberOfHearts--;
        }
    }
}