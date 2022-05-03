using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendPlatform : MonoBehaviour
{
    private PlatformEffector2D m_effector;
    bool m_isPlayerInPlatform = false;

    void Start() { m_effector = GetComponent<PlatformEffector2D>();
    Physics2D.IgnoreLayerCollision(7,10, true);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && Player.Instance.ObjectGroundedTo == "platform") { 
            m_effector.rotationalOffset = 180;
            if(!m_isPlayerInPlatform){
                Player.Instance.AddImpulse(new Vector2(0, -1));
                m_isPlayerInPlatform = true;
            }
            
        }
        else{ 
            m_effector.rotationalOffset = 0;
            m_isPlayerInPlatform = false;
        }
    }

}