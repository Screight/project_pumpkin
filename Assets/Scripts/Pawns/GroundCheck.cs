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
        if (collision.gameObject.tag == "floor")
        {
            Debug.Log("Grounded");
            playerScript.IsGrounded = true;
            playerRigidBody.gravityScale = playerScript.Gravity1 / Physics2D.gravity.y;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            playerScript.SetPlayerState(PLAYER_STATE.IDLE);
            playerScript.SetPlayerAnimation(PLAYER_ANIMATION.IDLE);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            Debug.Log("Not Grounded");
            playerScript.IsGrounded = false;
            playerRigidBody.gravityScale = playerScript.Gravity2 / Physics2D.gravity.y;
            playerScript.SetPlayerState(PLAYER_STATE.FALL);
            playerScript.SetPlayerAnimation(PLAYER_ANIMATION.FALL);
        }
    }

}


