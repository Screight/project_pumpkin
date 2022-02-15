using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Player playerScript;
    Rigidbody2D playerRigidBody;

    void Start()
    {
        playerScript = GetComponentInParent<Player>();
        playerRigidBody = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ContactPoint2D[] contactPoints = new ContactPoint2D[2];
        if (collision.gameObject.tag == "floor")
        {
            Debug.Log("Grounded");
            playerScript.IsGrounded = true;
            playerRigidBody.gravityScale = playerScript.Gravity2 / Physics2D.gravity.y;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            playerScript.SetPlayerState(PLAYER_STATE.IDLE);
        }
    }
}
