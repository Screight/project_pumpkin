using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulPlayerAtRange : MonoBehaviour
{
    Ghoul ghoulScript;
    void Start() { ghoulScript = GetComponentInParent<Ghoul>(); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { ghoulScript.IsPlayerAtRange = true; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { ghoulScript.IsPlayerAtRange = false; }
    }
}