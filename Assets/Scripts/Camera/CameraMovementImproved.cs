using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementImproved : MonoBehaviour
{
    GameObject m_player;
    Player m_playerScript;
    Rigidbody2D m_rb2DPlayer;
    BoxCollider2D m_playerBoxCollider2D;

    [SerializeField] float m_dampSpeedUp;
    [SerializeField] float m_dampSpeedDown;
    [SerializeField] float m_dampSpeedX;
    float m_dampSpeedY;

    [SerializeField] float m_offsetX;
    [SerializeField] float m_offsetY;

    Vector3 m_velocityX = Vector3.zero;
    Vector3 m_velocityY = Vector3.zero;

    Vector3 m_targetPosition;

    float m_topLimit = float.MaxValue;
    float m_bottomLimit = float.MaxValue;
    float m_leftLimit = float.MaxValue;
    float m_rightLimit = float.MaxValue;

    private void Awake() {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_rb2DPlayer = m_player.GetComponent<Rigidbody2D>();
        m_playerBoxCollider2D = m_player.GetComponent<BoxCollider2D>();
        m_playerScript = m_player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        m_targetPosition = new Vector3();
        m_targetPosition.x = m_player.transform.position.x + m_playerScript.GetFacingDirection() * m_offsetX;
        m_targetPosition.y = m_player.transform.position.y + m_offsetY;

        if(transform.position.y > m_targetPosition.y && m_rb2DPlayer.velocity.y < 0){
            m_dampSpeedY = m_dampSpeedDown;
        }
        else{ m_dampSpeedY = m_dampSpeedUp; }


        m_targetPosition.z = transform.position.z;
    }

    private void LateUpdate() {

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(m_targetPosition.x, transform.position.y, m_targetPosition.z), ref m_velocityX, m_dampSpeedX);
        
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, m_targetPosition.y, m_targetPosition.z), ref m_velocityY, m_dampSpeedY);
    }

    public void SetCameraToPlayerPosition()
    {
        transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y , transform.position.z);
    }

    public float LeftLimit { set { m_leftLimit = value; } }
    public float RightLimit { set { m_rightLimit = value; } }
    public float TopLimit { set { m_topLimit = value; } }
    public float BottomLimit { set { m_bottomLimit = value; } }

}
