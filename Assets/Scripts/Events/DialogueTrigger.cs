using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : InteractiveItem
{
    private DialogueEvent m_dialogue;
    public Timeline m_CutSceneStart;

    protected override void Awake()
    {
        base.Awake();
        m_dialogue = GetComponent<DialogueEvent>();
    }
    protected override void Update()
    {
        if (m_CutSceneStart != null)
        {
            if (m_CutSceneStart.HasStartedplaying && m_CutSceneStart.m_cutSceneStartsWithDialog)
            {
                m_dialogue.StartDialogueEvent();
                gameObject.SetActive(false);
            }
        }

        base.Update();
    }

    protected override void HandleInteraction()
    {
        if (m_CutSceneStart == null || !m_CutSceneStart.m_cutSceneStartsWithDialog)
        {
            base.HandleInteraction();
            m_dialogue.StartDialogueEvent();
        }
    }
}