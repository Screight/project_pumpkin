using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelyPlayerAtRange : MonoBehaviour
{
    Skeleton SkeletonScript;
    void Start()
    {
        SkeletonScript = GetComponentInParent<Skeleton>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { SkeletonScript.IsPlayerAtRange = true; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { SkeletonScript.IsPlayerAtRange = false; }
    }
}