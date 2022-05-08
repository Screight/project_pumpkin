using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct DialogueLine
{
    public Sprite icon;
    public string name;
    public string sentence;

    public DialogueLine(Sprite p_icon, string p_name, string p_sentence)
    {
        icon = p_icon;
        name = p_name;
        sentence = p_sentence;
    }
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject m_textBox;
    [SerializeField] TMP_Text m_nameUI;
    [SerializeField] Image m_iconUI;
    [SerializeField] TMP_Text m_textUI;

    Queue<DialogueLine> m_dialogue;

    bool m_conversationStarted = false;
    bool m_canGoToNextSentence = true;
    int m_sentenceCount = -1;
    bool m_isPrintingText = false;
    int m_currentNumberOfCharactersPrinted = 0;
    string m_currentSentece;

    Timer m_writeEffectTimer;
    [SerializeField] float m_timeBetweenCharacters = 0.1f;
    [SerializeField] float m_timeBetweenCharactersFast = 0.2f;

    DialogueEvent m_currentDialogueEvent;

    static DialogueManager m_instance;
    public static DialogueManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            Initialize();
        }
        else { Destroy(this); }
    }

    private void Update()
    {
        if (m_currentDialogueEvent == null) { return; }
        if (m_isPrintingText && InputManager.Instance.InteractButtonPressed)
        {
            m_writeEffectTimer.Duration = m_timeBetweenCharactersFast;
        }
        else if (m_isPrintingText && InputManager.Instance.InteractButtonReleased)
        {
            m_writeEffectTimer.Duration = m_timeBetweenCharacters;
        }
        if (m_textBox.activeSelf != false)
        {
            if (m_isPrintingText && m_writeEffectTimer.IsFinished)
            {
                if (m_currentNumberOfCharactersPrinted <= m_currentSentece.Length)
                {
                    //Write letter
                    m_textUI.text = m_currentSentece.Substring(0, m_currentNumberOfCharactersPrinted);
                    if (Random.Range(1, 3) == 1) { SoundManager.Instance.PlayOnce(AudioClipName.DIALOGUECLIC1); }
                    else { SoundManager.Instance.PlayOnce(AudioClipName.DIALOGUECLIC2); }

                    for (int i = m_currentNumberOfCharactersPrinted; i < m_currentSentece.Length; i++)
                    {
                        m_textUI.text += " ";
                    }
                    m_currentNumberOfCharactersPrinted++;
                    m_writeEffectTimer.Run();
                }
                else
                {
                    m_currentNumberOfCharactersPrinted = 0;
                    m_isPrintingText = false;
                    m_canGoToNextSentence = true;
                }
            }
        }

        if (InputManager.Instance.InteractButtonPressed && m_conversationStarted && m_canGoToNextSentence && !m_isPrintingText)
        {
            ShowNextSentence();
        }
    }

    private void Initialize()
    {
        m_textUI.text = "";
        m_dialogue = new Queue<DialogueLine>();
        m_textBox.SetActive(false);
        m_writeEffectTimer = gameObject.AddComponent<Timer>();
        m_writeEffectTimer.Duration = m_timeBetweenCharacters;
    }

    public void StartConversation(DialogueEvent p_dialogueEvent)
    {
        m_currentDialogueEvent = p_dialogueEvent;
        m_textBox.gameObject.SetActive(true);
        m_textBox.SetActive(true);       
        m_isPrintingText = true;
        m_writeEffectTimer.Duration = m_timeBetweenCharacters;
        m_currentNumberOfCharactersPrinted = 0;
        m_textUI.text = "";

        DialogueLine[] dialogueLines = p_dialogueEvent.Dialogue.GetDialogueLines();
        m_dialogue.Clear();
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            m_dialogue.Enqueue(dialogueLines[i]);
        }

        SetUpNextSentence();
        m_conversationStarted = true;
        Player.Instance.StartTalking();
    }

    void SetUpNextSentence()
    {
        DialogueLine dialogueLine = m_dialogue.Dequeue();
        m_currentSentece = dialogueLine.sentence;
        m_iconUI.sprite = dialogueLine.icon;
        m_nameUI.text = dialogueLine.name;
    }

    public void ShowNextSentence()
    {
        m_writeEffectTimer.Duration = m_timeBetweenCharacters;
        m_textUI.text = "";
        if (m_dialogue.Count > 0)
        {
            SetUpNextSentence();
            if (m_currentSentece.Length != 0) { m_isPrintingText = true; }
            m_sentenceCount++;
        }
        else
        {
            m_textBox.SetActive(false);
            m_conversationStarted = false;
            Player.Instance.State = PLAYER_STATE.IDLE;
            m_sentenceCount = -1;
            m_currentDialogueEvent.FinishDialogueEvent();
            m_currentDialogueEvent = null;
        }
    }

    public bool CanGoToNextSentence { set { m_canGoToNextSentence = value; }}
    public int SentenceCount { get { return m_sentenceCount; } }

    public void HideDialogue() { m_textBox.gameObject.SetActive(false); }
    public void ShowDialogue() { m_textBox.gameObject.SetActive(true); }
    public bool IsDialogueFinished { get { return m_conversationStarted; }}
}