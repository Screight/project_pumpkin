using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuDamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag != "Player") { return ;}
        Player.Instance.HandleHostileCollision(Vector2.zero, Vector2.zero, 1, 2,1);
    }  
    
}
