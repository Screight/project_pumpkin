using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject m_player;
    private Player m_playerScript;
    private Rigidbody2D m_rb2DPlayer;

    [SerializeField] float m_dampSpeedUp = 0.3f;
    [SerializeField] float m_dampSpeedDown = 0.1f;
    [SerializeField] float m_dampSpeedMovement = 0.3f;
    [SerializeField] float m_maxSpeedX = 150.0f;
    [SerializeField] float m_maxSpeedY = 50.0f;
    private float m_currentMaxSpeedY;
    private float m_dampSpeedX;
    private float m_dampSpeedY;

    [SerializeField] float m_offsetX = 20.0f;
    [SerializeField] float m_offsetY = 0.0f;
    float m_boxWidth = 0;
    [SerializeField] float m_boxHeight = 10.0f;
    private Vector3 m_velocityX = Vector3.zero;
    private Vector3 m_velocityY = Vector3.zero;
    private Vector3 m_targetPosition;

    private float m_topLimit    = float.MaxValue;
    private float m_bottomLimit = float.MaxValue;
    private float m_leftLimit   = float.MaxValue;
    private float m_rightLimit  = float.MaxValue;

    private float m_cameraWidth;
    private float m_cameraHeight;
    private bool m_isCameraStatic = false;
    private bool m_clampCamera = false;

    private float m_minimumHeightForCameraMovement = -10000000;

    private void Awake() { }

    private void Start()
    {
        m_player = Player.Instance.gameObject;
        m_rb2DPlayer = m_player.GetComponent<Rigidbody2D>();
        m_playerScript = Player.Instance;
        m_cameraWidth = CameraManager.Instance.Width;
        m_cameraHeight = CameraManager.Instance.Height;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isCameraStatic) { return; }

        m_targetPosition = new Vector3();
        
        m_dampSpeedX = m_dampSpeedMovement;
        
        if(Player.Instance.transform.position.x < transform.position.x + m_boxWidth/2 && Player.Instance.transform.position.x  > transform.position.x - m_boxWidth/2){
            m_targetPosition.x = transform.position.x;
        }else{
            m_targetPosition.x = m_player.transform.position.x + m_playerScript.FacingDirection() * m_offsetX;
        }

        if(Player.Instance.transform.position.y < transform.position.y + m_boxHeight/2 && Player.Instance.transform.position.y > transform.position.y){
            m_targetPosition.y = transform.position.y;
        }else{
            m_targetPosition.y = m_player.transform.position.y + m_offsetY;
        }

        m_targetPosition.z = transform.position.z;
        

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
            m_targetPosition.y = m_bottomLimit + m_cameraHeight / 2;
            transform.position = new Vector3(transform.position.x, m_bottomLimit + m_cameraHeight / 2, transform.position.z);
            /*if (m_player.transform.position.y > m_minimumHeightForCameraMovement)
            {
                if (transform.position.y > m_targetPosition.y && m_rb2DPlayer.velocity.y < 0) { m_dampSpeedY = m_dampSpeedDown; }
                else { m_dampSpeedY = m_dampSpeedUp; }
            }
            else
            {
                if (m_targetPosition.y < transform.position.y) { m_dampSpeedY = m_dampSpeedDown; }
                else { m_targetPosition.y = m_bottomLimit + m_cameraHeight / 2;  }
            }*/

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