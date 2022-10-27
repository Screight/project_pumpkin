using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTrigger : InteractiveItem
{
    [SerializeField] bool m_isHorizontal = true;
    [SerializeField] bool m_isPositive = true;
    [SerializeField] Transform m_finalPosition;
    [SerializeField] Transicion m_transicionScript;
    [SerializeField] bool m_changeCheckpointToFinalPosition = false;
    [SerializeField] ZONE m_destinationZone = ZONE.FOREST;
    [SerializeField] bool m_lookRight = false;

    protected override void Awake()
    {
        base.Awake();
        m_transicionScript = FindObjectOfType<Transicion>();
    }

    protected override void HandleInteraction()
    {
        base.HandleInteraction();

        Player.Instance.SetPlayerToScripted();
        m_transicionScript.AddListenerToEndOfTransition(ReturnPlayerToNormal);
        GameManager.Instance.IsGamePaused = true;
        Player.Instance.SetGravityScaleTo0();
        m_transicionScript.FadeIn();
        m_transicionScript.AddListenerToEndOfFadeIn(TransportPlayerToPosition);
        if (m_changeCheckpointToFinalPosition)
        {
            CheckpointsManager.Instance.SetMusicTo(m_destinationZone);
            CheckpointsManager.Instance.SetGlobalCheckPoint(m_finalPosition);
        }
        
        GameManager.Instance.CurrentZone = m_destinationZone;
        //CHANGE BGM
        if (m_destinationZone==ZONE.FOREST) { SoundManager.Instance.PlayBackground(BACKGROUND_CLIP.FORESTOFSOULS); }
        else if (m_destinationZone==ZONE.MINE) { SoundManager.Instance.PlayBackground(BACKGROUND_CLIP.ABANDONEDMINE); }
    }

    void ReturnPlayerToNormal()
    {
        Player.Instance.SetGravityScaleToFall();
        if (m_isHorizontal)
        {
            int direction = 1;
            if (!m_isPositive) { direction = -1; }
            Player.Instance.ScriptWalk(direction, 0.2f);
        }
        else { Player.Instance.StopScripting(); }
        
        m_transicionScript.RemoveListenerToEndOfTransition(ReturnPlayerToNormal);
        GameManager.Instance.IsGamePaused = false;
    }

    void TransportPlayerToPosition()
    {
        Player.Instance.transform.position = new Vector3(m_finalPosition.position.x, m_finalPosition.position.y, Player.Instance.transform.position.z);
        if(m_lookRight){ Player.Instance.FacePlayerToRight(); }
    }   
}