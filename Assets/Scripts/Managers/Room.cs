using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField ]ROOMS m_ID;
    [SerializeField] ROOMS[] m_adjacentRooms;

    List<Enemy> m_enemies = new List<Enemy>();

    private void OnEnable() {
        //RoomManager.Instance.AddRoom(this);
    }

    public void Reset(){
        if (m_enemies == null) { return; }
        for(int i = 0; i < m_enemies.Count; i++){
            m_enemies[i].gameObject.SetActive(true);
            m_enemies[i].Reset();
        }
    }

    public ROOMS ID { get { return m_ID;}}
    public ROOMS[] AdjacentRooms{ get { return m_adjacentRooms; }}
    public void AddEnemy(Enemy p_enemy){
        m_enemies.Add(p_enemy);
    }

}
