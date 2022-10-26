using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellyWallCheck : MonoBehaviour
{
    SkeletonController skeletonScript;

    void Start()
    {
        skeletonScript = GetComponentInParent<SkeletonController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor") {
             skeletonScript.IsHittingWall = true;
        }
    }
}