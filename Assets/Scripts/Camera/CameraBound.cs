using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION {TOP, BOTTOM, LEFT, RIGHT, LAST_NO_USE}

public struct Limit{

    

    public Limit(DIRECTION p_direction, float p_position, uint p_layerIndex, uint p_boundID){
        direction = p_direction;
        position = p_position;
        layerIndex = p_layerIndex;
        cameraBoundID = p_boundID;
    }
    public DIRECTION direction;
    public float position;
    public uint layerIndex;
    public uint cameraBoundID;
}

public class CameraBound : MonoBehaviour
{
    public static uint m_nextAvailableID = 0;

    [SerializeField] uint m_layerIndex = 0;
    [SerializeField] bool m_isTopLimitActive = false;
    [SerializeField] bool m_isBottomLimitActive = false;
    [SerializeField] bool m_isLeftLimitActive = false;
    [SerializeField] bool m_isRightLimitActive = false;

    // NEEDS TO BE UNIQUE
    uint m_ID;

    Limit[] m_limits;
    bool[] m_limitsActive;

    [SerializeField] BoxCollider2D m_boundsCollider;

    private void Awake() {
        m_limitsActive = new bool[(int)DIRECTION.LAST_NO_USE];
        m_limits = new Limit[(int)DIRECTION.LAST_NO_USE];

        m_limitsActive[(int)DIRECTION.TOP] = m_isTopLimitActive;
        m_limitsActive[(int)DIRECTION.BOTTOM] = m_isBottomLimitActive;
        m_limitsActive[(int)DIRECTION.LEFT] = m_isLeftLimitActive;
        m_limitsActive[(int)DIRECTION.RIGHT] = m_isRightLimitActive;

        m_boundsCollider = GetComponent<BoxCollider2D>();

        Vector2 boundsColliderPosition = new Vector2(m_boundsCollider.transform.position.x, m_boundsCollider.transform.position.y);

        m_ID = m_nextAvailableID;
        m_nextAvailableID++;

        for(int i = 0; i < (int)DIRECTION.LAST_NO_USE; i++){

            float boundPosition = 0;
            switch(i){
                default: break;
                case (int)DIRECTION.TOP:
                boundPosition = m_boundsCollider.bounds.max.y;
                break;
                case (int)DIRECTION.BOTTOM:
                boundPosition = m_boundsCollider.bounds.min.y;
                break;
                case (int)DIRECTION.LEFT:
                boundPosition = m_boundsCollider.bounds.min.x;
                break;
                case (int)DIRECTION.RIGHT:
                boundPosition = m_boundsCollider.bounds.max.x;
                break;
            }
            m_limits[i] = new Limit((DIRECTION)i, boundPosition,m_layerIndex, m_ID);
        }
    }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag != "Player"){return;}
        for(int i = 0; i < (int)DIRECTION.LAST_NO_USE; i++){
            if(m_limitsActive[i]){ BoundsManager.Instance.AddLimit(m_limits[i]); }
        }
        BoundsManager.Instance.UpdateBounds();
    }

    private void OnTriggerExit2D(Collider2D p_collider) {
        BoundsManager.Instance.DeleteLimits(m_ID);
        BoundsManager.Instance.UpdateBounds();
    }

    public Limit TopLimit{
        get {return m_limits[(int)DIRECTION.TOP];}
    }
    public Limit BottomLimit{
        get {return m_limits[(int)DIRECTION.BOTTOM];}
    }
    public Limit LeftLimit{
        get {return m_limits[(int)DIRECTION.LEFT];}
    }
    public Limit RightLimit{
        get {return m_limits[(int)DIRECTION.RIGHT];}
    }

    private void OnDrawGizmos() {
        switch(m_layerIndex){
            case 0:
            Gizmos.color = Color.green;
            break;
            case 1:
            Gizmos.color = Color.red;
            break;
            case 2:
            Gizmos.color = Color.blue;
            break;
            case 3:
            Gizmos.color = Color.yellow;
            break;
        }
        
        Vector2 bottomLeftCorner = new Vector2(m_boundsCollider.bounds.min.x, m_boundsCollider.bounds.min.y);
        Vector2 bottomRightCorner = new Vector2(m_boundsCollider.bounds.max.x, m_boundsCollider.bounds.min.y);
        Vector2 topLeftCorner = new Vector2(m_boundsCollider.bounds.min.x, m_boundsCollider.bounds.max.y);
        Vector2 topRightCorner = new Vector2(m_boundsCollider.bounds.max.x, m_boundsCollider.bounds.max.y);
        Gizmos.DrawLine(bottomLeftCorner,bottomRightCorner);
        Gizmos.DrawLine(bottomRightCorner,topRightCorner);
        Gizmos.DrawLine(topRightCorner,topLeftCorner);
        Gizmos.DrawLine(topLeftCorner,bottomLeftCorner);
    }

}
