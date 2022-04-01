using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellyWallCheck : MonoBehaviour
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
        if (collision.gameObject.tag == "floor") {
             skeletonScript.IsHittingWall = true; }
    }
}