using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samu_animation_script : MonoBehaviour
{

    [SerializeField] GameObject innerRing;
    [SerializeField] GameObject outerRing;
    [SerializeField] GameObject mainCircle;
    [SerializeField] GameObject subCircle1;
    [SerializeField] GameObject subCircle2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      float sin = Mathf.Cos(Time.time/2)*1.5f ;
        Debug.Log(sin);
        innerRing.transform.Rotate( new Vector3( 0,0,sin));
        outerRing.transform.Rotate(new Vector3(0, 0, - sin));
        mainCircle.transform.Rotate(new Vector3(0, 0, 0.5f));
        subCircle1.transform.Rotate(new Vector3(0, 0, -1f));
        subCircle2.transform.Rotate(new Vector3(0, 0, -1f));

    }
}