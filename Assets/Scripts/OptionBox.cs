using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class OptionBox : MonoBehaviour
{

    [SerializeField] GameObject m_defaultButton;
    [SerializeField] LoadingScreen m_script;

    public void OpenBox(){
        EventSystem.current.SetSelectedGameObject(m_defaultButton);
        MenuManager.Instance.CanGoBack = false;
    }

    public void StartNewGame(){
        Game.SceneManager.Instance.LoadGame = false;
        m_script.LoadScene(SCENE.GAME);
    }

    public void Continue(){
        m_script.LoadScene(SCENE.GAME);
    }

    private void Update() {
        if(InputManager.Instance.CancelButtonPressed){
            MenuManager.Instance.CanGoBack = true;
            MenuManager.Instance.FocusOnDefaultButton();
            InputManager.Instance.ClearAllInput();
            InputManager.Instance.PauseInputFor1Frame();
            this.gameObject.SetActive(false);
        }
    }

}
