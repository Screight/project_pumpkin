using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Player playerScript;
    Rigidbody2D playerRigidBody;

    bool m_hasAnyGroundBeenFound;

    void Start()
    {
        playerScript = GetComponentInParent<Player>();
        playerRigidBody = GetComponentInParent<Rigidbody2D>();

        m_hasAnyGroundBeenFound = false;
    }

    private void Update()
    {
        m_hasAnyGroundBeenFound = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            playerScript.IsGrounded = true;
            playerRigidBody.gravityScale = playerScript.Gravity1 / Physics2D.gravity.y;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            if(playerScript.State != PLAYER_STATE.MOVE && playerScript.State != PLAYER_STATE.CAST && playerScript.State != PLAYER_STATE.DASH && playerScript.State != PLAYER_STATE.ATTACK)
            {
                playerScript.SetPlayerState(PLAYER_STATE.IDLE);
                playerScript.SetPlayerAnimation(PLAYER_ANIMATION.IDLE);
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            playerScript.IsGrounded = false;
            playerRigidBody.gravityScale = playerScript.Gravity2 / Physics2D.gravity.y;
            
        }
    }

}


