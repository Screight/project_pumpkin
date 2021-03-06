using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Player m_playerScript;


    private void Start() {
        m_playerScript = Player.Instance;
    }

    private void OnTriggerStay2D(Collider2D p_collider) {
        string colliderTag = p_collider.tag;
        if(Player.Instance.State == PLAYER_STATE.DEATH && colliderTag == "floor"){
            Player.Instance.StopPlayerMovement();
        }

        bool canPlayerTransitionFromCurrentStateToGrounded = m_playerScript.State != PLAYER_STATE.BOOST && m_playerScript.State != PLAYER_STATE.JUMP && m_playerScript.State != PLAYER_STATE.DASH && m_playerScript.State != PLAYER_STATE.DEATH && m_playerScript.State != PLAYER_STATE.TALKING && m_playerScript.State != PLAYER_STATE.HURT;
        bool isObjectASurface = colliderTag == "floor" || colliderTag == "platform" || colliderTag == "vine";

        if(isObjectASurface && canPlayerTransitionFromCurrentStateToGrounded){

            if(m_playerScript.State == PLAYER_STATE.FALL && m_playerScript.State != PLAYER_STATE.ATTACK) {
                m_playerScript.State = PLAYER_STATE.LAND;
                AnimationManager.Instance.PlayAnimation(m_playerScript, ANIMATION.PLAYER_LAND, false);
            }else if (m_playerScript.State != PLAYER_STATE.LAND && m_playerScript.State != PLAYER_STATE.MOVE){
                AnimationManager.Instance.PlayAnimation(m_playerScript, ANIMATION.PLAYER_IDLE, false);
            }
            if(m_playerScript.State == PLAYER_STATE.GROUNDBREAKER){
                Physics2D.IgnoreLayerCollision(6,7,false);
            }
            m_playerScript.SetToGrounded(colliderTag);
        }
        bool isPlayerJumping = m_playerScript.State == PLAYER_STATE.JUMP || m_playerScript.State == PLAYER_STATE.BOOST;
    }

    private void OnTriggerExit2D(Collider2D other) {
        Player.Instance.IsGrounded = false;
    }
}
