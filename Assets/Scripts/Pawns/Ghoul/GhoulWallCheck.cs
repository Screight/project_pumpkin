using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulWallCheck : MonoBehaviour
{
    Ghoul ghoulScript;
    void Start() { ghoulScript = GetComponentInParent<Ghoul>(); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor")) { ghoulScript.IsHittingWall = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor")) { ghoulScript.IsHittingWall = false; }
    }

}