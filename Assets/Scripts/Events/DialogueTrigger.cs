using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : InteractiveItem
{
    DialogueEvent m_dialogue;
    public Timeline m_CutSceneTrigger;

    protected override void Awake()
    {
        base.Awake();
        m_dialogue = GetComponent<DialogueEvent>();
    }
    protected override void Update()
    {
        if (m_CutSceneTrigger != null)
        {
            if (m_CutSceneTrigger.HasStartedplaying && m_CutSceneTrigger.m_cutSceneStartsWithDialog)
            {
                m_dialogue.StartDialogueEvent();
                gameObject.SetActive(false);
            }
        }

        base.Update();
    }

    protected override void HandleInteraction()
    {
        if (m_CutSceneTrigger == null || !m_CutSceneTrigger.m_cutSceneStartsWithDialog)
        {
            Debug.Log("entra");
            base.HandleInteraction();
            m_dialogue.StartDialogueEvent();
            gameObject.SetActive(false);
        }
    }
}