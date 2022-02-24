using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform m_target;
    [SerializeField] Vector3 m_offset;
    float m_dampSpeedY;

    bool m_playerOutOfCameraBoundsY = false;
    bool m_doLerp = false;

    float cameraOffsetY = 0;

    Vector3 velocityX = Vector3.zero;
    Vector3 velocityY = Vector3.zero;

    [SerializeField] GameObject m_player;
    Player m_playerScript;
    BoxCollider2D m_playerBoxCollider2D;
    float cameraBoxWidth = 50;
    float cameraBoxHeight = 50;
    Vector2 cameraBoxPosition;

    Vector3 targetPosition;

    Timer m_fallTimer;
    Timer m_dampTimer;
    bool m_startFalling = false;


    float m_distance = 0;
    float m_lastPosition = 0;

    private void Awake()
    {
        
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_target = m_player.transform;
        
    }

    private void Start()
    {
        m_playerBoxCollider2D = m_player.GetComponent<BoxCollider2D>();

        m_playerScript = m_player.GetComponent<Player>();
        m_offset = new Vector3(20, 0, -10);
        m_fallTimer = gameObject.AddComponent<Timer>();
        m_dampTimer = gameObject.AddComponent<Timer>();
        m_fallTimer.Duration = 10000f;
        m_dampTimer.Duration = 1f;
    }

    private void Update()
    {
        if (m_playerScript.IsFacingRight)
        {
            m_offset.x = 20;
        }
        else
        {
            m_offset.x = -20;
        }



        if (m_playerScript.State == PLAYER_STATE.FALL)
        {
            if (m_player.GetComponent<Rigidbody2D>().velocity.y == -200)
            {
                if (m_fallTimer.IsFinished && !m_startFalling)
                {
                    m_fallTimer.Run();
                    m_startFalling = true;
                }

                if (m_fallTimer.CurrentTime >= 0.5 && m_fallTimer.IsRunning)
                {
                    m_dampTimer.Run();
                    m_fallTimer.Pause();
                }
                else if (m_dampTimer.IsRunning || m_dampSpeedY != 0)
                {
                    m_dampSpeedY = 0.4f  * ((m_dampTimer.Duration - m_dampTimer.CurrentTime) / m_dampTimer.Duration);
                    //m_offset.y = -50f * (1-((m_dampTimer.Duration - m_dampTimer.CurrentTime) / m_dampTimer.Duration));
                }

                

            }
            else
            {
                m_dampSpeedY = 0.4f;
            }

        }
        else
        {
            m_startFalling = false;
            m_fallTimer.Stop();
            m_dampTimer.Stop();
            m_dampSpeedY = 0.5f;
            m_offset.y = 0f;
        }

        if (m_player.transform.position.y + m_playerBoxCollider2D.size.y > transform.position.y - cameraOffsetY + cameraBoxHeight / 2) // sobresale por arriba
        {
            m_playerOutOfCameraBoundsY = true;
            m_doLerp = true;
        }
        else if (((m_player.transform.position.y) - (transform.position.y - cameraOffsetY)) < -cameraBoxHeight / 2) // sobresale por abajo
        {
            m_playerOutOfCameraBoundsY = true;
            m_doLerp = true;

        }
        else
        {
            m_playerOutOfCameraBoundsY = false;
        }
    }

    private void LateUpdate()
    {
        if (!m_playerOutOfCameraBoundsY)
        {
            if (transform.position.y + cameraBoxHeight / 2 == m_player.transform.position.y)
            {
                m_doLerp = false;
            }
        }

        targetPosition = new Vector3(m_player.transform.position.x + m_offset.x, m_player.transform.position.y + m_offset.y + cameraBoxHeight / 2, m_offset.z);


        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, transform.position.y, transform.position.z), ref velocityX, 0.3f);


        if (m_doLerp)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, targetPosition.y, transform.position.z), ref velocityY, m_dampSpeedY);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 10)), Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 10)));
        Gizmos.DrawWireCube(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 - 2 * m_offset.x, Screen.height / 2, 10)), new Vector3(2 * m_offset.x, cameraBoxHeight, 1));
    }

    public void SetCameraToPlayerPosition()
    {
        transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y , transform.position.z);
    }

}