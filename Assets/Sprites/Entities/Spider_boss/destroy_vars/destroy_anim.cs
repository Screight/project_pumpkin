using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_anim : MonoBehaviour
{
    public GameObject leg2;
    public GameObject leg1;
    public GameObject drill;
    private Rigidbody2D leg2_rg;
    private Rigidbody2D leg1_rg;
    private Rigidbody2D drill_rg;
    private float vel = 2000;
    
    
    // Start is called before the first frame update
    void Start()
    {
        leg2_rg = leg2.GetComponent<Rigidbody2D>();
        leg1_rg = leg1.GetComponent<Rigidbody2D>();
        drill_rg = drill.GetComponent<Rigidbody2D>();


        leg2_rg.AddForce(new Vector2(vel, vel*5f));
        leg1_rg.AddForce(new Vector2(vel, vel*5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
