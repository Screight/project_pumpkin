using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject[] Menus;
    private bool[] menusActive;
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < Menus.Length; i++)
        {
            Menus[i].SetActive(false);
            
        }
        Menus[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && Menus[1].activeSelf == true)
        {
            setActiveMenuMain();
        }
    }

    public void setActiveMenuOptions()
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            Menus[i].SetActive(false);

        }
        Menus[1].SetActive(true);
    }
    public void setActiveMenuMain()
    {
        for (int i = 0; i < Menus.Length; i++)
        {
            Menus[i].SetActive(false);

        }
        Menus[0].SetActive(true);
    }
}
