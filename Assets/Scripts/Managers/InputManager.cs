using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    enum ACTIONS { ATTACK, DASH, JUMP, SKILL_1, SKILL_2, SKILL_3, INTERACT, NUMBER_OF_ACTIONS };

    static InputManager m_instance;
    static public InputManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    //------------------KEYBOARD--------------------------
    KeyCode m_attackButton_k    = KeyCode.X;
    KeyCode m_dashButton_k      = KeyCode.C;
    KeyCode m_jumpButton_k      = KeyCode.Space;
    KeyCode m_skill1Button_k    = KeyCode.A;
    KeyCode m_skill2Button_k    = KeyCode.S;
    KeyCode m_skill3Button_k    = KeyCode.D;
    KeyCode m_interactButton_k = KeyCode.Space;


    //-------------------JOYSTICK-------------------------
    KeyCode m_attackButton_j = KeyCode.Joystick1Button2;
    KeyCode m_dashButton_j = KeyCode.Joystick1Button4;
    KeyCode m_jumpButton_j = KeyCode.Joystick1Button0;
    KeyCode m_skill1Button_j = KeyCode.Joystick1Button5;
    KeyCode m_skill2Button_j = KeyCode.Joystick1Button9;
    KeyCode m_skill3Button_j = KeyCode.Joystick1Button10;
    KeyCode m_interactButton_j = KeyCode.Joystick1Button0;

    bool[] m_buttonsPressed   = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];
    bool[] m_buttonsHold      = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];
    bool[] m_buttonsReleased  = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];

    float m_horizontalAxis;
    float m_verticalAxis;
    private void Awake()
    {
        if (m_instance == null) { m_instance = this; }
        else { Destroy(this.gameObject); }

        for (int i = 0; i < (int)ACTIONS.NUMBER_OF_ACTIONS; i++)
        {
            m_buttonsPressed[i] = false;
            m_buttonsHold[i] = false;
            m_buttonsReleased[i] = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < (int)ACTIONS.NUMBER_OF_ACTIONS; i++) {
            m_buttonsPressed[i] = false;
            m_buttonsHold[i] = false;
            m_buttonsReleased[i] = false;
        }

        m_horizontalAxis = 0;
        m_verticalAxis = 0;

        m_horizontalAxis = Input.GetAxis("Horizontal");
        m_verticalAxis = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(m_attackButton_k) || Input.GetKeyDown(m_attackButton_j))     { m_buttonsPressed[(int)ACTIONS.ATTACK]   = true; }
        if (Input.GetKeyDown(m_attackButton_k) || Input.GetKeyDown(m_attackButton_j))         { m_buttonsHold[(int)ACTIONS.ATTACK]      = true; }
        if (Input.GetKeyDown(m_attackButton_k) || Input.GetKeyDown(m_attackButton_j))       { m_buttonsReleased[(int)ACTIONS.ATTACK]  = true; }

        if (Input.GetKeyDown(m_dashButton_k) || Input.GetKeyDown(m_dashButton_j))       { m_buttonsPressed[(int)ACTIONS.DASH]     = true; }
        if (Input.GetKeyDown(m_dashButton_k) || Input.GetKeyDown(m_dashButton_j))           { m_buttonsHold[(int)ACTIONS.DASH]        = true; }
        if (Input.GetKeyDown(m_dashButton_k) || Input.GetKeyDown(m_dashButton_j))         { m_buttonsReleased[(int)ACTIONS.DASH]    = true; }

        if (Input.GetKeyDown(m_jumpButton_k) || Input.GetKeyDown(m_jumpButton_j))       { m_buttonsPressed[(int)ACTIONS.JUMP]     = true; }
        if (Input.GetKeyDown(m_jumpButton_k) || Input.GetKeyDown(m_jumpButton_j))           { m_buttonsHold[(int)ACTIONS.JUMP]        = true; }
        if (Input.GetKeyDown(m_jumpButton_k) || Input.GetKeyDown(m_jumpButton_j))         { m_buttonsReleased[(int)ACTIONS.JUMP]    = true; }

        if (Input.GetKeyDown(m_skill1Button_k) || Input.GetKeyDown(m_skill1Button_j))     { m_buttonsPressed[(int)ACTIONS.SKILL_1]  = true; }
        if (Input.GetKey(m_skill1Button_k) || Input.GetKey(m_skill1Button_k))         { m_buttonsHold[(int)ACTIONS.SKILL_1]     = true; }
        if (Input.GetKeyUp(m_skill1Button_k) || Input.GetKeyUp(m_skill1Button_j))       { m_buttonsReleased[(int)ACTIONS.SKILL_1] = true; }

        if (Input.GetKeyDown(m_skill2Button_k) || Input.GetKeyDown(m_skill2Button_j))     { m_buttonsPressed[(int)ACTIONS.SKILL_2]  = true; }
        if (Input.GetKey(m_skill2Button_k) || Input.GetKey(m_skill2Button_j))         { m_buttonsHold[(int)ACTIONS.SKILL_2]     = true; }
        if (Input.GetKeyUp(m_skill2Button_k) || Input.GetKeyUp(m_skill2Button_j))       { m_buttonsReleased[(int)ACTIONS.SKILL_2] = true; }

        if (Input.GetKeyDown(m_skill3Button_k) || Input.GetKeyDown(m_skill3Button_j))     { m_buttonsPressed[(int)ACTIONS.SKILL_3]  = true; }
        if (Input.GetKey(m_skill3Button_k) || Input.GetKey(m_skill3Button_j))         { m_buttonsHold[(int)ACTIONS.SKILL_3]     = true; }
        if (Input.GetKeyUp(m_skill3Button_k) || Input.GetKeyUp(m_skill3Button_j))       { m_buttonsReleased[(int)ACTIONS.SKILL_3] = true; }
        if (Input.GetKeyDown(m_skill3Button_k) || Input.GetKeyDown(m_skill3Button_j))     { m_buttonsPressed[(int)ACTIONS.SKILL_3]  = true; }

        if (Input.GetKey(m_interactButton_k) || Input.GetKey(m_interactButton_k))         { m_buttonsHold[(int)ACTIONS.SKILL_3]     = true; }
        if (Input.GetKeyUp(m_interactButton_k) || Input.GetKeyUp(m_interactButton_k))       { m_buttonsReleased[(int)ACTIONS.SKILL_3] = true; }
        if (Input.GetKeyDown(m_interactButton_k) || Input.GetKeyDown(m_interactButton_k))     { m_buttonsPressed[(int)ACTIONS.SKILL_3]  = true; }

        if (Input.GetKey(m_interactButton_j) || Input.GetKey(m_interactButton_j))         { m_buttonsHold[(int)ACTIONS.INTERACT]     = true; }
        if (Input.GetKeyUp(m_interactButton_j) || Input.GetKeyUp(m_interactButton_j))       { m_buttonsReleased[(int)ACTIONS.INTERACT] = true; }
        if (Input.GetKeyDown(m_interactButton_j) || Input.GetKeyDown(m_interactButton_j))     { m_buttonsPressed[(int)ACTIONS.INTERACT]  = true; }
    }

    public bool AttackButtonPressed     { get { return m_buttonsPressed[(int)ACTIONS.ATTACK]; } }
    public bool AttackButtonHold        { get { return m_buttonsHold[(int)ACTIONS.ATTACK]; } }
    public bool AttackbuttonReleased    { get { return m_buttonsReleased[(int)ACTIONS.ATTACK]; } }

    public bool JumpButtonPressed       { get { return m_buttonsPressed[(int)ACTIONS.JUMP]; } }
    public bool JumpButtonHold          { get { return m_buttonsHold[(int)ACTIONS.JUMP]; } }
    public bool JumpbuttonReleased      { get { return m_buttonsReleased[(int)ACTIONS.JUMP]; } }

    public bool DashButtonPressed       { get { return m_buttonsPressed[(int)ACTIONS.DASH]; } }
    public bool DashButtonHold          { get { return m_buttonsPressed[(int)ACTIONS.DASH]; } }
    public bool DashbuttonReleased      { get { return m_buttonsPressed[(int)ACTIONS.DASH]; } }

    public bool Skill1ButtonPressed     { get { return m_buttonsPressed[(int)ACTIONS.SKILL_1]; } }
    public bool Skill1ButtonHold        { get { return m_buttonsHold[(int)ACTIONS.SKILL_1]; } }
    public bool Skill1buttonReleased    { get { return m_buttonsReleased[(int)ACTIONS.SKILL_1]; } }

    public bool Skill2ButtonPressed     { get { return m_buttonsPressed[(int)ACTIONS.SKILL_2]; } }
    public bool Skill2ButtonHold        { get { return m_buttonsHold[(int)ACTIONS.SKILL_2]; } }
    public bool Skill2buttonReleased    { get { return m_buttonsReleased[(int)ACTIONS.SKILL_2]; } }

    public bool Skill3ButtonPressed     { get { return m_buttonsPressed[(int)ACTIONS.SKILL_3]; } }
    public bool Skill3ButtonHold        { get { return m_buttonsHold[(int)ACTIONS.SKILL_3]; } }
    public bool Skill3buttonReleased    { get { return m_buttonsReleased[(int)ACTIONS.SKILL_3]; } }

    public bool InteractButtonPressed     { get { return m_buttonsPressed[(int)ACTIONS.INTERACT]; } }
    public bool InteractButtonHold        { get { return m_buttonsHold[(int)ACTIONS.INTERACT]; } }
    public bool InteractButtonReleased    { get { return m_buttonsReleased[(int)ACTIONS.INTERACT]; } }

    public float HorizontalAxis{ get { return m_horizontalAxis;}}
    public float VerticalAxis{ get { return m_verticalAxis;}}

    public float HorizontalAxisFlat{
        get {
            if(m_horizontalAxis > 0){ return 1;}
            else if (m_horizontalAxis < 0) { return -1;}
            else return 0;
        }
    }
    public float VerticalAxisFlat{
        get {
            if(m_verticalAxis > 0){ return 1;}
            else if (m_verticalAxis < 0) { return -1;}
            else return 0;
        }
    }

}