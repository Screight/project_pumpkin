using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : InteractiveItem
{
    private bool m_hasBeenPicked = false;

    protected override void HandleInteraction()
    {
        if(m_hasBeenPicked){ return; }
        base.HandleInteraction();
        GameManager.Instance.AddAttackDamage();
        SoundManager.Instance.PlayOnce(AudioClipName.ITEM_PICK_UP);
        m_hasBeenPicked = true;
    }
}
