using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : InteractiveItem
{
    DialogueEvent m_dialogue;

    private void Awake()
    {
        m_dialogue = GetComponent<DialogueEvent>();
    }

    protected override void HandleInteraction()
    {
         

        base.HandleInteraction();
        m_dialogue.StartDialogueEvent();

    }
}
