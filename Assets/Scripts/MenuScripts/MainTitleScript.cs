using System.Collections;
using System.Collections.Generic;
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
    public EventSystem EvSys;

    public AudioSource menuBeep;
    // Start is called before the first frame update

   
    void Start()
    {
      EvSys.SetSelectedGameObject(  EvSys.firstSelectedGameObject);

       FireHUD.transform.position = new Vector2(FireHUD.transform.position.x, options[0].transform.position.y);


        
    }

    // Update is called once per frame
    void Update()
    {
        int last_option = option_counter;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            option_counter += 1;
            if (option_counter == options.Length)
            {
                option_counter = 0;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            option_counter -= 1;
            if(option_counter == -1) { option_counter = options.Length - 1; }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (option_counter)
            {
                case 0:
                    PlayGame();
                    break;
                case 1:
                    main.setActiveMenuOptions();
                    FireHUD.SetActive(false);
                    break;
                case 2:
                    break;
                
                default:
                    break;
            } ;
        }

            Debug.Log(EvSys.currentSelectedGameObject);
        if (last_option != option_counter) {
            FireHUD.transform.position = new Vector2(FireHUD.transform.position.x, options[option_counter].transform.position.y);
            EvSys.SetSelectedGameObject(options[option_counter]);
            menuBeep.Play();
        }

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        menu.enabled = false;
    }

}
