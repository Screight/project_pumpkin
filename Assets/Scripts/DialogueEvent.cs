using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    protected int m_nextEvent;
    protected bool[] m_eventTriggered;
    protected int m_dialogueCount = 0;
    protected int m_eventCount = 0;
    protected bool m_isEventActive = false;
    [SerializeField] protected Dialogue m_dialogue;
    bool m_isInteractive;

    protected virtual void Awake() {
        m_eventTriggered = new bool[m_dialogue.GetNumberOfSentences()];
        for(int i = 0; i < m_eventTriggered.Length; i++){
            m_eventTriggered[i] = false;
        }
    }

    public void StartDialogueEvent(){
        DialogueManager.Instance.StartConversation(this);
        m_isEventActive = true;
    }

    protected virtual bool Update()
    {
        if(!m_isEventActive){ return false;}
        m_dialogueCount = DialogueManager.Instance.SentenceCount;
        if(m_dialogueCount != (int)m_nextEvent ||  m_dialogueCount < 0 || m_eventTriggered[(int)m_eventCount]){ return false; }
        return true;
    }

    public virtual void FinishDialogueEvent(){
        m_isEventActive = false;
        Player.Instance.State = PLAYER_STATE.IDLE;
    }

    public Dialogue Dialogue{ get { return m_dialogue;} }

}