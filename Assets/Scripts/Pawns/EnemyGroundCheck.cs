using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour {
    Skeleton skeletonScript;
    Rigidbody2D skeletonRigidBody;

    void Start() {
        skeletonScript = GetComponentInParent<Skeleton>();
        skeletonRigidBody = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "floor") {
            skeletonScript.IsGrounded = false;
        }
    }



}
