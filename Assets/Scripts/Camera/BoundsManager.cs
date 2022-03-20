using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{
    static BoundsManager m_instance;

    CameraMovement m_cameraMovementScript;
    [SerializeField] GameObject m_mainBounds;
    CameraBound m_mainBoundsScript;

    List<Limit>[] m_limitsList;

    Limit[] m_currentLimits;

    private async void Awake() {

        if(m_instance == null){ m_instance = this; }
        else { Destroy(this.gameObject); }

        m_cameraMovementScript = Camera.main.GetComponent<CameraMovement>();
        m_mainBoundsScript = m_mainBounds.GetComponent<CameraBound>();

        m_limitsList = new List<Limit>[(int)DIRECTION.LAST_NO_USE];

        for(int i = 0; i < (int)DIRECTION.LAST_NO_USE; i++){
            m_limitsList[i] = new List<Limit>();
        }

        m_currentLimits = new Limit[(int)DIRECTION.LAST_NO_USE];
    }

    // Start is called before the first frame update
    void Start()
    {
        m_cameraMovementScript.TopLimit = m_mainBoundsScript.TopLimit.position;
        m_limitsList[(int)DIRECTION.TOP].Add(m_mainBoundsScript.TopLimit);

        m_cameraMovementScript.BottomLimit = m_mainBoundsScript.BottomLimit.position;
        m_limitsList[(int)DIRECTION.BOTTOM].Add(m_mainBoundsScript.BottomLimit);

        m_cameraMovementScript.LeftLimit = m_mainBoundsScript.LeftLimit.position;
        m_limitsList[(int)DIRECTION.LEFT].Add(m_mainBoundsScript.LeftLimit);

        m_cameraMovementScript.RightLimit = m_mainBoundsScript.RightLimit.position;
        m_limitsList[(int)DIRECTION.RIGHT].Add(m_mainBoundsScript.RightLimit);
    }

    void SetLimitsToMainBounds(){
        m_currentLimits[(int)DIRECTION.TOP] = m_mainBoundsScript.TopLimit;
        m_currentLimits[(int)DIRECTION.BOTTOM] = m_mainBoundsScript.BottomLimit;
        m_currentLimits[(int)DIRECTION.LEFT] = m_mainBoundsScript.LeftLimit;
        m_currentLimits[(int)DIRECTION.RIGHT] = m_mainBoundsScript.RightLimit;
    }

    public void UpdateBounds(){

        SetLimitsToMainBounds();

        for(int i = 0; i < (int)DIRECTION.LAST_NO_USE; i++){
            for(int j = 0; j < m_limitsList[i].Count; j++){
                if(m_limitsList[i][j].layerIndex >= m_currentLimits[i].layerIndex){
                    
                    float limit = m_limitsList[i][j].position;

                    switch(i){
                        default: break;
                        case (int)DIRECTION.TOP:
                        if(limit < m_currentLimits[i].position){ m_currentLimits[i] = m_limitsList[i][j];}
                        break;
                        case (int)DIRECTION.BOTTOM:
                        if(limit > m_currentLimits[i].position){ m_currentLimits[i] = m_limitsList[i][j];}
                        break;
                        case (int)DIRECTION.LEFT:
                        if(limit > m_currentLimits[i].position){ m_currentLimits[i] = m_limitsList[i][j];}
                        break;
                        case (int)DIRECTION.RIGHT:
                        if(limit < m_currentLimits[i].position){ m_currentLimits[i] = m_limitsList[i][j];}
                        break;
                    }
                }
            }
        }
        m_cameraMovementScript.TopLimit = m_currentLimits[(int)DIRECTION.TOP].position;
        m_cameraMovementScript.BottomLimit = m_currentLimits[(int)DIRECTION.BOTTOM].position;
        m_cameraMovementScript.LeftLimit = m_currentLimits[(int)DIRECTION.LEFT].position;
        m_cameraMovementScript.RightLimit = m_currentLimits[(int)DIRECTION.RIGHT].position;
    }

// Delete all the limits with an specific ID
    public void DeleteLimits(uint p_cameraBoundID){
        for(int i = 0; i < (int)DIRECTION.LAST_NO_USE; i++){
            for(int j = 0; j < m_limitsList[i].Count; j++){
                if( m_limitsList[i][j].cameraBoundID == p_cameraBoundID){ m_limitsList[i].RemoveAt(j);}
                // TODO: there should be only 1 limit per ID so stop searching for that specific direction once one is found
            }
        }
    }

    public void AddLimit(Limit p_limit){
        m_limitsList[(int)p_limit.direction].Add(p_limit);
        // TODO: check if there is an already existing limit for that id and if there is dont add a new one
    }

    static public BoundsManager Instance{
        get { return m_instance; }
        private set {; }
    }

}
