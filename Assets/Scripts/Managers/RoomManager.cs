using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] 
    static RoomManager m_instance;

    List<Room> m_rooms;

    ROOMS m_lastRoom;
    ROOMS m_currentRoom;
    [SerializeField] ROOMS m_initialRoom;

    [SerializeField] Transicion m_transicionScript;
    Timer m_scriptPlayerTimer;
    bool m_isBeingScripted = false;
    bool m_isCurrentTransitionHorizontal;
    bool m_isGoingUpwards;
    Vector3 m_positionToTransitionCameraTo;
    
    static public RoomManager Instance {
        get {return m_instance; }
        private set {}
    }

    private void Awake() {
        if(m_instance == null){ m_instance = this; }
        else { Destroy(this);}

        m_currentRoom = m_initialRoom;
        m_lastRoom = m_initialRoom;
        m_scriptPlayerTimer = gameObject.AddComponent<Timer>();
        m_rooms = new List<Room>();
    }

    private void Start() {

        GameObject[] roomsInWorld = GameObject.FindGameObjectsWithTag("room");

        foreach(GameObject room in  roomsInWorld ){
            Room script = room.GetComponentInChildren<Room>();
            if(script != null){
                AddRoom(script);
            }
            
        }

        /*for(int i = 0; i < Room.; i++){
            if( m_rooms[i] != null && m_rooms[i].ID != m_currentRoom){
                m_rooms[i].gameObject.SetActive(false);
            }
        }*/

        foreach(Room room in m_rooms){
            room.gameObject.SetActive(false);
        }

    }

    private void Update() {
        /*if(m_isBeingScripted && m_scriptPlayerTimer.IsFinished) {
            Player.Instance.StopScripting();
            m_isBeingScripted = false;
        }*/
    }

    public void AddRoom(Room p_room){
         m_rooms.Add(p_room);
    }

    public void ChangeRoom(){
        // Reset enemies position and set all to unactive
        m_rooms[(int)m_lastRoom].gameObject.GetComponent<Room>().Reset();
        m_rooms[(int)m_lastRoom].gameObject.SetActive(false);

        // Change current room and set all to active
        //m_currentRoom = m_roomToTransicion;
        m_rooms[(int)m_currentRoom].gameObject.SetActive(true);

    }

    public void StartRoomTransicion(bool p_isCurrentTransitionHorizontal, float p_playerScriptingDuration, bool p_isGoingUpwards){
        m_transicionScript.FadeIn();
        m_isCurrentTransitionHorizontal = p_isCurrentTransitionHorizontal;
        m_scriptPlayerTimer.Duration = p_playerScriptingDuration;
        m_isGoingUpwards = p_isGoingUpwards;
        Player.Instance.SetPlayerToScripted();
    }

    public void StartPlayerScripting(){
        if(m_isCurrentTransitionHorizontal){
             Player.Instance.ScriptWalk(Player.Instance.FacingDirection(), 0.2f); 
        }
        else{
            if(m_isGoingUpwards){
                Player.Instance.ScriptTopImpulse(new Vector2(50,150));
            }
            else{
                //Player.Instance.ScriptFall();
            }
        }
        m_isBeingScripted = true;
        m_scriptPlayerTimer.Run();
    }

    public void HandleRoomTransition(ROOMS p_roomToTransition){
        m_lastRoom = m_currentRoom;
        m_currentRoom = p_roomToTransition;

    }

    public Room GetRoom(int p_ID){
        foreach(Room currentRoom in m_rooms){
            if(currentRoom.ID == p_ID){
                return currentRoom;
            }
        }
        return null;
    }

    public int CurrentRoom { get { return m_rooms[(int)m_currentRoom].ID; }}

}
