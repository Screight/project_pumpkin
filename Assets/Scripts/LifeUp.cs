using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUp : InteractiveItem
{
    private bool m_hasBeenPicked = false;
    protected override void HandleInteraction()
    {
        if (!m_hasBeenPicked) {
            base.HandleInteraction();
            GameManager.Instance.GainExtraHeart();
            SoundManager.Instance.PlayOnce(AudioClipName.ITEM_PICK_UP);
            m_hasBeenPicked = true;
        }
    }
}