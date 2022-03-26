using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField ]ROOMS m_ID;
    [SerializeField] ROOMS[] m_adjacentRooms;

    Enemy[] m_enemies;

    private void Start() {
        RoomManager.Instance.AddRoom(this);
    }

    public void Reset(){
        if (m_enemies == null) { return; }
        for(int i = 0; i < m_enemies.Length; i++){
            m_enemies[i].Reset();
        }
    }

    public ROOMS ID { get { return m_ID;}}
    public ROOMS[] AdjacentRooms{ get { return m_adjacentRooms; }}

}
