using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossTrigger : InteractiveItem
{
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
            m_spiderBoss.StartBossFight();
        }
    }

}
