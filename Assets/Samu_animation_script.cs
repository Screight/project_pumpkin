using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samu_animation_script : MonoBehaviour
{

    [SerializeField] GameObject innerRing;
    [SerializeField] GameObject outerRing;
        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        innerRing.transform.rotation = new Quaternion(0, 0, 0.5f, 0);
    }
}
