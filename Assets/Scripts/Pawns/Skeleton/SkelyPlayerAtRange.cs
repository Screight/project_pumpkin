using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelyPlayerAtRange : MonoBehaviour
{
    SkeletonController SkeletonScript;
    void Start()
    {
        SkeletonScript = GetComponentInParent<SkeletonController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { SkeletonScript.IsPlayerInAttackRange = true; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { SkeletonScript.IsPlayerInAttackRange = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { SkeletonScript.IsPlayerInAttackRange = false; }
    }
}