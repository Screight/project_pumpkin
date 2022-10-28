using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skellyGroundCheck : MonoBehaviour
{
    SkeletonController skeletonScript;
    Rigidbody2D skeletonRigidBody;

    void Awake()
    {
        skeletonScript = GetComponentInParent<SkeletonController>();
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
            if(skeletonScript.Health > 0)
            {
                skeletonScript.ChangeState(skeletonScript.PatrolState);
            }
        }
        if(skeletonScript.State == SKELETON_STATE.DIE){
            skeletonRigidBody.velocity = Vector2.zero;
        }
    }
}