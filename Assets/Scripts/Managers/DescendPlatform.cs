using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendPlatform : MonoBehaviour
{
    private Rigidbody2D m_rb2D;

    void Start() { m_rb2D = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) { m_rb2D.simulated = false; }
        else { m_rb2D.simulated = true; }
    }
}
