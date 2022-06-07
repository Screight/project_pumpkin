using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle_Rot : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = new Vector3(0, 0, Time.deltaTime);
        transform.Rotate(rot*30);

        
    }
}
