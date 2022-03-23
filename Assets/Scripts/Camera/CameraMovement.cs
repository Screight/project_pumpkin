using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject m_player;
    Player m_playerScript;
    Rigidbody2D m_rb2DPlayer;
    BoxCollider2D m_playerBoxCollider2D;

    [SerializeField] float m_dampSpeedUp;
    [SerializeField] float m_dampSpeedDown;
    [SerializeField] float m_dampSpeedMovement;
    [SerializeField] float m_maxSpeedX;
    float m_dampSpeedX;
    float m_dampSpeedY;

    [SerializeField] float m_offsetX;
    [SerializeField] float m_offsetY;
    [SerializeField] float m_offsetAddUpY = 20;

    Vector3 m_velocityX = Vector3.zero;
    Vector3 m_velocityY = Vector3.zero;

    Vector3 m_targetPosition;

    float m_topLimit    = float.MaxValue;
    float m_bottomLimit = float.MaxValue;
    float m_leftLimit   = float.MaxValue;
    float m_rightLimit  = float.MaxValue;

    float m_minimumHeightForCameraMovement = 0;

    private void Awake() { m_player = GameObject.FindGameObjectWithTag("Player"); }

    void Start()
    {
        m_rb2DPlayer = m_player.GetComponent<Rigidbody2D>();
        m_playerBoxCollider2D = m_player.GetComponent<BoxCollider2D>();
        m_playerScript = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("down")){ m_offsetY += -m_offsetAddUpY;}
        else if(Input.GetKeyUp("down")){m_offsetY += m_offsetAddUpY;}
        if(Input.GetKeyDown("up")){ m_offsetY += m_offsetAddUpY;}
        else if(Input.GetKeyUp("up")){m_offsetY += -m_offsetAddUpY;}

        m_targetPosition = new Vector3();
        
        m_targetPosition.y = m_player.transform.position.y + m_offsetY;
        m_targetPosition.z = transform.position.z;
        m_targetPosition.x = m_player.transform.position.x + m_playerScript.FacingDirection() * m_offsetX;

        m_dampSpeedX = m_dampSpeedMovement;

        if (m_targetPosition.x - CameraManager.Instance.Width / 2 <= m_leftLimit)
        {
            m_targetPosition.x = m_leftLimit + CameraManager.Instance.Width / 2;
        }
        else if (m_targetPosition.x + CameraManager.Instance.Width / 2 >= m_rightLimit)
        {
            m_targetPosition.x = m_rightLimit - CameraManager.Instance.Width / 2;
        }

        if (m_targetPosition.y - CameraManager.Instance.Height / 2 <= m_bottomLimit)
        {
            m_targetPosition.y = m_bottomLimit + CameraManager.Instance.Height / 2;
        }
        else if (m_targetPosition.y + CameraManager.Instance.Height / 2 >= m_topLimit)
        {
            m_targetPosition.y = m_topLimit - CameraManager.Instance.Height / 2;
        }
        else 
        {
            if (m_player.transform.position.y > m_minimumHeightForCameraMovement)
            {
                if (transform.position.y > m_targetPosition.y && m_rb2DPlayer.velocity.y < 0)
                {
                    m_dampSpeedY = m_dampSpeedDown;
                }
                else { m_dampSpeedY = m_dampSpeedUp; }
            }
            else
            {
                if (m_targetPosition.y < transform.position.y) { m_dampSpeedY = m_dampSpeedDown; }
                else { m_dampSpeedY = 100000000; }
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(m_targetPosition.x, transform.position.y, m_targetPosition.z), ref m_velocityX, m_dampSpeedX, m_maxSpeedX);

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, m_targetPosition.y, m_targetPosition.z), ref m_velocityY, m_dampSpeedY);
    }

    public void SetCameraToPlayerPosition()
    {
        transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y , transform.position.z);
    }

    public float LeftLimit      { set { m_leftLimit = value; } }
    public float RightLimit     { set { m_rightLimit = value; } }
    public float TopLimit       { set { m_topLimit = value; } }
    public float BottomLimit    { set { m_bottomLimit = value; } }
    public float MinimumheightForCameraMovement { set { m_minimumHeightForCameraMovement = value; } }
}