using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skellyGroundCheck : MonoBehaviour
{
    Skeleton skeletonScript;
    Rigidbody2D skeletonRigidBody;

    void Start()
    {
        skeletonScript = GetComponentInParent<Skeleton>();
        skeletonRigidBody = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor") { 
            skeletonScript.IsGrounded = false; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "floor" || collision.gameObject.tag == "platform") && skeletonScript.State != SKELETON_STATE.DIE)
        {
            skeletonScript.IsGrounded = true;
            skeletonRigidBody.velocity = Vector2.zero;
            //Physics2D.IgnoreLayerCollision(6, 7, false);
            skeletonScript.State = SKELETON_STATE.MOVE;
        }
        if(skeletonScript.State == SKELETON_STATE.DIE){
            skeletonRigidBody.velocity = Vector2.zero;
        }
    }
}