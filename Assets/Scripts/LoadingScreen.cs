using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject m_screen;
    [SerializeField] TMPro.TextMeshProUGUI  m_text;
    int m_dotCount = 0;
    Timer m_eventTimer;
    [SerializeField] float m_dotDuration = 0.1f;
    private void Awake() {
        m_screen.SetActive(false);
        m_eventTimer = gameObject.AddComponent<Timer>();
        m_eventTimer.Duration = m_dotDuration;
    }

    public void LoadScene(SCENE p_scene){
        m_screen.SetActive(true);
        StartCoroutine(LoadAsynchronously((int)p_scene));
    }

    IEnumerator LoadAsynchronously(int p_scene){

        AsyncOperation operation = SceneManager.LoadSceneAsync(p_scene);

        while(!operation.isDone){
            m_text.text = "Loading";
            if(m_eventTimer.IsFinished){
                for(int i = 0; i < m_dotCount; i++){
                m_text.text += ".";
                }
                if(m_dotCount < 3){
                    m_dotCount++;
                }
                else{ m_dotCount = 0; }
                
                m_eventTimer.Run();
            }
            
            yield return null;
        }

    }

}
