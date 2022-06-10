using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnlock : InteractiveItem
{
    protected override void HandleInteraction()
    {
        base.HandleInteraction();
        GameManager.Instance.UnlockMap();
    }
}
