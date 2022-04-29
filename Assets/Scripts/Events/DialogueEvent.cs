using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueEvent : MonoBehaviour
{
    protected int m_nextEvent;
    protected bool[] m_eventTriggered;
    protected int m_dialogueCount = 0;
    protected int m_eventCount = 0;
    protected bool m_isEventActive = false;

    [SerializeField] protected Dialogue m_dialogue;
    [SerializeField] protected PlayableDirector m_cutscene;
    [SerializeField] protected Timeline m_cutSceneStart;

    protected virtual void Awake()
    {
        m_cutscene = GetComponentInParent<PlayableDirector>();
        m_eventTriggered = new bool[m_dialogue.GetNumberOfSentences()];
        for (int i = 0; i < m_eventTriggered.Length; i++) { m_eventTriggered[i] = false; }
    }

    public void StartDialogueEvent()
    {
        //Debug.Log("Dialogo empezado");
        Player.Instance.SetPlayerToScripted();
        if (m_cutscene != null) { m_cutscene.Pause(); }
        DialogueManager.Instance.StartConversation(this);
        m_isEventActive = true;
    }

    protected virtual bool Update()
    {
        if (!m_isEventActive) { return false; }
        m_dialogueCount = DialogueManager.Instance.SentenceCount;
        if (m_dialogueCount != m_nextEvent || m_dialogueCount < 0 || m_eventTriggered[m_eventCount]) { return false; }
        return true;
    }

    public virtual void FinishDialogueEvent()
    {
        //Debug.Log("Dialogo terminado");
        m_isEventActive = false;
        if (m_cutscene != null) { m_cutscene.Resume(); }

        if ((m_cutscene != null && m_cutscene.state != PlayState.Playing) || (m_cutscene != null && m_cutSceneStart != null && m_cutSceneStart.PlayerCanMove) || m_cutscene == null) { Player.Instance.StopScripting(); }
        else { Player.Instance.SetPlayerToScripted(); }
        Player.Instance.State = PLAYER_STATE.IDLE;        
    }

    public Dialogue Dialogue{ get { return m_dialogue;} }
}