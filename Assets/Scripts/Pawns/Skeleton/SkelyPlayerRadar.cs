using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelyPlayerRadar : MonoBehaviour
{
    SkeletonController skeletonScript;
    Rigidbody2D skeletonRigidBody;
    void Start()
    {
        skeletonScript = GetComponentInParent<SkeletonController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { skeletonScript.IsPlayerInVisionRange = true; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { skeletonScript.IsPlayerInVisionRange = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { skeletonScript.IsPlayerInVisionRange = false; }
    }
}