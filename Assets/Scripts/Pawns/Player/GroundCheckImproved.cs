using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckImproved : MonoBehaviour
{
    Player m_playerScript;

    private void Awake() {
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerStay2D(Collider2D p_collider) {
        string colliderTag = p_collider.tag;
        bool canTransitionFromCurrentStateToGrounded = m_playerScript.State != PLAYER_STATE.BOOST && m_playerScript.State != PLAYER_STATE.JUMP && m_playerScript.State != PLAYER_STATE.DASH;
        bool isObjectASurface = colliderTag == "floor" || colliderTag == "enemy" || colliderTag == "platform";

        if(isObjectASurface && canTransitionFromCurrentStateToGrounded){
            m_playerScript.SetToGrounded(colliderTag);
        }

    }

}
