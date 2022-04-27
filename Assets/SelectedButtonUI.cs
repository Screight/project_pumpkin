using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedButtonUI : MonoBehaviour, ISelectHandler
{
    [SerializeField] GameObject m_flames;

    public void OnSelect(BaseEventData p_eventData){
        m_flames.transform.position = transform.position;
    }

}
