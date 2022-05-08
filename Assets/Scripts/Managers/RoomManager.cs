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

    Vector3 m_positionToTransitionCameraTo;
    
    static public RoomManager Instance {
        get {return m_instance; }
        private set {}
    }

    private void Awake() {
        if(m_instance == null){ m_instance = this; }
        else { Destroy(this);}

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
            //room.gameObject.SetActive(false);
        }

    }

    public void AddRoom(Room p_room){
         m_rooms.Add(p_room);
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
