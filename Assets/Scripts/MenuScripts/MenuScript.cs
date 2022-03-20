using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Canvas menu;

    private void Awake()
    {
        menu = GetComponent<Canvas>();
        menu.enabled = true;
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
    }

    public void Pause() { Time.timeScale = 0; }
}