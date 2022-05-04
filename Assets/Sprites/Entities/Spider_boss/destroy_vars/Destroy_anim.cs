using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destroy_anim : AnimatedCharacter
{
    public GameObject leg2;
    public GameObject leg1;
    public GameObject drill;
    private Rigidbody2D leg2_rg;
    private Rigidbody2D leg1_rg;
    private Rigidbody2D drill_rg;
    private float vel = 2000;
    
    UnityEvent m_loseLeg;

    
    
    protected override void Awake() {
        base.Awake();
        m_loseLeg = new UnityEvent();
    }

    void Start()
    {
        leg2_rg = leg2.GetComponent<Rigidbody2D>();
        leg1_rg = leg1.GetComponent<Rigidbody2D>();
        drill_rg = drill.GetComponent<Rigidbody2D>();


        leg2_rg.AddForce(new Vector2(vel, vel*5f));
        leg1_rg.AddForce(new Vector2(vel, vel*5f));
    }

    public void AddListenerLoseLeg(UnityAction p_function){
        m_loseLeg.AddListener(p_function);
    }

    public void RemoveListenerLoseLeg(UnityAction p_function){
        m_loseLeg.RemoveListener(p_function);
    }

    public void EndDestroyLeg(){
        m_loseLeg.Invoke();
        Debug.Log("LEG DESTROYED EVENT TRIGGERED");
    }

}
