using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraMovement : MonoBehaviour
{
    GameObject m_player;
    Player m_playerScript;

    [SerializeField] float m_timeToReachPlayerMaxSpeed;
    float m_cameraSpeed;
    float m_cameraAceleration;
    float m_playerMaxSpeed;
    Rigidbody2D m_rb2D;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        m_playerScript = m_player.GetComponent<Player>();
        m_playerMaxSpeed = 60;
        m_cameraAceleration = m_playerMaxSpeed / m_timeToReachPlayerMaxSpeed;
    }

    private void LateUpdate()
    {
        if( m_playerScript.IsFacingRight && transform.position.x < m_player.transform.position.x)
        {
            m_cameraSpeed += m_cameraAceleration * Time.deltaTime;
            
        }
        else if (m_playerScript.IsFacingRight && transform.position.x > m_player.transform.position.x)
        {
            m_cameraSpeed = m_playerMaxSpeed;
        }

        m_rb2D.velocity = new Vector2(m_cameraSpeed, 0);

    }
}
