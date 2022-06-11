using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    [SerializeField] GameObject m_drop;

    Timer m_eventTimer;
    [Tooltip("Time between each drop.")]
    [SerializeField] float m_frecuency = 1.0f;
    [SerializeField] Transform m_spawnPosition;
    

    private void Awake() {
        m_eventTimer = gameObject.AddComponent<Timer>();
        m_eventTimer.Duration = m_frecuency;
        m_eventTimer.Run();
    }

    private void Update() {
        if(m_eventTimer.IsFinished){
            Instantiate(m_drop, m_spawnPosition.position, Quaternion.identity);
            m_eventTimer.Run();
        }
    }

}
