using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleOptionsMainMenu : MonoBehaviour
{

    [SerializeField] MenuManager m_menuManager;

    private void Update()
    {
        if (m_menuManager.GetCurrentMenuIndex() == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SoundManager.Instance.PlayOnce(AudioClipName.BUTTONCLICKED);
                MenuManager.Instance.GoBack();
            }
        }
    }
}
