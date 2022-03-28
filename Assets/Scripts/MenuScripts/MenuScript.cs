using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Canvas menu;
    public Canvas HUD;
    public List<Button> buttons = new List<Button>();

    private void Awake()
    {
        menu = GetComponent<Canvas>();
        if (SceneManager.GetActiveScene().buildIndex == 0) { menu.enabled = true; }
        else { menu.enabled = false; }
    }

    public void Update()
    {
        if (Input.GetKeyDown("escape") && SceneManager.GetActiveScene().buildIndex != 0) { menu.enabled = true; }

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
        if (SceneManager.GetActiveScene().buildIndex != 0) { SceneManager.LoadScene(0); }
        else
        {
            Debug.Log("Game Closed!");
            Application.Quit();
        }
    }

    public void Continue()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            OnDisable();
            menu.enabled = false;
            Time.timeScale = 1;
            HUD.enabled = true;
        }
    }

    public void Pause()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            OnEnable();
            Time.timeScale = 0; HUD.enabled = false;
        }
    }

    void OnEnable() { foreach (var button in buttons) { button.interactable = true; } }
    void OnDisable() { foreach (var button in buttons) { button.interactable = false; } }
}