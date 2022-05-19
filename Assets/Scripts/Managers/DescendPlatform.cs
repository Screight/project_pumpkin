using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendPlatform : MonoBehaviour
{
    private PlatformEffector2D m_effector;
    bool m_isPlayerInPlatform = false;
    bool m_isInThisPlatform = false;
    Collider2D m_collider;

    void Start() { m_effector = GetComponent<PlatformEffector2D>();
    Physics2D.IgnoreLayerCollision(7,10, true);
    Physics2D.IgnoreLayerCollision(1,10, true);
    Physics2D.IgnoreLayerCollision(1,9,true);
    Physics2D.IgnoreLayerCollision(1,10,true);
    }

    private void Awake() {
        m_collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && Player.Instance.ObjectGroundedTo == "platform" && !m_isPlayerInPlatform && Player.Instance.IsGrounded && m_isInThisPlatform) { 
            m_effector.rotationalOffset = 180;
            m_collider.isTrigger = true;
            m_collider.usedByEffector = false;
            //m_effector.useColliderMask = 
            Player.Instance.Speed = new Vector2(0,-20);
        }
    }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag == "groundCheck"){
            m_isInThisPlatform = true;
            Player.Instance.InsidePlatform = true;
        }
        if(p_collider.tag != "platformController"){ return ;}
        m_isPlayerInPlatform = true;
        
    }

    private void OnTriggerExit2D(Collider2D p_collider) {
        if(p_collider.tag == "groundCheck"){m_isInThisPlatform = false;}
        if(p_collider.tag != "platformController" && !Physics2D.GetIgnoreLayerCollision(7,9)){ return ;}
        m_effector.rotationalOffset = 0;
        m_isPlayerInPlatform = false;
        m_collider.isTrigger = false;
        m_collider.usedByEffector = true;
        Player.Instance.InsidePlatform = false;
    }

}