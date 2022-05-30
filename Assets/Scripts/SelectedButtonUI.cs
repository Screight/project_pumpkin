using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedButtonUI : MonoBehaviour, ISelectHandler
{
    [SerializeField] GameObject m_flames;
    [SerializeField] OptionBox m_options;

    public void OnSelect(BaseEventData p_eventData)
    {
        SoundManager.Instance.PlayOnce(AudioClipName.BUTTONSWITCH);
        m_flames.transform.position = transform.position;
    }
}
