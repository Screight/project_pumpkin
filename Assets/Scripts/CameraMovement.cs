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

    private void Awake()
    {
        m_playerBoxCollider2D = m_player.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        m_playerScript = m_player.GetComponent<Player>();
        m_offset = new Vector3(20, 0, -10);

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
            m_dampSpeedY = 0.4f;
        }
        else
        {
            m_dampSpeedY = 0.5f;
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

    void FixedUpdate()
    {
        if (!m_playerOutOfCameraBoundsY)
        {
            if (transform.position.y + cameraBoxHeight / 2 == m_player.transform.position.y)
            {
                m_doLerp = false;
            }
        }

        targetPosition = new Vector3(m_player.transform.position.x + m_offset.x, m_player.transform.position.y + cameraBoxHeight / 2, m_offset.z);

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

}