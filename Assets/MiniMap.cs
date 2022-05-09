using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] Sprite m_sprite;
    [SerializeField] TMPro.TextMeshProUGUI m_title;
    [SerializeField] Color m_unVisitedRoom;
    [SerializeField] Color m_visitedRoom;
    [SerializeField] Color m_activeRoom;
    [SerializeField] float m_desiredIconScale = 5.0f;
    float m_iconScale;
    [SerializeField] Image m_playerIcon;
    [SerializeField] GameObject m_background;
    [SerializeField]  Vector2 m_desiredDistanceFromCenter = Vector2.zero;
    Vector2 m_distanceFromCenter = Vector2.zero;

    int m_currentActiveRoom = -1;

    Dictionary<int,int> m_roomsDictionary;
    Image[] m_rooms;
    Room[] m_roomsScript;
    [SerializeField] float m_desiredScale = 10;
    float m_scale = 10;
    Vector2 m_iconInitialSize;
    Vector2[] m_initialPositionTransformToTopLeftCorner = new Vector2[(int)ZONE.LAST_NO_USE];

    float[] m_topLimit = new float[(int)ZONE.LAST_NO_USE];
    float[] m_bottomLimit = new float[(int)ZONE.LAST_NO_USE];
    float[] m_leftLimit = new float[(int)ZONE.LAST_NO_USE];
    float[] m_rightLimit = new float[(int)ZONE.LAST_NO_USE];

    bool m_isMapActive = false;

    private void Start() {

        // 320 es la anchura de referencia
        m_scale = m_desiredScale / Screen.width * 320;
        m_distanceFromCenter.x = m_desiredDistanceFromCenter.x * Screen.width / 320;
        m_distanceFromCenter.y = m_desiredDistanceFromCenter.y * Screen.width / 320;

        RectTransform rectTransform = m_playerIcon.GetComponent<RectTransform>();
        m_iconInitialSize = rectTransform.sizeDelta;
        m_iconScale = m_desiredIconScale / Screen.width * 320;

        Room[] rooms = GameObject.FindObjectsOfType<Room>();

        m_roomsDictionary = new Dictionary<int,int>();
        m_rooms = new Image[rooms.Length];
        m_roomsScript = new Room[rooms.Length];

        for(int i = 0; i < (int)ZONE.LAST_NO_USE; i++){
            m_topLimit[i] = -100000;
            m_bottomLimit[i] = 100000;
            m_rightLimit[i] = -100000;
            m_leftLimit[i] = 100000;
            CreateMapForEachZone(rooms, (ZONE)i);
        }

        m_playerIcon.transform.SetAsLastSibling();

        HideMap();
        m_background.SetActive(false);
        m_title.transform.position += new Vector3(m_distanceFromCenter.x, m_distanceFromCenter.y, 0);
        
    }

    void CreateMapForEachZone(Room[] p_room, ZONE p_zone){
        foreach(Room room in p_room){
            CheckForMapLimits(room, p_zone); }

        m_initialPositionTransformToTopLeftCorner[(int)p_zone].x = -m_leftLimit[(int)p_zone];
        m_initialPositionTransformToTopLeftCorner[(int)p_zone].y = -m_topLimit[(int)p_zone];

        for(int i = 0; i < p_room.Length; i++){ CreateAndSetUpRoomInMap(p_room[i], i, p_zone);  }
    }

    // Compare the limits of a room to the actual ones and substitute them if they are broken
    void CheckForMapLimits(Room p_room, ZONE p_zone){
        if(!p_room.DrawInMap) { return; }
        if(p_room.Zone != p_zone) { return ;}

        float roomTopLimit = p_room.transform.position.y + p_room.GetRoomHeight()/2 + p_room.GetRoomOffSetY();
        float roomBottomLimit = p_room.transform.position.y - p_room.GetRoomHeight()/2 + p_room.GetRoomOffSetY();
        float roomRightLimit = p_room.transform.position.x + p_room.GetRoomWidth()/2 + p_room.GetRoomOffSetX();
        float roomLeftLimit = p_room.transform.position.x - p_room.GetRoomWidth()/2 + p_room.GetRoomOffSetX();

        if(roomTopLimit > m_topLimit[(int)p_zone]) { m_topLimit[(int)p_zone] = roomTopLimit; }
        if(roomBottomLimit < m_bottomLimit[(int)p_zone]) { m_bottomLimit[(int)p_zone] = roomBottomLimit; }
        if(roomRightLimit > m_rightLimit[(int)p_zone]) { m_rightLimit[(int)p_zone] = roomRightLimit; }
        if(roomLeftLimit < m_leftLimit[(int)p_zone]) { m_leftLimit[(int)p_zone] = roomLeftLimit; }
    }

    void CreateAndSetUpRoomInMap(Room p_room, int p_indexInArray, ZONE p_zone){
        if(p_room.Zone != p_zone) { return ;}

        GameObject empty = new GameObject();
        empty.transform.parent = transform;
        empty.name = "room_" + p_room.ID;

        m_rooms[p_indexInArray] = empty.AddComponent<Image>();
        m_roomsScript[p_indexInArray] = p_room;
        m_roomsDictionary.Add(p_room.ID,p_indexInArray);
            
        m_rooms[p_indexInArray].sprite = m_sprite;
        m_rooms[p_indexInArray].color = m_unVisitedRoom;

        RectTransform rectTransform = m_rooms[p_indexInArray].gameObject.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(p_room.GetRoomWidth() / m_scale, p_room.GetRoomHeight() / m_scale);

        float leftToCenter = Screen.width/2 - (m_rightLimit[(int)p_zone] - m_leftLimit[(int)p_zone])/(2*m_scale);
        float topToCenter = Screen.height/2 - (m_topLimit[(int)p_zone] - m_bottomLimit[(int)p_zone])/(2*m_scale);

        rectTransform.position = new Vector2((p_room.transform.position.x + p_room.GetRoomOffSetX() + m_initialPositionTransformToTopLeftCorner[(int)p_zone].x)/m_scale + leftToCenter + m_distanceFromCenter.x, (p_room.transform.position.y + p_room.GetRoomOffSetY() + m_initialPositionTransformToTopLeftCorner[(int)p_zone].y)/m_scale  + Screen.height - topToCenter + m_distanceFromCenter.y);
    }

    private void Update() {

        m_scale = m_desiredScale / Screen.width * 320;
        m_distanceFromCenter.x = m_desiredDistanceFromCenter.x * Screen.width / 320;
        m_distanceFromCenter.y = m_desiredDistanceFromCenter.y * Screen.width / 320;

        if(Input.GetKeyDown(KeyCode.M) && !m_isMapActive && !GameManager.Instance.IsGamePaused){
            SetActiveZone(GameManager.Instance.CurrentZone);
            m_background.SetActive(true);
            m_title.enabled = true;
            m_playerIcon.enabled = true;
            m_isMapActive = true;
            GameManager.Instance.SetGameToPaused(true, false);
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.M) && m_isMapActive){
            HideMap();
            m_background.SetActive(false);
            m_isMapActive = false;
            GameManager.Instance.SetGameToPaused(false, false);
            Time.timeScale = 1;
        }

        if(m_isMapActive){
            PositionPlayerInMap();
        }
        
    }

    void PositionPlayerInMap(){
        m_iconScale = m_desiredIconScale / Screen.width * 320;
        RectTransform rectTransform = m_playerIcon.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(m_iconInitialSize.x/m_iconScale, m_iconInitialSize.y/m_iconScale);

        float leftToCenter = Screen.width/2 - (m_rightLimit[(int)GameManager.Instance.CurrentZone] - m_leftLimit[(int)GameManager.Instance.CurrentZone])/(2*m_scale);
        float topToCenter = Screen.height/2 - (m_topLimit[(int)GameManager.Instance.CurrentZone] - m_bottomLimit[(int)GameManager.Instance.CurrentZone])/(2*m_scale);

        rectTransform.position = new Vector2((Player.Instance.transform.position.x + m_initialPositionTransformToTopLeftCorner[(int)GameManager.Instance.CurrentZone].x)/m_scale + leftToCenter + m_distanceFromCenter.x, (Player.Instance.transform.position.y + m_initialPositionTransformToTopLeftCorner[(int)GameManager.Instance.CurrentZone].y)/m_scale  + Screen.height - topToCenter + m_distanceFromCenter.y);
    }

    public void SetActiveRoom(int p_activeRoom){
        if(m_currentActiveRoom != -1){
            m_rooms[m_roomsDictionary[m_currentActiveRoom]].color = m_visitedRoom;
        }
       
        m_currentActiveRoom = p_activeRoom;
        m_rooms[m_roomsDictionary[p_activeRoom]].color = m_activeRoom;
    }

    void HideMap(){
        for(int i = 0; i < m_rooms.Length; i++){
            m_rooms[i].enabled = false;
        }
        m_playerIcon.enabled = false;
        m_title.enabled = false;
    }

    public void SetActiveZone(ZONE p_zone){
        if(m_currentActiveRoom != -1){
            //m_rooms[m_roomsDictionary[m_currentActiveRoom]].color = m_visitedRoom;
        }
        
        for(int i = 0; i < m_roomsScript.Length; i++)
        {
            if(m_roomsScript[i] != null && m_roomsScript[i].Zone == p_zone){
                m_rooms[i].enabled = true;
            }
        }
        switch(p_zone){
            case ZONE.FOREST:
                m_title.text = "Dark Forest";
            break;
            case ZONE.MINE:
                m_title.text = "Abandoned Mine";
            break;
            default:
                m_title.text = "Unknown place";
            break;
        }
        
    }

}
