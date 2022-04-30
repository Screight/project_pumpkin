using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float m_duration;
    float m_magnitude;
    float m_initialSmoothTime;
    float m_finalSmoothTime;
    Timer m_eventTimer;
    bool m_isShaking = false;
    Vector3 m_initialPosition;
    static CameraShake m_instance;
    static public CameraShake Instance { 
        get { return m_instance;}
        private set {}
    }
    private void Awake() {
        if(m_instance == null){
            m_instance = this;
            Initialize();
        }
        else { Destroy(this.gameObject);}
    }

    void Initialize(){
        m_eventTimer = gameObject.AddComponent<Timer>();
    }

    private void Start() {
        
    }

    private void Update() {
        if(!m_isShaking){ return ;}
        SmoothShake();
    }

    void SmoothShake(){
        if(!m_eventTimer.IsFinished){
            float magnitude = m_magnitude;

            if(m_eventTimer.CurrentTime < m_initialSmoothTime){
                magnitude = m_eventTimer.CurrentTime  / m_initialSmoothTime;
            }
            else if(m_eventTimer.CurrentTime > m_eventTimer.Duration - m_finalSmoothTime){
                magnitude = (m_eventTimer.Duration - m_eventTimer.CurrentTime)  / m_finalSmoothTime;
            }

            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.position = new Vector3(x, y, transform.position.z);

        }
        else{
            m_isShaking = false;
            transform.position = m_initialPosition;
        }
    }

    void Shake(){
        if(!m_eventTimer.IsFinished){
            float x = Random.Range(-1, 1) * m_magnitude;
            float y = Random.Range(-1, 1) * m_magnitude;

            transform.position = new Vector3(x, y, transform.position.z);

        }
        else{
            m_isShaking = false;
            transform.position = m_initialPosition;
        }
    }

    public void InitializeShake(float p_duration, float p_magnitude, float p_initialSmoothTime, float p_finalSmoothTime){
        m_initialPosition = transform.position;
        m_magnitude = p_magnitude;
        m_duration = p_duration;
        m_initialSmoothTime = p_initialSmoothTime;
        m_finalSmoothTime = p_finalSmoothTime;
        m_isShaking = true;
        m_eventTimer.Duration = m_duration;
        m_eventTimer.Run();
    }

    void InitializeShake(float p_duration, float p_magnitude){
        m_initialPosition = transform.position;
        m_magnitude = p_magnitude;
        m_duration = p_duration;
        m_initialSmoothTime = 0;
        m_finalSmoothTime = 0;
        m_isShaking = true;
        m_eventTimer.Duration = m_duration;
        m_eventTimer.Run();
    }

}
