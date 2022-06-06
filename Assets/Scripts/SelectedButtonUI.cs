using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedButtonUI : MonoBehaviour, ISelectHandler
{
    [SerializeField] GameObject m_flames;
    [SerializeField] OptionBox m_options;

    private void Start() {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject){
            m_flames.transform.position = new Vector3(transform.position.x, transform.position.y + 16, transform.position.z);
        }
    }

    public void OnSelect(BaseEventData p_eventData)
    {
        SoundManager.Instance.PlayOnce(AudioClipName.BUTTONSWITCH);
        m_flames.transform.position = new Vector3(transform.position.x, transform.position.y + 16, transform.position.z);
    }
}
