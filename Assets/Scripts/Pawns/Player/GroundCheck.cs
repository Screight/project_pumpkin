using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Player m_playerScript;

    private void Awake() {
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerStay2D(Collider2D p_collider) {
        string colliderTag = p_collider.tag;
        if(Player.Instance.State == PLAYER_STATE.DEATH && colliderTag == "floor"){
            Player.Instance.StopPlayerMovement();
        }
        bool canPlayerTransitionFromCurrentStateToGrounded = m_playerScript.State != PLAYER_STATE.BOOST && m_playerScript.State != PLAYER_STATE.JUMP && m_playerScript.State != PLAYER_STATE.DASH && m_playerScript.State != PLAYER_STATE.DEATH && m_playerScript.State != PLAYER_STATE.TALKING;
        bool isObjectASurface = colliderTag == "floor" || colliderTag == "platform" || colliderTag == "vine";

        if(isObjectASurface && canPlayerTransitionFromCurrentStateToGrounded){

            if(m_playerScript.State == PLAYER_STATE.FALL && m_playerScript.State == PLAYER_STATE.ATTACK) {
                m_playerScript.State = PLAYER_STATE.LAND;
                AnimationManager.Instance.PlayAnimation(m_playerScript, ANIMATION.PLAYER_LAND);
            }
            if(m_playerScript.State == PLAYER_STATE.GROUNDBREAKER){
                Physics2D.IgnoreLayerCollision(6,7,false);
            }
            m_playerScript.SetToGrounded(colliderTag);
        }
        bool isPlayerJumping = m_playerScript.State == PLAYER_STATE.JUMP || m_playerScript.State == PLAYER_STATE.BOOST;

        if(colliderTag == "platform" && isPlayerJumping){
            m_playerScript.HandleOneWayPlatforms();
        }
    }

}
