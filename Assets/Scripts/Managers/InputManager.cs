using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    enum ACTIONS { ATTACK, DASH, JUMP, SKILL_1, SKILL_2, SKILL_3, NUMBER_OF_ACTIONS };

    static InputManager m_instance;
    static public InputManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    //------------------KEYBOARD--------------------------
    KeyCode attackButton_k    = KeyCode.X;
    KeyCode dashButton_k      = KeyCode.C;
    KeyCode jumpButton_k      = KeyCode.Z;
    KeyCode skill1Button_k    = KeyCode.A;
    KeyCode skill2Button_k    = KeyCode.S;
    KeyCode skill3Button_k    = KeyCode.D;


    //-------------------JOYSTICK-------------------------
    KeyCode attackButton_j = KeyCode.Joystick1Button2;
    KeyCode dashButton_j = KeyCode.Joystick1Button4;
    KeyCode jumpButton_j = KeyCode.Joystick1Button0;
    KeyCode skill1Button_j = KeyCode.Joystick1Button5;
    KeyCode skill2Button_j = KeyCode.Joystick1Button9;
    KeyCode skill3Button_j = KeyCode.Joystick1Button10;

    bool[] buttonsPressed   = new bool[6];
    bool[] buttonsHold      = new bool[6];
    bool[] buttonsReleased  = new bool[6];

    private void Awake()
    {
        if (m_instance == null) { m_instance = this; }
        else { Destroy(this.gameObject); }

        for (int i = 0; i < (int)ACTIONS.NUMBER_OF_ACTIONS; i++)
        {
            buttonsPressed[i] = false;
            buttonsHold[i] = false;
            buttonsReleased[i] = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < (int)ACTIONS.NUMBER_OF_ACTIONS; i++) {
            buttonsPressed[i] = false;
            buttonsHold[i] = false;
            buttonsReleased[i] = false;
        }

        if (Input.GetKeyDown(attackButton_k) || Input.GetKeyDown(attackButton_j))     { buttonsPressed[(int)ACTIONS.ATTACK]   = true; }
        if (Input.GetKeyDown(attackButton_k) || Input.GetKeyDown(attackButton_j))         { buttonsHold[(int)ACTIONS.ATTACK]      = true; }
        if (Input.GetKeyDown(attackButton_k) || Input.GetKeyDown(attackButton_j))       { buttonsReleased[(int)ACTIONS.ATTACK]  = true; }

        if (Input.GetKeyDown(dashButton_k) || Input.GetKeyDown(dashButton_j))       { buttonsPressed[(int)ACTIONS.DASH]     = true; }
        if (Input.GetKeyDown(dashButton_k) || Input.GetKeyDown(dashButton_j))           { buttonsHold[(int)ACTIONS.DASH]        = true; }
        if (Input.GetKeyDown(dashButton_k) || Input.GetKeyDown(dashButton_j))         { buttonsReleased[(int)ACTIONS.DASH]    = true; }

        if (Input.GetKeyDown(jumpButton_k) || Input.GetKeyDown(jumpButton_j))       { buttonsPressed[(int)ACTIONS.JUMP]     = true; }
        if (Input.GetKeyDown(jumpButton_k) || Input.GetKeyDown(jumpButton_j))           { buttonsHold[(int)ACTIONS.JUMP]        = true; }
        if (Input.GetKeyDown(jumpButton_k) || Input.GetKeyDown(jumpButton_j))         { buttonsReleased[(int)ACTIONS.JUMP]    = true; }

        if (Input.GetKeyDown(skill1Button_k))     { buttonsPressed[(int)ACTIONS.SKILL_1]  = true; }
        if (Input.GetKey(skill1Button_k))         { buttonsHold[(int)ACTIONS.SKILL_1]     = true; }
        if (Input.GetKeyUp(skill1Button_k))       { buttonsReleased[(int)ACTIONS.SKILL_1] = true; }

        if (Input.GetKeyDown(skill2Button_k))     { buttonsPressed[(int)ACTIONS.SKILL_2]  = true; }
        if (Input.GetKey(skill2Button_k))         { buttonsHold[(int)ACTIONS.SKILL_2]     = true; }
        if (Input.GetKeyUp(skill2Button_k))       { buttonsReleased[(int)ACTIONS.SKILL_2] = true; }

        if (Input.GetKeyDown(skill2Button_k))     { buttonsPressed[(int)ACTIONS.SKILL_3]  = true; }
        if (Input.GetKey(skill2Button_k))         { buttonsHold[(int)ACTIONS.SKILL_3]     = true; }
        if (Input.GetKeyUp(skill2Button_k))       { buttonsReleased[(int)ACTIONS.SKILL_3] = true; }
    }

    public bool AttackButtonPressed     { get { return buttonsPressed[(int)ACTIONS.ATTACK]; } }
    public bool AttackButtonHold        { get { return buttonsHold[(int)ACTIONS.ATTACK]; } }
    public bool AttackbuttonReleased    { get { return buttonsReleased[(int)ACTIONS.ATTACK]; } }

    public bool JumpButtonPressed       { get { return buttonsPressed[(int)ACTIONS.JUMP]; } }
    public bool JumpButtonHold          { get { return buttonsHold[(int)ACTIONS.JUMP]; } }
    public bool JumpbuttonReleased      { get { return buttonsReleased[(int)ACTIONS.JUMP]; } }

    public bool DashButtonPressed       { get { return buttonsPressed[(int)ACTIONS.DASH]; } }
    public bool DashButtonHold          { get { return buttonsPressed[(int)ACTIONS.DASH]; } }
    public bool DashbuttonReleased      { get { return buttonsPressed[(int)ACTIONS.DASH]; } }

    public bool Skill1ButtonPressed     { get { return buttonsPressed[(int)ACTIONS.SKILL_1]; } }
    public bool Skill1ButtonHold        { get { return buttonsHold[(int)ACTIONS.SKILL_1]; } }
    public bool Skill1buttonReleased    { get { return buttonsReleased[(int)ACTIONS.SKILL_1]; } }

    public bool Skill2ButtonPressed     { get { return buttonsPressed[(int)ACTIONS.SKILL_2]; } }
    public bool Skill2ButtonHold        { get { return buttonsHold[(int)ACTIONS.SKILL_2]; } }
    public bool Skill2buttonReleased    { get { return buttonsReleased[(int)ACTIONS.SKILL_2]; } }

    public bool Skill3ButtonPressed     { get { return buttonsPressed[(int)ACTIONS.SKILL_3]; } }
    public bool Skill3ButtonHold        { get { return buttonsHold[(int)ACTIONS.SKILL_3]; } }
    public bool Skill3buttonReleased    { get { return buttonsReleased[(int)ACTIONS.SKILL_3]; } }
}