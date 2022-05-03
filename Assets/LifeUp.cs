using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUp : InteractiveItem
{
    protected override void HandleInteraction()
    {
        base.HandleInteraction();
        GameManager.Instance.GainExtraHeart();
        SoundManager.Instance.PlayOnce(AudioClipName.ITEM_PICK_UP);
        Destroy(this.gameObject);

    }
}
