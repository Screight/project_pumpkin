using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject m_player;
    Player m_playerScript;
    Rigidbody2D m_rb2DPlayer;

    [SerializeField] float m_dampSpeedUp = 0.3f;
    [SerializeField] float m_dampSpeedDown = 0.1f;
    [SerializeField] float m_dampSpeedMovement = 0.3f;
    [SerializeField] float m_maxSpeedX = 150.0f;
    [SerializeField] float m_maxSpeedY = 50.0f;
    float m_currentMaxSpeedY;
    float m_dampSpeedX;
    float m_dampSpeedY;

    [SerializeField] float m_offsetX = 20.0f;
    [SerializeField] float m_offsetY = 0.0f;
    [SerializeField] float m_offsetAddUpY = 20.0f;

    Vector3 m_velocityX = Vector3.zero;
    Vector3 m_velocityY = Vector3.zero;

    Vector3 m_targetPosition;

    float m_topLimit    = float.MaxValue;
    float m_bottomLimit = float.MaxValue;
    float m_leftLimit   = float.MaxValue;
    float m_rightLimit  = float.MaxValue;

    float m_cameraWidth;
    float m_cameraHeight;
    bool m_isCameraStatic = false;
    bool m_clampCamera = false;

    float m_minimumHeightForCameraMovement = -10000000;

    private void Awake() { m_player = GameObject.FindGameObjectWithTag("Player"); }

    void Start()
    {
        m_rb2DPlayer = m_player.GetComponent<Rigidbody2D>();
        m_playerScript = Player.Instance;
        m_cameraWidth = CameraManager.Instance.Width;
        m_cameraHeight = CameraManager.Instance.Height;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isCameraStatic) { return; }

        if(Input.GetKeyDown("down") && m_playerScript.State == PLAYER_STATE.IDLE)    { m_offsetY += -m_offsetAddUpY;}
        else if(Input.GetKeyUp("down")) {m_offsetY += m_offsetAddUpY;}

        if(Input.GetKeyDown("up") && m_playerScript.State == PLAYER_STATE.IDLE)      { m_offsetY += m_offsetAddUpY;}
        else if(Input.GetKeyUp("up"))   {m_offsetY += -m_offsetAddUpY;}

        m_targetPosition = new Vector3();
        
        m_dampSpeedX = m_dampSpeedMovement;
        
        m_targetPosition.y = m_player.transform.position.y + m_offsetY;
        m_targetPosition.z = transform.position.z;
        m_targetPosition.x = m_player.transform.position.x + m_playerScript.FacingDirection() * m_offsetX;

        if (m_targetPosition.x - m_cameraWidth / 2 <= m_leftLimit)
        {
            m_targetPosition.x = m_leftLimit + m_cameraWidth / 2;
        }
        else if (m_targetPosition.x + m_cameraWidth / 2 >= m_rightLimit)
        {
            m_targetPosition.x = m_rightLimit - m_cameraWidth / 2;
        }

        if (m_targetPosition.y - m_cameraHeight / 2 <= m_bottomLimit)
        {
            m_targetPosition.y = m_bottomLimit + m_cameraHeight / 2;
        }
        else if (m_targetPosition.y + m_cameraHeight / 2 >= m_topLimit)
        {
            m_targetPosition.y = m_topLimit - m_cameraHeight / 2;
        }
        else
        {
            if (m_player.transform.position.y > m_minimumHeightForCameraMovement)
            {
                if (transform.position.y > m_targetPosition.y && m_rb2DPlayer.velocity.y < 0) { m_dampSpeedY = m_dampSpeedDown; }
                else { m_dampSpeedY = m_dampSpeedUp; }
            }
            else
            {
                if (m_targetPosition.y < transform.position.y) { m_dampSpeedY = m_dampSpeedDown; }
                else { m_targetPosition.y = m_bottomLimit + m_cameraHeight / 2;  }
            }

        }

    }

    private void LateUpdate()
    {
        if(m_isCameraStatic && !m_clampCamera) { return; }
        if(m_clampCamera)
        {
            m_targetPosition.y = m_player.transform.position.y + m_offsetY;
            m_targetPosition.z = transform.position.z;
            m_targetPosition.x = m_player.transform.position.x + m_playerScript.FacingDirection() * m_offsetX;

            if (m_targetPosition.x - m_cameraWidth / 2 <= m_leftLimit)
            {
                m_targetPosition.x = m_leftLimit + m_cameraWidth / 2;
            }
            else if (m_targetPosition.x + m_cameraWidth / 2 >= m_rightLimit)
            {
                m_targetPosition.x = m_rightLimit - m_cameraWidth / 2;
            }

            if (m_targetPosition.y - m_cameraHeight / 2 <= m_bottomLimit)
            {
                m_targetPosition.y = m_bottomLimit + m_cameraHeight / 2;
            }
            else if (m_targetPosition.y + m_cameraHeight / 2 >= m_topLimit)
            {
                m_targetPosition.y = m_topLimit - m_cameraHeight / 2;
            }

            transform.position = new Vector3(m_targetPosition.x, m_targetPosition.y,transform.position.z);
            m_clampCamera = false;
            return;
        } 

        bool m_isMaxSpeedUnlimited = Player.Instance.State == PLAYER_STATE.FALL || Player.Instance.State == PLAYER_STATE.JUMP || Player.Instance.State == PLAYER_STATE.BOOST || Player.Instance.State == PLAYER_STATE.GROUNDBREAKER;

        if (m_isMaxSpeedUnlimited){ m_currentMaxSpeedY = 10000; }// HIGH FOR IT TO BE INSTANT
        else {m_currentMaxSpeedY = m_maxSpeedY;}
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(m_targetPosition.x, transform.position.y, m_targetPosition.z), ref m_velocityX, m_dampSpeedX, m_maxSpeedX);

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, m_targetPosition.y, m_targetPosition.z), ref m_velocityY, m_dampSpeedY, m_currentMaxSpeedY);

    }

    public void SetCameraToPlayerPosition()
    {
        transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y , transform.position.z);
    }

    public bool IsCameraStatic { set { m_isCameraStatic = value;}}
    public bool IsCameraClamped { set { m_clampCamera = value;}}
    public void ClampCamera(){ m_clampCamera = true;}

    public Vector3 GetTarget() { return m_targetPosition; }
    public float GetSpeedX() { return m_velocityX.x; }

    public float LeftLimit      { set { m_leftLimit = value; } }
    public float RightLimit     { set { m_rightLimit = value; } }
    public float TopLimit       { set { m_topLimit = value; } }
    public float BottomLimit    { set { m_bottomLimit = value; } }
    public float MinimumheightForCameraMovement { set { m_minimumHeightForCameraMovement = value; } }
}