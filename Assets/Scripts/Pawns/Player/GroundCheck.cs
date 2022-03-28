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
        bool canPlayerTransitionFromCurrentStateToGrounded = m_playerScript.State != PLAYER_STATE.BOOST && m_playerScript.State != PLAYER_STATE.JUMP && m_playerScript.State != PLAYER_STATE.DASH;
        bool isObjectASurface = colliderTag == "floor" || colliderTag == "platform";

        if(isObjectASurface && canPlayerTransitionFromCurrentStateToGrounded){

            if(m_playerScript.State == PLAYER_STATE.FALL && m_playerScript.State == PLAYER_STATE.ATTACK) {
                m_playerScript.State = PLAYER_STATE.LAND;
                m_playerScript.ChangeAnimationState(PLAYER_ANIMATION.LAND);
            }
            m_playerScript.SetToGrounded(colliderTag);
        }
        bool isPlayerJumping = m_playerScript.State == PLAYER_STATE.JUMP || m_playerScript.State == PLAYER_STATE.BOOST;

        if(colliderTag == "platform" && isPlayerJumping){
            m_playerScript.HandleOneWayPlatforms();
        }
    }

}
