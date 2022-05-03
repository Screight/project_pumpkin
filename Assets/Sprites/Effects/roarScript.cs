using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class roarScript: MonoBehaviour
{
    public Transform a;
    public Transform b;

    void Update()
    {
        a.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        if (a.localScale.x > 10f)
        {
            a.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        } 
        b.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        if (b.localScale.x > 5f)
        {
            b.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
