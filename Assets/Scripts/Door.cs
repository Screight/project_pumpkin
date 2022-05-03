using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool m_isOpen = true;
    float m_speed;
    [SerializeField] float m_time = 1.0f;
    [SerializeField] BoxCollider2D m_collider;
    float m_heigth;
    Vector3 m_openPosition;
    Vector3 m_closePosition;
    bool m_event = false;

    private void Awake() {
    }

    private void Start() {
        m_heigth = m_collider.size.y;
        m_speed = m_heigth / m_time;
        m_closePosition = transform.position;
        m_openPosition = new Vector3(transform.position.x, transform.position.y + m_heigth, transform.position.z);

        if(m_isOpen){
            transform.position = m_openPosition;
            m_collider.transform.position = m_openPosition;
        }
        //m_collider.enabled = !m_isOpen;
    }

    private void Update() {

        if(m_event){
            if(!m_isOpen && transform.position.y > m_closePosition.y){
                transform.position += new Vector3(0, -m_speed * Time.deltaTime, 0);
            }
            else if(!m_isOpen){
                transform.position = m_closePosition;
                m_event = false;
            }
            else if(m_isOpen && transform.position.y < m_openPosition.y){
                transform.position += new Vector3(0, m_speed * Time.deltaTime, 0);
                m_collider.transform.position = transform.position;
            }
            else if(m_isOpen){
                transform.position = m_openPosition;
                m_event = false;
            }
        }
    }

    public void OpenDoor(){
        if(!m_isOpen){
            ChangeDoorState();
        }
    }

    public void CloseDoor(){
        if(m_isOpen){
            ChangeDoorState();
        }
    }

    void ChangeDoorState(){
        if(m_event){ return ;}
        SoundManager.Instance.PlayOnce(AudioClipName.PILAR);
        m_isOpen = !m_isOpen;
        m_event = true;
            if(!m_isOpen){
                m_collider.transform.position = m_closePosition;
            }
    }

}
