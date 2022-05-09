using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundbreakerRune : InteractiveItem
{
    private bool m_hasBeenPicked = false;
    [SerializeField] DialogueEvent m_dialogue;

    protected override void HandleInteraction()
    {
        if(m_hasBeenPicked){ return; }
        base.HandleInteraction();
        SoundManager.Instance.PlayOnce(AudioClipName.ITEM_PICK_UP);
        m_hasBeenPicked = true;
        m_dialogue.addListenerToDialogueFinish(unlockGroundBreaker);
    }

    private void unlockGroundBreaker() {
        GameManager.Instance.SetIsSkillAvailable(SKILLS.GROUNDBREAKER, true);
        m_dialogue.removeListenerToDialogueFinish(unlockGroundBreaker);    
    }
}