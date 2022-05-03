using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendPlatform : MonoBehaviour
{
    private Rigidbody2D m_rb2D;

    void Start() { m_rb2D = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) { 
            Physics.IgnoreLayerCollision(7,9,true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow)) { 
            Physics.IgnoreLayerCollision(7,9,false); }
    }
}