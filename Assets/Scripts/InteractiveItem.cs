using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveItem : MonoBehaviour
{
    [SerializeField] GameObject m_icon;
    protected bool m_canPlayerInteract = true;
    protected bool m_isPlayerInside = false;

    [SerializeField] bool m_isOneUseOnly = false;
    [SerializeField] bool m_isAutomatic = false;
    bool m_hasBeenUsed = false;

    protected virtual bool Update() {
        if(m_isOneUseOnly && m_hasBeenUsed) {
            if(m_icon != null) {m_icon.SetActive(false);}
            }
        if((m_isOneUseOnly && !m_hasBeenUsed) || !m_isOneUseOnly){
            if(m_isPlayerInside && m_canPlayerInteract){
                if(m_isAutomatic && !m_hasBeenUsed || !m_isAutomatic && InputManager.Instance.InteractButtonPressed)
                HandleInteraction();
            }
        }
        return true;
    }

    protected virtual void Awake() {
        if(!m_isAutomatic){
            m_icon.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag != "Player") { return ;}
        m_isPlayerInside = true;
        if(m_canPlayerInteract){
            if(!m_isAutomatic){
            m_icon.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D p_collider) {
        if(p_collider.tag != "Player") { return ;}
        if(!m_isAutomatic){
            m_icon.SetActive(false);
        }
        m_isPlayerInside = false;
        if(m_isAutomatic && !m_isOneUseOnly) { m_hasBeenUsed = false; }
    }

    protected virtual void HandleInteraction(){
        m_hasBeenUsed = true;
    }
    protected void SetIconTo(bool p_state){
        if(!m_isAutomatic){
            m_icon.SetActive(p_state);
        }
        else Debug.LogWarning("THERE IS NO ICON ATTACHED.");
    }
}