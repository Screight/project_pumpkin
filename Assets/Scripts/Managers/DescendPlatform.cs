using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendPlatform : MonoBehaviour
{
    private PlatformEffector2D m_effector;
    bool m_isPlayerInPlatform = false;

    void Start() { m_effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && m_isPlayerInPlatform) { 
            m_effector.rotationalOffset = 180;
            Player.Instance.AddImpulse(new Vector2(0, -30));
        }
        else if (!m_isPlayerInPlatform) { 
            m_effector.rotationalOffset = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D p_collider) {
        if(p_collider.gameObject.tag == "Player"){
            m_isPlayerInPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D p_collider) {
        if(p_collider.gameObject.tag == "Player"){
            m_isPlayerInPlatform = false;
        }
    }

}