using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransicion : MonoBehaviour
{
    [SerializeField] ROOMS m_room_1;
    [SerializeField] ROOMS m_room_2;
    [SerializeField] Transform m_room_1_position;
    [SerializeField] Transform m_room_2_position;

    [SerializeField] bool m_isHorizontalTransition;
    [SerializeField] float m_playerScriptingDuration = 0.5f;
    [SerializeField] float m_transitionDuration = 2.0f;
    
    bool m_isBeingScripted = false;

    RoomManager m_roomManager;

    private void Start() { m_roomManager = RoomManager.Instance; }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag != "Player"){ return; }

        if(m_roomManager.CurrentRoom == m_room_1){ 
            m_roomManager.RoomToTransition = m_room_2;
            RoomManager.Instance.PositionToTransitionCameraTo = m_room_2_position.position;
        }
        else { 
            m_roomManager.RoomToTransition = m_room_1;
            RoomManager.Instance.PositionToTransitionCameraTo = m_room_1_position.position;
            }

        RoomManager.Instance.StartRoomTransicion(m_isHorizontalTransition, m_playerScriptingDuration);
        

    }

    private void OnDrawGizmos() {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(collider.bounds.size.x, collider.bounds.size.y,1));
        Gizmos.DrawLine(new Vector3(transform.position.x, collider.bounds.max.y, transform.position.z), new Vector3(transform.position.x, collider.bounds.min.y, transform.position.z));
    }

}
