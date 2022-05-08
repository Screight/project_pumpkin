using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject m_interface;
    bool m_isGamePaused = false;
    bool m_isGameInCutscene = false;
    public void ReturnToMainMenu()
    {
        Game.SceneManager.Instance.LoadScene((int)SCENE.MAIN_MENU);
    }

    private void Update()
    {
        if (InputManager.Instance.PauseButtonPressed && !GameManager.Instance.IsGamePaused && !m_isGameInCutscene) { 
            Pause(); 
            }
        else if (InputManager.Instance.PauseButtonPressed && m_isGamePaused) { 
            UnPause(); }
    }

    public void Pause()
    {
        MenuManager.Instance.GoTo(1);
        Time.timeScale = 0;
        m_isGamePaused = true;
        GameManager.Instance.SetGameToPaused(true, true);
        m_interface.SetActive(false);
    }

    public void UnPause()
    {
        MenuManager.Instance.GoTo(0);
        Time.timeScale = 1;
        m_isGamePaused = false;
        GameManager.Instance.SetGameToPaused(false, false);
        m_interface.SetActive(true);
    }

    public bool IsCutScenePlaying { set { m_isGameInCutscene = value; } }
}