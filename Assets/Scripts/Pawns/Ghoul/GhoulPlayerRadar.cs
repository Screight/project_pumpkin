using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulPlayerRadar : MonoBehaviour
{
    Ghoul ghoulScript;
    void Start() { ghoulScript = GetComponentInParent<Ghoul>(); }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { ghoulScript.IsPlayerNear = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { ghoulScript.IsPlayerNear = false; }
    }
}