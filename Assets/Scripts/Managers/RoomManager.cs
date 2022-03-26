using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] 
    static RoomManager m_instance;

    Room[] m_rooms;

    ROOMS m_currentRoom;
    ROOMS m_roomToTransicion;
    [SerializeField] ROOMS m_initialRoom;

    [SerializeField] Transicion m_transicionScript;
    Timer m_scriptPlayerTimer;
    bool m_isBeingScripted = false;
    bool m_isCurrentTransitionHorizontal;
    Vector3 m_positionToTransitionCameraTo;
    
    static public RoomManager Instance {
        get {return m_instance; }
        private set {}
    }

    private void Awake() {
        if(m_instance == null){ m_instance = this; }
        else { Destroy(this.gameObject);}

        m_rooms = new Room[(int)ROOMS.LAST_NO_USE];
        m_currentRoom = m_initialRoom;
        m_scriptPlayerTimer = gameObject.AddComponent<Timer>();

    }

    private void Update() {
        if(m_isBeingScripted && m_scriptPlayerTimer.IsFinished) {
            Player.Instance.StopScripting();
            m_isBeingScripted = false;
        }
    }

    public void AddRoom(Room p_room){
        m_rooms[(int)p_room.ID] = p_room;
    }

    private void ChangeRoom(ROOMS p_room){
        // Reset enemies position and set all to unactive
        m_rooms[(int)m_currentRoom].gameObject.GetComponent<Room>().Reset();
        m_rooms[(int)m_currentRoom].gameObject.SetActive(false);
        Debug.Log("Current Room unactive " + m_currentRoom);

        // Change current room and set all to active
        m_currentRoom = p_room;
        m_rooms[(int)m_currentRoom].gameObject.SetActive(true);
        Debug.Log("Current Room active " + m_currentRoom);

    }

    public void StartRoomTransicion(bool p_isCurrentTransitionHorizontal, float p_playerScriptingDuration){
        m_transicionScript.FadeIn();
        m_isCurrentTransitionHorizontal = p_isCurrentTransitionHorizontal;
        m_scriptPlayerTimer.Duration = p_playerScriptingDuration;
        Player.Instance.SetPlayerToScripted();
    }

    public void StartPlayerScripting(){
        if(m_isCurrentTransitionHorizontal){ Player.Instance.ScriptWalk(Player.Instance.FacingDirection()); }
        m_isBeingScripted = true;
        m_scriptPlayerTimer.Run();
        ChangeRoom(m_roomToTransicion);
    }

    public ROOMS CurrentRoom { get { return m_rooms[(int)m_currentRoom].ID; }} 
    public ROOMS RoomToTransition { set {  m_roomToTransicion = value; }} 
    public Vector3 PositionToTransitionCameraTo { set { m_positionToTransitionCameraTo = value;}}

}
