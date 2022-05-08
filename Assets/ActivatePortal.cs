using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePortal : MonoBehaviour
{
    [SerializeField] Portal m_portal;
    
    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag == "Player") { return; }
        m_portal.OpenPortal();
    }

    private void OnTriggerExit2D(Collider2D p_collider) {
        if(p_collider.tag == "Player") { return; }
        m_portal.ClosePortal();
    }

}
