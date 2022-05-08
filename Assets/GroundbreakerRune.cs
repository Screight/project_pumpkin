using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundbreakerRune : InteractiveItem
{
    private bool m_hasBeenPicked = false;

    protected override void HandleInteraction()
    {
        if(m_hasBeenPicked){ return; }
        GameManager.Instance.SetIsSkillAvailable(SKILLS.GROUNDBREAKER, true);
        base.HandleInteraction();
        GameManager.Instance.PlayerAttackDamage++;
        SoundManager.Instance.PlayOnce(AudioClipName.ITEM_PICK_UP);
        m_hasBeenPicked = true;
    }
}
