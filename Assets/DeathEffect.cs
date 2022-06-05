using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] GameObject m_explosionPrefab;
    [SerializeField] float m_speed = 20.0f;
    [SerializeField] int m_numberOfExplosion = 6;
    List<GameObject> m_explosion;

    Timer m_explosionFadeTimer;
    bool m_isRunning = false;
    Vector3 m_workingVector;

    private void Awake() {
        m_explosionFadeTimer = gameObject.AddComponent<Timer>();
        m_explosion = new List<GameObject>();

    }

    private void Start() {
        m_explosionFadeTimer.Duration = AnimationManager.Instance.GetClipDuration(m_explosionPrefab.GetComponent<Animator>(),"explosion");
        m_workingVector.z = 0;
    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.Z)){
            Explode();
        }

        if(m_explosionFadeTimer.IsFinished && m_isRunning){
            m_isRunning = false;
            for(int i = 0; i < m_numberOfExplosion; i++){
                Destroy(m_explosion[0]);
                m_explosion.RemoveAt(0);
            }
        }
        else if (m_isRunning){
            for(int i = 0; i < m_numberOfExplosion; i++){

                m_workingVector.x = Mathf.Cos(i *2 * Mathf.PI / m_numberOfExplosion);
                m_workingVector.y = Mathf.Sin(i *2 * Mathf.PI / m_numberOfExplosion);

                m_explosion[i].transform.position += m_speed * Time.deltaTime * m_workingVector;
            }
        }
    }

    public void Explode(){
        GameObject explosion;
        for(int i = 0; i < m_numberOfExplosion; i++){
            explosion = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
            m_explosion.Add(explosion);
        }
        m_isRunning = true;
        m_explosionFadeTimer.Run();
    }

}
