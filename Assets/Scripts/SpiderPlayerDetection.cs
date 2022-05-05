using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPlayerDetection : MonoBehaviour
{
    Spider m_spider;
    private void Awake() {
        m_spider = GetComponentInParent<Spider>();
    }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(m_spider.CanHatch() && p_collider.tag == "Player"){
            m_spider.InitializeEclosion();
        }
    }
}
