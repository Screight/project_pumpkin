using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePointText : MonoBehaviour
{
    [SerializeField] float m_textDuration;
    TMPro.TextMeshProUGUI m_text;
    Timer m_timer;
    bool m_isTextShowing;

    private void Awake() {
        m_text = GetComponent<TMPro.TextMeshProUGUI>();
        m_timer = gameObject.AddComponent<Timer>();
        m_timer.Duration = m_textDuration;
        m_text.enabled = false;
    }

    private void Update() {
        if(m_timer.IsFinished && m_isTextShowing){
            m_isTextShowing = false;
            m_text.enabled = false;
        }
    }

    public void ActivateText(){
        if(m_timer.IsFinished){
            m_isTextShowing = true;
            m_text.enabled = true;
            m_timer.Run();
        }
        else { m_timer.Restart();}
        
    }

}
