using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransicion : MonoBehaviour
{
    [SerializeField] ROOMS m_rightTopRoom;
    [SerializeField] ROOMS m_leftBottomRoom;
    BoxCollider2D m_collider;

    [SerializeField] bool m_isHorizontalTransition;
    [SerializeField] float m_playerScriptingDuration = 0.5f;
    //[SerializeField] float m_transitionDuration = 2.0f;

    RoomManager m_roomManager;

    private void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }

    private void Start() { m_roomManager = RoomManager.Instance; }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if (p_collider.CompareTag("enemyProjectile")) { Destroy(p_collider.gameObject); }
        if (!p_collider.CompareTag("Player")) { return; }

        /*if (m_roomManager.CurrentRoom == m_rightTopRoom)
        {
            RoomManager.Instance.HandleRoomTransition(m_leftBottomRoom);
            if (m_isHorizontalTransition)
            {
                Player.Instance.SetPlayerToPosition(new Vector3(m_collider.bounds.min.x, m_collider.bounds.min.y - 1, Player.Instance.transform.position.z));
                Player.Instance.FacePlayerToLeft();
            }
            else
            {
                Player.Instance.SetPlayerToPosition(new Vector3(transform.position.x, m_collider.bounds.min.y - p_collider.bounds.size.y / 2, Player.Instance.transform.position.z));
            }
        }
        else
        {
            RoomManager.Instance.HandleRoomTransition(m_rightTopRoom);
            if (m_isHorizontalTransition)
            {
                Player.Instance.SetPlayerToPosition(new Vector3(m_collider.bounds.max.x, m_collider.bounds.min.y - 1, Player.Instance.transform.position.z));
                Player.Instance.FacePlayerToRight();
            }
            else
            {
                Player.Instance.SetPlayerToPosition(new Vector3(transform.position.x, m_collider.bounds.max.y - p_collider.bounds.size.y / 2, Player.Instance.transform.position.z));
                Player.Instance.FacePlayerToRight();
            }
        }
        */
        bool m_isGoingUpwards = false;
        if(Player.Instance.Speed.y > 0){ m_isGoingUpwards = true;}
        RoomManager.Instance.StartRoomTransicion(m_isHorizontalTransition, m_playerScriptingDuration, m_isGoingUpwards);
    }

    private void OnDrawGizmos()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(new Vector3(collider.bounds.min.x, collider.bounds.min.y, transform.position.z), new Vector3(collider.bounds.min.x, collider.bounds.max.y, transform.position.z));

        Gizmos.DrawLine(new Vector3(collider.bounds.max.x, collider.bounds.min.y, transform.position.z), new Vector3(collider.bounds.max.x, collider.bounds.max.y, transform.position.z));

        Gizmos.DrawLine(new Vector3(collider.bounds.max.x, collider.bounds.max.y, transform.position.z), new Vector3(collider.bounds.min.x, collider.bounds.max.y, transform.position.z));

        Gizmos.DrawLine(new Vector3(collider.bounds.min.x, collider.bounds.min.y, transform.position.z), new Vector3(collider.bounds.max.x, collider.bounds.min.y, transform.position.z));

        Gizmos.DrawLine(new Vector3(transform.position.x, collider.bounds.max.y, transform.position.z), new Vector3(transform.position.x, collider.bounds.min.y, transform.position.z));
    }
}