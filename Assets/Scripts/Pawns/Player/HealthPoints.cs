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

    [SerializeField] Sprite m_fullHeart;
    [SerializeField] Sprite m_emptyHeart;

    Image[] p_hearts;

    int m_currentNumberOfHearts = 4;

    private void Start()
    {
        p_hearts = new Image[]{ m_firstHeart, m_secondHeart, m_thirdHeart, m_forthHeart};
        p_hearts[0].sprite = m_fullHeart;
        p_hearts[1].sprite = m_fullHeart;
        p_hearts[2].sprite = m_fullHeart;
        p_hearts[3].sprite = m_fullHeart;
        SetHealth(GameManager.Instance.PlayerHealth);
    }

    public void SetHealth(int p_value)
    {
        Sprite sprite;
        m_currentNumberOfHearts = p_value;
        for( int i = 0; i < p_hearts.Length; i++)
        {
            if (i < m_currentNumberOfHearts) { sprite = m_fullHeart; }
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