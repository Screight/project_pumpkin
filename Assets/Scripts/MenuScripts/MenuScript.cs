using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Canvas menu;
    public Canvas HUD;
    private void Awake()
    {
        menu = GetComponent<Canvas>();
        if (SceneManager.GetActiveScene().buildIndex == 0) { menu.enabled = true; }
        else { menu.enabled = false; }
    }

    public void Update()
    {
        if (Input.GetKeyDown("escape")) { menu.enabled = !menu.enabled; }

        if (menu.enabled) { Pause(); } 
        else { Continue(); }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        menu.enabled = false;
        HUD.enabled = true;
    }
    public void Exit()
    {
        Debug.Log("Game Closed!");
        Application.Quit();
    }

    public void Continue()
    {
        menu.enabled = false;
        Time.timeScale = 1;
        HUD.enabled = true;
    }

    public void Pause() { Time.timeScale = 0; HUD.enabled = false; }
}