using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{

    bool m_hasBeenPausedLastFrame = false;

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
    KeyCode m_interactButton_k  = KeyCode.Space;
    KeyCode m_cancelButton_k = KeyCode.X;
    KeyCode m_downButton_k = KeyCode.DownArrow;
    KeyCode m_downButton_j = KeyCode.DownArrow;
    KeyCode m_upButton_k = KeyCode.UpArrow;
    KeyCode m_pauseButton_k = KeyCode.Escape;
    KeyCode m_mapButton_k = KeyCode.M;

    //-------------------JOYSTICK-------------------------
    KeyCode m_attackButton_j    = KeyCode.Joystick1Button2;
    KeyCode m_dashButton_j      = KeyCode.Joystick1Button4;
    KeyCode m_jumpButton_j      = KeyCode.Joystick1Button0;
    KeyCode m_skill1Button_j    = KeyCode.Joystick1Button1;
    KeyCode m_skill2Button_j    = KeyCode.Joystick1Button3;
    KeyCode m_skill3Button_j    = KeyCode.Joystick1Button5;
    KeyCode m_interactButton_j  = KeyCode.Joystick1Button0;
    KeyCode m_cancelButton_j = KeyCode.Joystick1Button1;
    KeyCode m_pauseButton_j = KeyCode.Joystick1Button7;
    KeyCode m_mapButton_j = KeyCode.Joystick1Button6;

    bool[] m_buttonsPressed   = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];
    bool[] m_buttonsHold      = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];
    bool[] m_buttonsReleased  = new bool[(int)ACTIONS.NUMBER_OF_ACTIONS];

    float m_horizontalAxis;
    float m_verticalAxis;

    bool m_axisXPositivePressed = false;
    bool m_axisXPositiveHold = false;
    bool m_axisXPositiveReleased = false;
    bool m_axisYPositivePressed = false;
    bool m_axisYPositiveHold = false;
    bool m_axisYPositiveReleased = false;
    bool m_axisXNegativePressed = false;
    bool m_axisXNegativeHold = false;
    bool m_axisXNegativeReleased = false;
    bool m_axisYNegativePressed = false;
    bool m_axisYNegativeHold = false;
    bool m_axisYNegativeReleased = false;

    private GameObject selectedObj;

    private void Awake()
    {
        if (m_instance == null) { m_instance = this; }
        else { Destroy(this); }

        for (int i = 0; i < (int)ACTIONS.NUMBER_OF_ACTIONS; i++)
        {
            m_buttonsPressed[i]     = false;
            m_buttonsHold[i]        = false;
            m_buttonsReleased[i]    = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        if (EventSystem.current.currentSelectedGameObject == null)
             EventSystem.current.SetSelectedGameObject(selectedObj);
     
         selectedObj = EventSystem.current.currentSelectedGameObject;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ClearAllInput();
        
        if(m_hasBeenPausedLastFrame){
            m_hasBeenPausedLastFrame = false;
            return ;
        }

        if (Input.GetKeyDown(m_attackButton_k) || Input.GetKeyDown(m_attackButton_j))       { m_buttonsPressed[(int)ACTIONS.ATTACK]   = true; }
        if (Input.GetKey(m_attackButton_k) || Input.GetKey(m_attackButton_j))       { m_buttonsHold[(int)ACTIONS.ATTACK]      = true; }
        if (Input.GetKeyUp(m_attackButton_k) || Input.GetKeyUp(m_attackButton_j))       { m_buttonsReleased[(int)ACTIONS.ATTACK]  = true; }

        if (Input.GetKeyDown(m_dashButton_k) || Input.GetKeyDown(m_dashButton_j))           { m_buttonsPressed[(int)ACTIONS.DASH]     = true; }
        if (Input.GetKey(m_dashButton_k) || Input.GetKey(m_dashButton_j))           { m_buttonsHold[(int)ACTIONS.DASH]        = true; }
        if (Input.GetKeyUp(m_dashButton_k) || Input.GetKeyUp(m_dashButton_j))           { m_buttonsReleased[(int)ACTIONS.DASH]    = true; }

        if (Input.GetKeyDown(m_jumpButton_k) || Input.GetKeyDown(m_jumpButton_j))           { m_buttonsPressed[(int)ACTIONS.JUMP]     = true; }
        if (Input.GetKey(m_jumpButton_k) || Input.GetKey(m_jumpButton_j))           { m_buttonsHold[(int)ACTIONS.JUMP]        = true; }
        if (Input.GetKeyUp(m_jumpButton_k) || Input.GetKeyUp(m_jumpButton_j))           { m_buttonsReleased[(int)ACTIONS.JUMP]    = true; }

        if (Input.GetKeyDown(m_skill1Button_k) || Input.GetKeyDown(m_skill1Button_j))       { m_buttonsPressed[(int)ACTIONS.SKILL_1]  = true; }
        if (Input.GetKey(m_skill1Button_k) || Input.GetKey(m_skill1Button_k))               { m_buttonsHold[(int)ACTIONS.SKILL_1]     = true; }
        if (Input.GetKeyUp(m_skill1Button_k) || Input.GetKeyUp(m_skill1Button_j))           { m_buttonsReleased[(int)ACTIONS.SKILL_1] = true; }

        if (Input.GetKeyDown(m_skill2Button_k) || Input.GetKeyDown(m_skill2Button_j))       { m_buttonsPressed[(int)ACTIONS.SKILL_2]  = true; }
        if (Input.GetKey(m_skill2Button_k) || Input.GetKey(m_skill2Button_j))               { m_buttonsHold[(int)ACTIONS.SKILL_2]     = true; }
        if (Input.GetKeyUp(m_skill2Button_k) || Input.GetKeyUp(m_skill2Button_j))           { m_buttonsReleased[(int)ACTIONS.SKILL_2] = true; }

        if (Input.GetKeyDown(m_skill3Button_k) || Input.GetKeyDown(m_skill3Button_j))       { m_buttonsPressed[(int)ACTIONS.SKILL_3]  = true; }
        if (Input.GetKey(m_skill3Button_k) || Input.GetKey(m_skill3Button_j))               { m_buttonsHold[(int)ACTIONS.SKILL_3]     = true; }
        if (Input.GetKeyUp(m_skill3Button_k) || Input.GetKeyUp(m_skill3Button_j))           { m_buttonsReleased[(int)ACTIONS.SKILL_3] = true; }

        if (Input.GetKey(m_interactButton_k) || Input.GetKey(m_interactButton_j))           { m_buttonsHold[(int)ACTIONS.INTERACT]     = true; }
        if (Input.GetKeyUp(m_interactButton_k) || Input.GetKeyUp(m_interactButton_j))       { m_buttonsReleased[(int)ACTIONS.INTERACT] = true; }
        if (Input.GetKeyDown(m_interactButton_k) || Input.GetKeyDown(m_interactButton_j))   { m_buttonsPressed[(int)ACTIONS.INTERACT]  = true; }

        if (Input.GetKey(m_cancelButton_j) || Input.GetKey(m_cancelButton_k))           { m_buttonsHold[(int)ACTIONS.CANCEL]     = true; }
        if (Input.GetKeyUp(m_cancelButton_j) || Input.GetKeyUp(m_cancelButton_k))       { m_buttonsReleased[(int)ACTIONS.CANCEL] = true; }
        if (Input.GetKeyDown(m_cancelButton_j) || Input.GetKeyDown(m_cancelButton_k))   { m_buttonsPressed[(int)ACTIONS.CANCEL]  = true; }

        if (Input.GetKey(m_downButton_k) || Input.GetKey(m_downButton_k))           { m_buttonsHold[(int)ACTIONS.BOTTOM_UI]     = true; }
        if (Input.GetKeyUp(m_downButton_k) || Input.GetKeyUp(m_downButton_k))       { m_buttonsReleased[(int)ACTIONS.BOTTOM_UI] = true; }
        if (Input.GetKeyDown(m_downButton_k) || Input.GetKeyDown(m_downButton_k))   { m_buttonsPressed[(int)ACTIONS.BOTTOM_UI]  = true; }

        if (Input.GetKey(m_upButton_k) || Input.GetKey(m_upButton_k))           { m_buttonsHold[(int)ACTIONS.UP_UI]     = true; }
        if (Input.GetKeyUp(m_upButton_k) || Input.GetKeyUp(m_upButton_k))       { m_buttonsReleased[(int)ACTIONS.UP_UI] = true; }
        if (Input.GetKeyDown(m_upButton_k) || Input.GetKeyDown(m_upButton_k))   { m_buttonsPressed[(int)ACTIONS.UP_UI]  = true; }

        if (Input.GetKey(m_pauseButton_k) || Input.GetKey(m_pauseButton_j))           { m_buttonsHold[(int)ACTIONS.PAUSE]     = true; }
        if (Input.GetKeyUp(m_pauseButton_k) || Input.GetKeyUp(m_pauseButton_j))       { m_buttonsReleased[(int)ACTIONS.PAUSE] = true; }
        if (Input.GetKeyDown(m_pauseButton_k) || Input.GetKeyDown(m_pauseButton_j))   { m_buttonsPressed[(int)ACTIONS.PAUSE]  = true; }

        if (Input.GetKey(m_mapButton_k) || Input.GetKey(m_mapButton_j))           { m_buttonsHold[(int)ACTIONS.MAP]     = true; }
        if (Input.GetKeyUp(m_mapButton_k) || Input.GetKeyUp(m_mapButton_j))       { m_buttonsReleased[(int)ACTIONS.MAP] = true; }
        if (Input.GetKeyDown(m_mapButton_k) || Input.GetKeyDown(m_mapButton_j))   { m_buttonsPressed[(int)ACTIONS.MAP]  = true; }
    }

    public void ClearAllInput(){
        for (int i = 0; i < (int)ACTIONS.NUMBER_OF_ACTIONS; i++)
        {
            m_buttonsPressed[i]     = false;
            m_buttonsHold[i]        = false;
            m_buttonsReleased[i]    = false;
        }

        m_horizontalAxis = 0;
        m_verticalAxis = 0;

        m_horizontalAxis = Input.GetAxis("Horizontal");
        m_verticalAxis = Input.GetAxis("Vertical");

        if(m_horizontalAxis == 1 && !m_axisXPositiveHold && !m_axisXPositivePressed){
            m_axisXPositivePressed = true;
        }
        else if (m_horizontalAxis == 1 && m_axisXPositivePressed){
            m_axisXPositivePressed = false;
            m_axisXPositiveHold = true;
        }
        else if(m_horizontalAxis != 1 && m_axisXPositiveHold){
            m_axisXPositiveHold = false;
            m_axisXPositiveReleased = true;
        }
        else if(m_axisXPositiveReleased){
            m_axisXPositiveReleased = false;
        }

        if(m_horizontalAxis == -1 && !m_axisXNegativeHold && !m_axisXNegativePressed){
            m_axisXNegativePressed = true;
        }
        else if (m_horizontalAxis == -1 && m_axisXNegativePressed){
            m_axisXNegativePressed = false;
            m_axisXNegativeHold = true;
        }
        else if(m_horizontalAxis != -1 && m_axisXNegativeHold){
            m_axisXNegativeHold = false;
            m_axisXNegativeReleased = true;
        }
        else if(m_axisXNegativeReleased){
            m_axisXNegativeReleased = false;
        }



        if(m_verticalAxis == 1 && !m_axisYPositiveHold && !m_axisYPositivePressed){
            m_axisYPositivePressed = true;
        }
        else if (m_verticalAxis == 1 && m_axisYPositivePressed){
            m_axisYPositivePressed = false;
            m_axisYPositiveHold = true;
        }
        else if(m_verticalAxis != 1 && m_axisYPositiveHold){
            m_axisYPositiveHold = false;
            m_axisYPositiveReleased = true;
        }
        else if(m_axisYPositiveReleased){
            m_axisYPositiveReleased = false;
        }

        if(m_verticalAxis == -1 && !m_axisYNegativeHold && !m_axisYNegativePressed){
            m_axisYNegativePressed = true;
        }
        else if (m_verticalAxis == -1 && m_axisYNegativePressed){
            m_axisYNegativePressed = false;
            m_axisYNegativeHold = true;
        }
        else if(m_verticalAxis != -1 && m_axisYNegativeHold){
            m_axisYNegativeHold = false;
            m_axisYNegativeReleased = true;
        }
        else if(m_axisYNegativeReleased){
            m_axisYNegativeReleased = false;
        }
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

    public bool InteractButtonPressed   { get { return m_buttonsPressed[(int)ACTIONS.INTERACT]; } }
    public bool InteractButtonHold      { get { return m_buttonsHold[(int)ACTIONS.INTERACT]; } }
    public bool InteractButtonReleased  { get { return m_buttonsReleased[(int)ACTIONS.INTERACT]; } }

    public bool CancelButtonPressed   { get { return m_buttonsPressed[(int)ACTIONS.CANCEL]; } }
    public bool CancelButtonHold      { get { return m_buttonsHold[(int)ACTIONS.CANCEL]; } }
    public bool CancelButtonReleased  { get { return m_buttonsReleased[(int)ACTIONS.CANCEL]; } }

    public bool UpButtonPressed   { get { return m_buttonsPressed[(int)ACTIONS.UP_UI]; } }
    public bool UpButtonHold      { get { return m_buttonsHold[(int)ACTIONS.UP_UI]; } }
    public bool UpButtonReleased  { get { return m_buttonsReleased[(int)ACTIONS.UP_UI]; } }

    public bool DownButtonPressed   { get { return m_buttonsPressed[(int)ACTIONS.BOTTOM_UI]; } }
    public bool DownButtonHold      { get { return m_buttonsHold[(int)ACTIONS.BOTTOM_UI]; } }
    public bool DownButtonReleased  { get { return m_buttonsReleased[(int)ACTIONS.BOTTOM_UI]; } }

    public bool PauseButtonPressed   { get { return m_buttonsPressed[(int)ACTIONS.PAUSE]; } }
    public bool PauseButtonHold      { get { return m_buttonsHold[(int)ACTIONS.PAUSE]; } }
    public bool PauseButtonReleased  { get { return m_buttonsReleased[(int)ACTIONS.PAUSE]; } }

    public bool MapButtonPressed   { get { return m_buttonsPressed[(int)ACTIONS.MAP]; } }
    public bool MapButtonHold      { get { return m_buttonsHold[(int)ACTIONS.MAP]; } }
    public bool MapButtonReleased  { get { return m_buttonsReleased[(int)ACTIONS.MAP]; } }

    public float HorizontalAxis { get { return m_horizontalAxis; } }
    public float VerticalAxis { get { return m_verticalAxis; } }

    public float HorizontalAxisFlat
    {
        get
        {
            if (m_horizontalAxis > 0) { return 1; }
            else if (m_horizontalAxis < 0) { return -1; }
            else { return 0; }
        }
    }
    public float VerticalAxisFlat
    {
        get
        {
            if (m_verticalAxis > 0) { return 1; }
            else if (m_verticalAxis < 0) { return -1; }
            else { return 0; }
        }
    }

    public float HorizontalAxisRaw
    {
        get
        {
            if (m_horizontalAxis == 1) { return 1; }
            else if (m_horizontalAxis == -1) { return -1; }
            else { return 0; }
        }
    }
    public float VerticalAxisRaw
    {
        get
        {
            if (m_verticalAxis == 1) { return 1; }
            else if (m_verticalAxis == -1) { return -1; }
            else { return 0; }
        }
    }

    public bool HorizontalPositiveAxisPressed { get { return m_axisXPositivePressed; } }
    public bool HorizontalPositiveAxisHold { get { return m_axisXPositiveHold; } }
    public bool HorizontalPositiveAxisReleased { get { return m_axisXPositiveReleased; } }

    public bool HorizontalNegativeAxisPressed { get { return m_axisXNegativePressed; } }
    public bool HorizontalNegativeAxisHold { get { return m_axisXNegativeHold; } }
    public bool HorizontalNegativeAxisReleased { get { return m_axisXNegativeReleased; } }

    public bool VerticalPositiveAxisPressed { get { return m_axisYPositivePressed; } }
    public bool VerticalPositiveAxisHold { get { return m_axisYPositiveHold; } }
    public bool VerticalPositiveAxisReleased { get { return m_axisYPositiveReleased; } }
    public bool VerticalNegativeAxisPressed { get { return m_axisYNegativePressed; } }
    public bool VerticalNegativeAxisHold { get { return m_axisYNegativeHold; } }
    public bool VerticalNegativeAxisReleased { get { return m_axisYNegativeReleased; } }

    public void PauseInputFor1Frame(){
        m_hasBeenPausedLastFrame = true;
    }

}