using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] Sprite m_icon;
    [SerializeField] bool m_showIcon;
    [SerializeField] GameObject m_activableObjects;
    [SerializeField] bool m_drawInMap = true;
    BoxCollider2D m_roomLimits;
    static int g_ID = 0;
    int m_ID;

    ZONE m_zone = ZONE.LAST_NO_USE;

    MiniMap m_miniMap;

    List<Enemy> m_enemies = new List<Enemy>();
    List<VineDestroyer> m_vines = new List<VineDestroyer>();

    private void Awake()
    {

        m_ID = g_ID;
        g_ID++;

        if (gameObject.CompareTag("Mine"))
        {
            m_zone = ZONE.MINE;
        }
        else if (gameObject.CompareTag("Forest"))
        {
            m_zone = ZONE.FOREST;
        }

        m_roomLimits = GetComponent<BoxCollider2D>();
        m_miniMap = FindObjectOfType<MiniMap>();
        Enemy[] enemies = GetComponentsInChildren<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            m_enemies.Add(enemy);
        }

        VineDestroyer[] vines = GetComponentsInChildren<VineDestroyer>();

        foreach (VineDestroyer vine in vines)
        {
            m_vines.Add(vine);
        }
        m_activableObjects.SetActive(false);
    }

    private void OnEnable() {
        //RoomManager.Instance.AddRoom(this);
    }

    public void Reset()
    {
        if (m_enemies == null) { return; }
        for (int i = 0; i < m_enemies.Count; i++)
        {
            m_enemies[i].gameObject.SetActive(true);
            m_enemies[i].Reset();
        }
        if (m_vines == null) { return; }
        for (int i = 0; i < m_vines.Count; i++)
        {
            m_vines[i].gameObject.SetActive(true);
            m_vines[i].Reset();
        }
    }

    private void OnTriggerEnter2D(Collider2D p_collider) 
    {
        if (!p_collider.CompareTag("Player")) { return; }
        if (m_miniMap != null) { m_miniMap.SetActiveRoom(m_ID); }
        m_activableObjects.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D p_collider) 
    {
        if(!p_collider.CompareTag("Player")) { return ;}
        Reset();
        m_activableObjects.SetActive(false);
        
    }

    public int ID { get { return m_ID;}}
    public void AddEnemy(Enemy p_enemy)
    {
        m_enemies.Add(p_enemy);
    }

    public void AddVine(VineDestroyer p_vine)
    {
        m_vines.Add(p_vine);
    }

    public float GetRoomHeight() { return m_roomLimits.size.y; }
    public float GetRoomWidth() { return m_roomLimits.size.x; }
    public float GetRoomOffSetX() { return m_roomLimits.offset.x; }
    public float GetRoomOffSetY() { return m_roomLimits.offset.y; }

    public bool DrawInMap { get { return m_drawInMap; }}

    public ZONE Zone { get { return m_zone; }}
    public static void ResetID(){ g_ID = 0;}
    public Sprite RoomIcon{
        get { return m_icon; }
    }
    public bool ShowIcon{
        get { return m_showIcon; }
        set { m_showIcon = value; }
    }

    public void RemoveIcon(){
        m_icon = null;
    }

}