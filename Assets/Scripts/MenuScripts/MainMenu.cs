using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject[] Menus;

    void Start()
    {
        
        for (int i = 0; i < Menus.Length; i++)
        {
            Menus[i].SetActive(false);
            
        }
        Menus[0].SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && Menus[1].activeSelf)
        {
            setActiveMenuMain();
        }
    }

    public void setActiveMenuOptions()
    {
        for (int i = 0; i < Menus.Length; i++) { Menus[i].SetActive(false); }
        Menus[1].SetActive(true);
    }
    public void setActiveMenuMain()
    {
        for (int i = 0; i < Menus.Length; i++) { Menus[i].SetActive(false); }
        Menus[0].SetActive(true);
    }
}