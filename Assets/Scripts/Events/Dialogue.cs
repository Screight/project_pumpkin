using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    [SerializeField] Sprite[] m_icon;
    [SerializeField] string[] m_name;
    [TextArea(3,10)]
    [SerializeField] string[] m_sentences;
    [SerializeField] int[] m_characterTalkingSentence;

    public string[] GetSentences(){
        return m_sentences;
    }

    public string[] GetNames() { return m_name; }
    public Sprite[] GetIcons() { return m_icon; }
    public int GetNumberOfSentences() { return m_sentences.Length; }

    public DialogueLine[] GetDialogueLines(){
        DialogueLine[] dialogueLines = new  DialogueLine[m_sentences.Length];
        for(int i = 0; i < m_sentences.Length; i++){
            dialogueLines[i] = new DialogueLine(m_icon[m_characterTalkingSentence[i]], m_name[m_characterTalkingSentence[i]], m_sentences[i]);
        }
        return dialogueLines;
    }
    
}
