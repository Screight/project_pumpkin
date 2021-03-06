using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class DialogueEvent : MonoBehaviour
{
    protected int m_nextEvent;
    protected bool[] m_eventTriggered;
    protected int m_dialogueCount = 0;
    protected int m_eventCount = 0;
    protected bool m_isEventActive = false;
    private UnityEvent m_dialogueFinish;

    [SerializeField] protected Dialogue m_dialogue;
    [SerializeField] protected PlayableDirector m_cutscene;
    [SerializeField] protected Timeline m_cutSceneStart;
    private double m_cutSceneCurrentTime;

    protected virtual void Awake()
    {
        m_dialogueFinish= new UnityEvent();

        m_cutscene = GetComponentInParent<PlayableDirector>();
        m_eventTriggered = new bool[m_dialogue.GetNumberOfSentences()];
        for (int i = 0; i < m_eventTriggered.Length; i++) { m_eventTriggered[i] = false; }
    }

    public void StartDialogueEvent()
    {
        //Debug.Log("Dialogo empezado");
        Player.Instance.SetPlayerToScripted();
        
        if (m_cutscene != null) { m_cutSceneCurrentTime = m_cutscene.time; m_cutscene.Stop(); }

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
        if (m_cutscene != null) { m_cutscene.Play(); m_cutscene.time = m_cutSceneCurrentTime; }

        if ((m_cutscene != null && m_cutscene.state != PlayState.Playing) || (m_cutscene != null && m_cutSceneStart != null && m_cutSceneStart.PlayerCanMove) || m_cutscene == null) { Player.Instance.StopScripting(); }
        else { Player.Instance.SetPlayerToScripted(); }
        m_dialogueFinish.Invoke();
    }
    public void addListenerToDialogueFinish(UnityAction function) { m_dialogueFinish.AddListener(function); }
    public void removeListenerToDialogueFinish(UnityAction function) { m_dialogueFinish.RemoveListener(function); }

    public Dialogue Dialogue{ get { return m_dialogue;} }
}