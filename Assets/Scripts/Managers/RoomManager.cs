using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] 
    static RoomManager m_instance;

    Room[] m_rooms;

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
        else { Destroy(this.gameObject);}

        m_rooms = new Room[(int)ROOMS.LAST_NO_USE];
        m_currentRoom = m_initialRoom;
        m_lastRoom = m_initialRoom;
        m_scriptPlayerTimer = gameObject.AddComponent<Timer>();

    }

    private void Start() {

        GameObject[] roomsInWorld = GameObject.FindGameObjectsWithTag("room");

        foreach(GameObject room in  roomsInWorld ){
            Room script = room.GetComponentInChildren<Room>();
            if(script != null){
                AddRoom(script);
            }
            
        }

        GameObject[] enemiesInWorld = GameObject.FindGameObjectsWithTag("enemy");

        foreach(GameObject enemy in  enemiesInWorld ){
            Enemy script = enemy.GetComponent<Enemy>();
            m_rooms[(int)script.Room].AddEnemy(script);
        }

        GameObject[] vinesInWorld = GameObject.FindGameObjectsWithTag("vine");

        foreach(GameObject vine in  vinesInWorld ){
            VineDestroyer script = vine.GetComponent<VineDestroyer>();
            if(script != null && script.RespawnOnRoomCHange){
                m_rooms[(int)script.Room].AddVine(script);
            }
        }

        for(int i = 0; i < (int)ROOMS.LAST_NO_USE; i++){
            if( m_rooms[i] != null && m_rooms[i].ID != m_currentRoom){
                m_rooms[i].gameObject.SetActive(false);
            }
        }

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
             Player.Instance.ScriptWalk(Player.Instance.FacingDirection()); 
        }
        else{
            if(m_isGoingUpwards){
                Player.Instance.ScriptTopImpulse(new Vector2(50,150));
            }
            else{
                Player.Instance.ScriptFall();
            }
        }
        m_isBeingScripted = true;
        m_scriptPlayerTimer.Run();
    }

    public void HandleRoomTransition(ROOMS p_roomToTransition){
        m_lastRoom = m_currentRoom;
        m_currentRoom = p_roomToTransition;

    }

    public Room GetCurrentRoom(){
        return m_rooms[(int)m_currentRoom];
    }

    public ROOMS CurrentRoom { get { return m_rooms[(int)m_currentRoom].ID; }}

}
