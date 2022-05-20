using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedButtonUIPauseMenu : MonoBehaviour, ISelectHandler
{

    public void OnSelect(BaseEventData p_eventData)
    {
        SoundManager.Instance.PlayOnce(AudioClipName.BUTTONSWITCH);
    }

}
