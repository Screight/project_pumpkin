using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossTrigger : InteractiveItem
{
    [SerializeField] Transform m_respawnPosition;
    [SerializeField] Door[] m_doors = new Door[2];
    SpiderBoss m_spiderBoss;

    protected override void Awake() {
        base.Awake();
        m_spiderBoss = GameObject.FindObjectOfType<SpiderBoss>();
    }

    protected override void HandleInteraction(){
        base.HandleInteraction();
        for(int i = 0; i < m_doors.Length; i++){
            m_doors[i].CloseDoor();
        }
        GameManager.Instance.IsPlayerInSpiderBossFight = true;
        m_spiderBoss.StartBossFight();
        SoundManager.Instance.StopBackground();
        CheckpointsManager.Instance.SetGlobalCheckPoint(m_respawnPosition);
    }

    public void HandlePlayerDeath(){
        m_spiderBoss.Reset();
        for(int i = 0; i < m_doors.Length; i++){
            m_doors[i].OpenDoor();
        }
        GameManager.Instance.IsPlayerInSpiderBossFight = false;
        ResetState();
        // TP PLAYER TO LAST SAVE POINT
    }

}
