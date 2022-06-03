using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samu_eye_script : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Samu_animation_script par_scr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Damage(){
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag != "fireball"){ return; }
        Damage();
    }

}
