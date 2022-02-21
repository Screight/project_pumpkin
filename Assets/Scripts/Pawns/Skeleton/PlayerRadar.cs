using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadar : MonoBehaviour
{
    Skeleton skeletonScript;
    Rigidbody2D skeletonRigidBody;
    void Start()
    {
        skeletonScript = GetComponentInParent<Skeleton>();
        skeletonRigidBody = GetComponentInParent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            skeletonScript.IsPlayerNear = true;
        }
        //else { skeletonScript.IsPlayerNear = false; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            skeletonScript.IsPlayerNear = false;
        }
    }
}
