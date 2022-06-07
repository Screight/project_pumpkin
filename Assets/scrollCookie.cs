using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]

public class scrollCookie : MonoBehaviour
{
    Light2D lg;
    [SerializeField] Texture2D text;

    // Start is called before the first frame update
    
void Start()
    {
        lg = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lg.transform.Rotate(new Vector3(0, 0, Time.deltaTime*25));
        float sin = (Mathf.Sin(Time.time*5f));

        lg.intensity = (sin+4f)/1.5f;    
    }
}
