using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedButtonUI : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] GameObject m_flames;

    public void OnSelect(BaseEventData p_eventData){
        m_flames.transform.position = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_flames.transform.position = transform.position;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

}
