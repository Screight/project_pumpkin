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

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Idle si est�s en suelo || DENTRO de plataforma NO saltando/cayendo
        if ((collision.gameObject.tag == "floor" || collision.gameObject.tag == "enemy" || (collision.gameObject.tag == "platform" && playerScript.State != PLAYER_STATE.JUMP && playerScript.State != PLAYER_STATE.BOOST)) && playerScript.State != PLAYER_STATE.DASH)
        {
            playerScript.IsGrounded = true;
            playerScript.ObjectGroundedTo = collision.gameObject.tag;
            playerRigidBody.gravityScale = playerScript.Gravity1 / Physics2D.gravity.y;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            playerScript.HasUsedDash = false;

            if(playerScript.State != PLAYER_STATE.MOVE && playerScript.State != PLAYER_STATE.CAST && playerScript.State != PLAYER_STATE.DASH && playerScript.State != PLAYER_STATE.ATTACK)
            {
                if(playerScript.State == PLAYER_STATE.FALL)
                {
                    playerScript.State = PLAYER_STATE.LAND;
                    playerScript.ChangeAnimationState(PLAYER_ANIMATION.LAND);
                }
                else if (playerScript.State != PLAYER_STATE.LAND)
                {
                    playerScript.State = PLAYER_STATE.IDLE;
                    playerScript.ChangeAnimationState(PLAYER_ANIMATION.IDLE);
                }
                
            }           
        }
        //Subir si est�s DENTRO de plataforma saltando
        if (collision.gameObject.tag == "platform" && (playerScript.State == PLAYER_STATE.JUMP || playerScript.State == PLAYER_STATE.BOOST))
        {
            playerScript.IsGrounded = false;

            playerRigidBody.gravityScale = playerScript.Gravity1 / Physics2D.gravity.y;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 40);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "floor" || collision.gameObject.tag == "platform"))
        {
            playerScript.IsGrounded = false;

            if (playerScript.State != PLAYER_STATE.DASH)
            {
                playerRigidBody.gravityScale = playerScript.Gravity2 / Physics2D.gravity.y;
            }           
        }
    }

    
}


