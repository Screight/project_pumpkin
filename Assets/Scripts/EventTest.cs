using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : InteractiveItem
{
    DialogueEvent m_dialogueScript;
    private void Start() {
        m_dialogueScript = GetComponent<DialogueEvent>();
    }
    protected override void HandleInteraction()
    {
        base.HandleInteraction();
        m_dialogueScript.StartDialogueEvent();
    }
}
