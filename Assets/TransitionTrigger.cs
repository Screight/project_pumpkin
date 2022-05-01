using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : InteractiveItem
{
    [SerializeField] bool m_isHorizontal = true;
    [SerializeField] bool m_isPositive = true;
    [SerializeField] Transform m_finalPosition;
    [SerializeField] Transicion m_transicionScript;
    protected override void HandleInteraction()
    {
        base.HandleInteraction();
        
        Player.Instance.SetPlayerToScripted();
        m_transicionScript.AddListenerToEndOfTransition(ReturnPlayerToNormal);
        Player.Instance.SetGravityScaleTo0();
        m_transicionScript.FadeIn();
        m_transicionScript.AddListenerToEndOfFadeIn(TransportPlayerToPosition);
    }

    void ReturnPlayerToNormal(){
        Player.Instance.SetGravityScaleToFall();
        if(m_isHorizontal){
            int direction = 1;
            if(!m_isPositive){ direction = -1;}
                Player.Instance.ScriptWalk(direction, 0.2f);
        }
        m_transicionScript.RemoveListenerToEndOfTransition(ReturnPlayerToNormal);
    }

    void TransportPlayerToPosition(){
        Player.Instance.transform.position = new Vector3(m_finalPosition.position.x, m_finalPosition.position.y, Player.Instance.transform.position.z);
    }
    
}
