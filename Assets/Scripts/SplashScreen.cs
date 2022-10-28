using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    CanvasGroup m_canvas;

    Timer m_eventTimer;
    Timer m_alphaTimer;
    [SerializeField] float m_splashScreenDuration = 2.0f;
    [SerializeField] float m_fadeOutDuration = 1.0f;
    [SerializeField] float m_alphaTicDuration = 0.1f;
    [SerializeField] float m_alphaTic = 0.1f;
    [SerializeField] GameObject m_menu;
    bool m_hasBeenShowed = false;
    bool m_hasMusicBeenPlayed = false;

    private void Awake() {
        m_canvas = GetComponent<CanvasGroup>();
        m_eventTimer = gameObject.AddComponent<Timer>();
        m_alphaTimer = gameObject.AddComponent<Timer>();
        m_alphaTimer.Duration = m_alphaTicDuration;
    }

    private void Start() {
        m_eventTimer.Duration = m_splashScreenDuration;
        m_eventTimer.Run();
    }

    private void Update() {

        if(!m_hasMusicBeenPlayed){
            SoundManager.Instance.StopBackground();
            m_menu.SetActive(false);
            m_hasMusicBeenPlayed = true;
        }

        if(m_eventTimer.IsFinished){
            if(!m_hasBeenShowed){
                m_eventTimer.Duration = m_fadeOutDuration;
                m_eventTimer.Run();
                m_hasBeenShowed = true;
            }
            else{
                if(m_alphaTimer.IsFinished){
                    if(m_canvas.alpha > 0){
                        m_canvas.alpha -= m_alphaTic;
                        m_alphaTimer.Run();
                    } else{
                        SoundManager.Instance.PlayBackground(BACKGROUND_CLIP.MAINTITLE, false);
                        m_menu.SetActive(true);
                        gameObject.SetActive(false);
                    }
                }
            }
            
        }
    }
}
