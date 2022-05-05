using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUp : InteractiveItem
{
    private bool m_hasBeenPicked = false;
    private SpriteRenderer m_spriteRenderer;
    protected override void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void HandleInteraction()
    {
        if (!m_hasBeenPicked) {
            base.HandleInteraction();
            GameManager.Instance.GainExtraHeart();
            SoundManager.Instance.PlayOnce(AudioClipName.ITEM_PICK_UP);
            m_hasBeenPicked = true;
            m_spriteRenderer.enabled = false;
        }
    }
}