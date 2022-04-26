using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager m_instance;
    static public CameraManager Instance { get { return m_instance; } }
    Camera m_camera;

    [SerializeField] Vector2Int m_referenceResolution;
    [SerializeField] int m_pixelsPerUnit;

    [SerializeField] GameObject m_mainScene;
    [SerializeField] CameraMovement m_cameraMovementScript;

    int m_screenWidth;
    int m_screenHeight;
    Vector2 topLeftCorner;
    Vector2 topRighttCorner;
    Vector2 bottomLeftCorner;
    Vector2 bottomRightCorner;
    float leftLimit;
    float rightLimit;
    float topLimit;
    float bottomLimit;

    private void Awake()
    {
        m_camera = Camera.main;
        m_referenceResolution = new Vector2Int(320, 180);
        m_pixelsPerUnit = 1;

        if (m_instance == null) { m_instance = this; }
        else { Destroy(gameObject); }

        m_screenWidth = m_referenceResolution.x / m_pixelsPerUnit;
        m_screenHeight = m_referenceResolution.y / m_pixelsPerUnit;
        
        InitializeCamera();
        UpdateCameraPosition();
    }

    private void Update() 
    { 
        UpdateCameraPosition(); 
    }

    void UpdateCameraPosition()
    {
        topLeftCorner       = new Vector2(transform.position.x - m_screenWidth/2, transform.position.y + m_screenHeight/2);
        bottomLeftCorner    = new Vector2(transform.position.x - m_screenWidth/2, transform.position.y - m_screenHeight/2);
        topRighttCorner     = new Vector2(transform.position.x + m_screenWidth/2, transform.position.y + m_screenHeight/2);
        bottomRightCorner   = new Vector2(transform.position.x + m_screenWidth/2, transform.position.y - m_screenHeight/2);

        leftLimit   = topLeftCorner.x;
        topLimit    = topLeftCorner.y;
        rightLimit  = bottomRightCorner.x;
        bottomLimit = bottomRightCorner.y;
    }

    void InitializeCamera()
    {
        float cameraAngle = m_camera.fieldOfView;
        float PI = Mathf.PI;
        float distanceToMainScene = (m_referenceResolution.y / 2) / (Mathf.Tan(PI * cameraAngle / 360));

        m_mainScene.transform.position = new Vector3(m_mainScene.transform.position.x, m_mainScene.transform.position.y, distanceToMainScene);
    }

    public void ClampCameraToTarget()
    {
        m_cameraMovementScript.ClampCamera();
    }

    public void SetCameraToPlayerPosition(){
        m_cameraMovementScript.SetCameraToPlayerPosition();
    }

    public void SetCameraToStatic()
    {
        m_cameraMovementScript.IsCameraStatic = true;
    }

    public void SetCameraToNormal()
    {
        m_cameraMovementScript.IsCameraStatic = false;
        m_cameraMovementScript.IsCameraClamped = false;
    }

    public float LeftLimit      { get { return leftLimit; } }
    public float RightLimit     { get { return rightLimit; } }
    public float TopLimit       { get { return topLimit; } }
    public float BottomLimit    { get { return bottomLimit; } }
    public float Width          { get { return m_screenWidth; } }
    public float Height         { get { return m_screenHeight; } }
    public Vector3 Position     { get{ return transform.position;}}
    public float MainSceneDepth { get { return m_mainScene.transform.position.z - transform.position.z;}}
}