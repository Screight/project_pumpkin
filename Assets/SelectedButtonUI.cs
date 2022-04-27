using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedButtonUI : MonoBehaviour
{
    [SerializeField] GameObject[] m_buttons;
    int m_currentButton = 0;
    private void OnEnable() {
        m_currentButton = 0;
        transform.position = m_buttons[m_currentButton].transform.position;
    }

    private void Update() {
        if(InputManager.Instance.UpButtonPressed){
            if(m_currentButton > 0){
                m_currentButton--;
            }
            else{ m_currentButton = m_buttons.Length - 1;}
            transform.position = m_buttons[m_currentButton].transform.position;
        }
        else if(InputManager.Instance.DownButtonPressed){
            if(m_currentButton < m_buttons.Length - 1){
                m_currentButton++;
            }
            else{ m_currentButton = 0;}
            transform.position = m_buttons[m_currentButton].transform.position;
        }
    }
}
