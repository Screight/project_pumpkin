using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

struct Menu{
    public GameObject gameObject;
    public int ID;
    public GameObject initialSelectecButton;

    public Menu(GameObject p_gameObject, int p_menuID, GameObject p_initialSelectedButton){
        gameObject = p_gameObject;
        ID = p_menuID;
        initialSelectecButton = p_initialSelectedButton;
    }
}

public class MenuManager : MonoBehaviour
{

    static MenuManager m_instance;

    static public MenuManager Instance
    {
        get { return m_instance; }
        private set { }
    }
    [SerializeField] GameObject[] m_menuReferences;
    [SerializeField] GameObject[] m_initialSelectedButton;
    Menu[] m_menu;
    Stack<Menu> m_menuStack;
    [SerializeField] int m_initialMenu;
    int m_currentMenu;
    bool[] m_isMenuInStack;
    bool m_isMenuActive = true;

    private void Awake() {
        if(m_instance == null){m_instance = this; }
        else { Destroy(this.gameObject) ;}
        m_menuStack = new Stack<Menu>();
        m_menu = new Menu[m_menuReferences.Length];
        m_isMenuInStack = new bool[m_menu.Length];
        InitializeMenu();
        
    }
    
    private void Update() {
        if(m_isMenuActive){
            if(InputManager.Instance.CancelButtonPressed){
                if(Game.SceneManager.Instance.Scene == SCENE.GAME && m_currentMenu == 1){
                    GameObject.FindObjectOfType<PauseMenu>().UnPause();
                }
                GoBack();
            }
        }
    }

    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(null);
        if(m_menu[m_initialMenu].initialSelectecButton == null) { return ;}
        EventSystem.current.SetSelectedGameObject(m_menu[m_initialMenu].initialSelectecButton);
        m_menu[m_initialMenu].initialSelectecButton.GetComponent<Button>().OnSelect(null);
    }

    void InitializeMenu(){
        m_currentMenu = m_initialMenu;

        for(int menuIndex = 0; menuIndex < m_menu.Length; menuIndex++){
            Menu menu = new Menu(m_menuReferences[menuIndex], menuIndex, m_initialSelectedButton[menuIndex]);
            m_menu[menuIndex] = menu;
            // DOUBLE CHECK TO AVOID PROBLEM WITH ACTIVE/INACTIVE GAMEOBJECTS AS LONG AS THE REFERENCES ARE SET PROPERLY
            bool isInitialMenu = false;
            if(menuIndex != (int)m_currentMenu){
                isInitialMenu = false;
            }
            else {
                isInitialMenu = true;
                m_menuStack.Push(menu);
            }
            // THE ONLY MENU IN THE STACK IS THE INITIAL MENU
            m_isMenuInStack[menuIndex] = isInitialMenu;
            m_menu[menuIndex].gameObject.SetActive(isInitialMenu);
        } 
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_menu[m_initialMenu].initialSelectecButton);

    }

    public void GoTo(int p_menu){
        // SINCE THERE IS ONLY 1 INSTANCE OF EVERY MENU AVAILABLE, IF IT'S ALREADY IN THE STACK IT CANNOT BE CHARGED/TRANSITION TO AGAIN
        // THE ONLY WAY TO GO BACK TO THE MENU WOULD BE GOING BACK TILL WE REACH IT AGAIN
        // TODO - IF WE WANT TO MOVE BACKWARS TO AN ALREADY EXISTING MENU IN THE STACK WE WOULD NEED TO POP ALL THE MENUS 
        if(m_isMenuInStack[(int)p_menu]) {
            int currentMenuID = m_menuStack.Peek().ID;
             while(currentMenuID != p_menu){
                 m_menuStack.Peek().gameObject.SetActive(false);
                 m_menuStack.Pop();
                 m_isMenuInStack[currentMenuID] = false;
                 currentMenuID = m_menuStack.Peek().ID;
                 m_menuStack.Peek().gameObject.SetActive(true);
             }
        }
        else
        {
            //m_isMenuInStack[m_menuStack.Peek().ID] = false;
            m_menuStack.Peek().gameObject.SetActive(false);

            m_menuStack.Push(m_menu[p_menu]);
            m_menuStack.Peek().gameObject.SetActive(true);
            m_isMenuInStack[p_menu] = true;
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_menuStack.Peek().initialSelectecButton);
        m_currentMenu = m_menuStack.Peek().ID;
    }

    public void GoBack(){
        if(m_menuStack.Count == 1) { return ;}
        // SET CURRENT MENU TO INACTIVE
        m_menuStack.Peek().gameObject.SetActive(false);
        m_isMenuInStack[m_menuStack.Peek().ID] = false;
        m_menuStack.Pop();
        // SET LAST MENU TO ACTIVE
        m_menuStack.Peek().gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_menuStack.Peek().initialSelectecButton);
        m_currentMenu = m_menuStack.Peek().ID;
    }
    
    public int GetCurrentMenuIndex(){
        if(m_menuStack.Count <= 0){ return -1;}
        return m_menuStack.Peek().ID;
    }

}

