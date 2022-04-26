using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : InteractiveItem
{
    [SerializeField] Transform m_respawnPoint;
    protected override void HandleInteraction()
    {
        base.HandleInteraction();
        CheckpointsManager.Instance.SetGlobalCheckPoint(m_respawnPoint);
        Debug.Log("CHECKPOINT");
    }
}
