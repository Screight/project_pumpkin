using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager m_instance;

    [SerializeField] Camera camera;

    int screenWidth;
    int screenHeight;
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
        if (m_instance == null) { m_instance = this; }
        else { Destroy(this.gameObject); }

        screenWidth = Screen.width;
        screenHeight = Screen.height;
        topLeftCorner = camera.ScreenToWorldPoint(new Vector2(0, screenHeight));
        bottomLeftCorner = camera.ScreenToWorldPoint(new Vector2(0, 0));
        topRighttCorner = camera.ScreenToWorldPoint(new Vector2(screenWidth, screenHeight));
        bottomRightCorner = camera.ScreenToWorldPoint(new Vector2(screenWidth, 0));

        leftLimit = topLeftCorner.x;
        topLimit = topLeftCorner.y;
        rightLimit = bottomRightCorner.x;
        bottomLimit = bottomRightCorner.y;
    }

    private void Update() { UpdateCameraPosition(); }

    public float LeftLimit { get { return leftLimit; } }
    public float RightLimit { get { return rightLimit; } }
    public float TopLimit { get { return topLimit; } }
    public float BottomLimit { get { return bottomLimit; } }
    public float Width { get { return rightLimit - leftLimit; } }
    public float Height { get { return topLimit - bottomLimit; } }

    void UpdateCameraPosition()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        topLeftCorner = camera.ScreenToWorldPoint(new Vector2(0, screenHeight));
        bottomLeftCorner = camera.ScreenToWorldPoint(new Vector2(0, 0));
        topRighttCorner = camera.ScreenToWorldPoint(new Vector2(screenWidth, screenHeight));
        bottomRightCorner = camera.ScreenToWorldPoint(new Vector2(screenWidth, 0));

        leftLimit = topLeftCorner.x;
        topLimit = topLeftCorner.y;
        rightLimit = bottomRightCorner.x;
        bottomLimit = bottomRightCorner.y;
    }

    static public CameraManager Instance { get { return m_instance; } }
}