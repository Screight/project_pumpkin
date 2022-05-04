using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class roarScript: MonoBehaviour
{
    public Transform a;
    public Transform b;

    void Update()
    {
        a.localScale += new Vector3(0.025f, 0.025f, 0.025f);
        if (a.localScale.x > 6f)
        {
            a.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        } 
        b.localScale += new Vector3(0.015f, 0.015f, 0.015f);
        if (b.localScale.x > 3f)
        {
            b.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
