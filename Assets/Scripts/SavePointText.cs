using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePointText : MonoBehaviour
{
    [SerializeField] float m_textDuration;
    Timer m_timer;
    bool m_isTextShowing;

    private void Awake()
    {
        m_timer = gameObject.AddComponent<Timer>();
        m_timer.Duration = m_textDuration;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_timer.IsFinished && m_isTextShowing)
        {
            m_isTextShowing = false;
            gameObject.SetActive(false);
        }
    }

    public void ActivateText()
    {
        if (m_timer.IsFinished)
        {
            m_isTextShowing = true;
            gameObject.SetActive(true);
            m_timer.Run();
        }
        else { m_timer.Restart(); }
    }
}