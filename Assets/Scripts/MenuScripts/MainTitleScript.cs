using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainTitleScript : MonoBehaviour
{
    public MainMenu main;
    public Canvas menu;
    public GameObject[] options;
   
    private int option_counter = 0;
    public GameObject FireHUD;
    public GameObject logoImg;
    public EventSystem EvSys;

    public AudioSource menuBeep;
    private int last_option;
    private bool changed_option = false;

    private void Awake()
    {
        EvSys = EventSystem.current;
    }
    private void Start()
    {
        EvSys.SetSelectedGameObject(EvSys.firstSelectedGameObject);

        FireHUD.transform.position = new Vector2(FireHUD.transform.position.x, EvSys.firstSelectedGameObject.transform.position.y);   
    }

    void Update()
    {

        last_option = option_counter;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            option_counter += 1;
            if (option_counter >= options.Length) { option_counter = 0; }
            changed_option = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            option_counter -= 1;
            if (option_counter <= -1) { option_counter = options.Length - 1; }
            changed_option = true;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter)|| Input.GetKeyDown(KeyCode.Space))
        {
            switch (option_counter)
            {
                default: break;
                case 0:
                    { PlayGame(); }
                    break;
                case 1:
                    {
                        main.setActiveMenuOptions();
                        logoImg.SetActive(false);
                        FireHUD.SetActive(false);
                    }
                    break;
            }
        }

        if (last_option != option_counter)
        {
            FireHUD.transform.position = new Vector2(FireHUD.transform.position.x, options[option_counter].transform.position.y);
            menuBeep.Play();
            changed_option = false;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Back()
    {
        main.setActiveMenuMain();
        logoImg.SetActive(true);
        FireHUD.SetActive(true);
    }
    public void QuitGame()
    {
        Debug.Log("Game Closed!");
        Application.Quit();
    }
}