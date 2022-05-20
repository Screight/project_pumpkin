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
      float sin = Mathf.Cos(Time.time/3)/5 ;
        Debug.Log(sin);
        innerRing.transform.Rotate( new Vector3( 0,0,sin));
        outerRing.transform.Rotate(new Vector3(0, 0, - sin));
    }
}