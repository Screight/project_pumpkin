using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class roarScript: MonoBehaviour
{
    public Transform a;
    public Transform b;

    void Update()
    {
        a.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        if (a.localScale.x > 10f)
        {
            a.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        } 
        b.localScale += new Vector3(0.01f, 0.01f, 0.01f);
        if (b.localScale.x > 5f)
        {
            b.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
