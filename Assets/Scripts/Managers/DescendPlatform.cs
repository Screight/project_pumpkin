using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    Timer descendTimer;
    bool restoreOffset;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();

        descendTimer = gameObject.AddComponent<Timer>();
        descendTimer.Duration = 2;
        restoreOffset = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            descendTimer.Run();
            effector.surfaceArc = -180.0f;
            restoreOffset = true;
        }

        if (descendTimer.IsFinished && restoreOffset == true)
        {
            effector.surfaceArc = 270.0f;
            restoreOffset = false;
        }
    }
}
