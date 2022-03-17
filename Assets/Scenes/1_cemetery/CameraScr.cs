using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScr : MonoBehaviour
{

    public Transform player_tr;
    private Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = new Vector3 (player_tr.position.x, player_tr.position.y, -10) ;
    }
}
