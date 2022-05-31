using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye_parent_sc : MonoBehaviour
{

    [SerializeField] Samu_animation_script scr;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            scr.player_ref = collision.gameObject.GetComponent<Player>().gameObject;
            scr.playerInRange = true; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { scr.playerInRange = false; }
    }
}
