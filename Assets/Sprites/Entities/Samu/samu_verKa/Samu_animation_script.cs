using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Events;

public class Samu_animation_script : MonoBehaviour
{
    UnityEvent m_endOfEnragedChargeEvent;
    public UnityEvent EndOfEnragedChargeEvent { get { return m_endOfEnragedChargeEvent; } }
    UnityEvent m_endOfFireBallSummon;
    public UnityEvent EndOfFireballSummonEvent { get { return m_endOfFireBallSummon; } }
    UnityEvent m_arriveToCenterEvent;
    public UnityEvent ArriveToCenterEvent { get { return m_arriveToCenterEvent; } }
    UnityEvent m_fullyRecoveredEvent;
    public UnityEvent FullyRecoveredEvent { get { return m_fullyRecoveredEvent; } }

    UnityEvent m_unsummonCirclesEventAtk1;
    public UnityEvent UnsummonCirclesEventAtk1 { get { return m_unsummonCirclesEventAtk1; } }
    UnityEvent m_unsummonCirclesEventAtk2;
    public UnityEvent UnsummonCirclesEventAtk2 { get { return m_unsummonCirclesEventAtk2; } }

    UnityEvent m_eyesDeadEvent;
    public UnityEvent EyesDeadEvent { get { return m_eyesDeadEvent; } }
    bool m_areEyesAlive = true;

    UnityEvent m_endNormalChargesEvent;
    public UnityEvent EndNormalChargesEvent { get { return m_endNormalChargesEvent; } }

    #region Variables   
    enum Anim_States { STOP, BACK2ORIGIN, IDLE, ATK1, ATK1VAR, ATK2 };
    enum BodyParts { CORE, INNER_RING, OUTER_RING }
    enum Bodies { MAIN, BODY1 }
    Anim_States state = Anim_States.STOP;
    Anim_States next_state = Anim_States.STOP;
    Anim_States prev_state = Anim_States.STOP;


    //[SerializeField] GameObject innerRing;
    //[SerializeField] GameObject outerRing;
    //[SerializeField] GameObject core;
    
    [Header("Transferable Objects")]
    [Tooltip("Glowing magic circle")]
    [SerializeField] GameObject mainCircle;
    [Tooltip("Parent object for eyes")]
    [SerializeField] GameObject eye_obj;
    [Tooltip("Light")]
    [SerializeField] GameObject SamuLight;

    [Header ("Bodies and bodyparts")]
    [SerializeField] GameObject[] Samu_bodies;
    [SerializeField] GameObject[] MainBodyParts;
    [SerializeField] GameObject[] Body1Parts;

    [Header("Materials")]
    [SerializeField] Material WhiteMaterial;
    [SerializeField] Material ringMaterial;
    [SerializeField] Material coreMaterial;


    [Header("Fireball things")]
    [SerializeField] GameObject atk1_Fireball_pos_obj;
    [SerializeField] Samu_BigFireball fireball_pref;
    private GameObject[] Atk1_fireball_init_pos;
    public Samu_BigFireball[] Atk1_fireballs;

    [SerializeField] GameObject atk1var_Fireball_posObj;
    private GameObject[] Atk1var_fireball_init_pos;
    public Samu_BigFireball[] Atk1var_fireballs;

    [SerializeField] Transform floorLevelpos;
    private GameObject currentBody;
    private GameObject[] eyes;
    private GameObject[] eyes_init_pos;
    private GameObject player;
    Samu_BigFireball[] fb;

    [Header("Controlable var")]
    [SerializeField] float dash_velocity = 5f;
    [SerializeField] float idle_velocity = 2.5f;
    [SerializeField] float enterOnScreenSpeed = 2.5f;
    [SerializeField] float trackingTimer = 1;
    [SerializeField] int tick_counter = 3;
    [SerializeField] int max_dash_number = 2;


    Vector3 init_pos;
    float time;
    private int dash_number = 1;
    private int dir = 0;

    private bool playerOnSight = false;
    private bool canMove = true;
    private bool changeState = false;
    private bool fireballsSummoned = false;
    private bool fireballsThrown = false;

    private float trackingTimer_time = 0;
    private bool tracking = false;
    private bool canGoOffscreen = false;
    private bool Atk2Finished = false;
    private bool enraged = false;
    private bool IsStuck = false;

    private float tickingTimer = 1;
    private float tickingTimer_time = 0;


    private float base_intesity = 1.5f;
    private float max_intesity = 40;
    private bool increaseIntensity = true;


    private bool unstuck_cycle_counter = false;
    private float damp_f = 1;


    Vector3 cam_bounds_;
    Vector3 init_wPos;

    Vector3 cameraOffscreenPoint;
    Vector3 point;


    float softening_movement_mod = 0.3f;

    private Vector3 rot_speed = new Vector3(0, 0, 0.3f);
    #endregion
    private void Awake()
    {
        m_endOfEnragedChargeEvent = new UnityEvent();
        m_endOfFireBallSummon = new UnityEvent();
        m_arriveToCenterEvent = new UnityEvent();
        m_unsummonCirclesEventAtk1 = new UnityEvent();
        m_unsummonCirclesEventAtk2 = new UnityEvent();
        m_eyesDeadEvent = new UnityEvent();
        m_endNormalChargesEvent = new UnityEvent();
        m_fullyRecoveredEvent = new UnityEvent();
    }
    void Start()
    {
        currentBody = Samu_bodies[0];

        List<GameObject> list = new List<GameObject>();
        int eyes_obj_id = eye_obj.GetInstanceID();

        foreach (Collider2D eye in eye_obj.GetComponentsInChildren<Collider2D>())
        {
            if (eye.gameObject.GetInstanceID() != eyes_obj_id)
            {
                list.Add(eye.gameObject);
            }
        }
        eyes_init_pos = list.ToArray();
        list.Clear();

        foreach (GameObject eye in eyes_init_pos)
        {
            list.Add(eye.GetComponentInChildren<SpriteRenderer>().gameObject);
        }
        eyes = list.ToArray();
        list.Clear();

        foreach (SpriteRenderer fire in atk1_Fireball_pos_obj.GetComponentsInChildren<SpriteRenderer>())
        {
            fire.gameObject.transform.localScale = Vector3.zero;
            list.Add(fire.gameObject);
        }
        Atk1_fireball_init_pos = list.ToArray();
        list.Clear();

        foreach (SpriteRenderer fire in atk1var_Fireball_posObj.GetComponentsInChildren<SpriteRenderer>())
        {
            fire.gameObject.transform.localScale = Vector3.zero;
            list.Add(fire.gameObject);
        }
        Atk1var_fireball_init_pos = list.ToArray();
        init_pos = currentBody.transform.localPosition;
        Samu_bodies[1].SetActive(false);
        //UnsummonCircles();
    }

    void Update()
    {
        ///ROTATION 
        float sin = Mathf.Cos(Time.time / 2) * 1.5f;
        //Debug.Log(sin);
        if (currentBody == Samu_bodies[((int)(Bodies.MAIN))])
        {
            MainBodyParts[(int)(BodyParts.INNER_RING)].transform.Rotate(new Vector3(0, 0, 30 * sin  * Time.deltaTime));
            MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.Rotate(new Vector3(0, 0, - 30 * sin * Time.deltaTime));
        }

        if (!Samu_bodies[1].activeSelf)
        {
            eye_obj.transform.rotation = new Quaternion(MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.x, MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.y, MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.z, MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.w);
        }
        else
        {
            eye_obj.transform.rotation = new Quaternion(Samu_bodies[1].transform.rotation.x, Samu_bodies[1].transform.rotation.y, Samu_bodies[1].transform.rotation.z, -Samu_bodies[1].transform.rotation.w);
        }
        mainCircle.transform.Rotate(30 * rot_speed * Time.deltaTime);

        for (int i = 0; i < Atk1_fireball_init_pos.Length; i++)
        {
            Atk1_fireball_init_pos[i].transform.Rotate(30 * rot_speed * 4f * Time.deltaTime);
        }

        for (int i = 0; i < Atk1var_fireball_init_pos.Length; i++)
        {
            Atk1var_fireball_init_pos[i].transform.Rotate(30 * rot_speed * 4f * Time.deltaTime);
        }
        int eyes_alive = eyes.Length;
        ///EYETRACKING
        for (int i = 0; i < eyes.Length; i++)
        {
            if (!eyes_init_pos[i].activeSelf)
            {
                eyes_alive--;
            }
            if (playerOnSight)
            {
                Vector3 offset = player.transform.position - eyes_init_pos[i].transform.position;
                Vector3 direction = Vector3.ClampMagnitude(offset, 1.5f);

                eyes[i].transform.localPosition = (new Vector3(eyes_init_pos[i].transform.position.x, eyes_init_pos[i].transform.position.y, 0) + new Vector3(eyes_init_pos[i].transform.position.x - direction.x, eyes_init_pos[i].transform.position.y - direction.y, 0) * -1);
            }
            else { eyes[i].transform.position = eyes_init_pos[i].transform.position; }
            if (!Samu_bodies[1].activeSelf)
            {
                eyes_init_pos[i].transform.localRotation = new Quaternion(MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.x, MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.y, MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.z, -MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.w);
            }
            else
            {
                eyes_init_pos[i].transform.localRotation = new Quaternion(Samu_bodies[1].transform.rotation.x, Samu_bodies[1].transform.rotation.y, Samu_bodies[1].transform.rotation.z, -Samu_bodies[1].transform.rotation.w);
            }
        }
        if (eyes_alive == 0 && m_areEyesAlive)
        {
            Debug.Log("eyes dead");
            m_areEyesAlive = false;
            m_eyesDeadEvent.Invoke();
            enraged = true;
            Body1Parts[(int)(BodyParts.OUTER_RING)].GetComponent<SpriteRenderer>().forceRenderingOff = true;
            MainBodyParts[(int)(BodyParts.OUTER_RING)].GetComponent<SpriteRenderer>().forceRenderingOff = true;
        }

        magicCircleController(eyes_alive);

        cam_bounds_ = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        init_wPos = transform.TransformPoint(init_pos);
        
        //INPUT PROVISIONAL
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            ATK2();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ATK1();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ATK1VAR();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Stop();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveIdle();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GoBackCenter();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            KillFB();
        }*/
        AnimationInputController();
        

        //MOVEMENT
        switch (state)
        {
            case Anim_States.IDLE:
                time += Time.deltaTime;
                float x_pos = Mathf.Cos(time) * 200;
                float y_pos = Mathf.Sin(time * 2f) * 60;
                int max_range = 2;

                if (changeState)
                {
                    if (!(NumberInRange(x_pos, currentBody.transform.localPosition.x, max_range) && NumberInRange(y_pos, currentBody.transform.localPosition.y, max_range)))
                    {
                        Vector3 offset = currentBody.transform.localPosition - (init_pos + new Vector3(x_pos, -y_pos, 0));
                        Vector3 direction = Vector3.ClampMagnitude(offset, idle_velocity);
                        currentBody.transform.localPosition += -direction;
                        canMove = false;
                    }
                    else
                    {
                        softening_movement_mod = 0.3f;
                        changeState = false;
                        canMove = true;
                    }
                }
                if (canMove)
                {
                    if (softening_movement_mod < 1) { softening_movement_mod += Time.deltaTime; }

                    Vector3 motion = init_pos + new Vector3(x_pos * softening_movement_mod, -y_pos * softening_movement_mod, 0);

                    currentBody.transform.localPosition = motion;
                }
                break;

            case Anim_States.BACK2ORIGIN:
                if (currentBody.transform.localPosition != init_pos)
                {
                    Back2Origin_f();
                }
                else
                {
                    next_state = Anim_States.STOP;
                    m_arriveToCenterEvent.Invoke();
                }
                break;
            case Anim_States.STOP:
                break;
            case Anim_States.ATK1:
                if (!fireballsSummoned) { SummonFireballs(Atk1_fireball_init_pos, Atk1_fireballs); }
                break;
            case Anim_States.ATK1VAR:
                if (!fireballsSummoned) { SummonFireballs(Atk1var_fireball_init_pos, Atk1var_fireballs); }

                break;
            case Anim_States.ATK2:
                if (!Atk2Finished)
                {
                    /*if (!IsStuck)
                    {
                        GoOffscreen();
                    }*/
                    GoOffscreen();
                }
                else
                {
                    /*if (Samu_bodies[(int)(Bodies.BODY1)].transform.position.y != cam_bounds_.y - init_pos.y / 2 + 100 && enraged) { 
                        MoveToPoint(currentBody.transform, new Vector3(currentBody.transform.position.x, cam_bounds_.y - init_pos.y / 2 + 100, currentBody.transform.position.z), 2.5f, 1);
                    }
                    else*/
                    if (currentBody != Samu_bodies[(int)(Bodies.MAIN)])
                    {
                        TransferBody(Samu_bodies[(int)(Bodies.MAIN)]);
                        if (m_areEyesAlive)
                        {
                            //Samu_bodies[(int)Bodies.MAIN].transform.position = Samu_bodies[(int)Bodies.BODY1].transform.position;
                            m_endNormalChargesEvent.Invoke();
                            Debug.Log("end of normal phase");
                            Atk2Finished = false;
                            canGoOffscreen = false;
                        }
                        else
                        {
                            m_endOfEnragedChargeEvent.Invoke();
                            Atk2Finished = false;
                            canGoOffscreen = false;
                            Debug.Log("end of enraged phase");
                        }
                    }
                    else
                    {
                        Back2Origin_f();
                    }
                }
                if (currentBody.transform.localPosition == init_pos && Atk2Finished)
                {
                    Atk2Finished = false;
                    canGoOffscreen = false;
                    //state = prev_state;
                }
                break;
        }
        /*if (fireballsSummoned && ((Atk1var_fireballs.Length == 0 || !Atk1var_fireballs[0].isActiveAndEnabled) && (Atk1_fireballs.Length == 0 || !Atk1_fireballs[0].isActiveAndEnabled)))
        {
            UnsummonCircles();
        }*/
        mainCircle.transform.position = currentBody.transform.position;
        eye_obj.transform.position = currentBody.transform.position;
        SamuLight.transform.position = currentBody.transform.position;
    }
    private void magicCircleController(int eyes_alive)
    {
        Vector3 circleScale = new Vector3((eyes_alive + 1f) / (eyes.Length+1f), (eyes_alive + 1f) / (eyes.Length+1f) , 0);

        circleScale = new Vector3(Mathf.Clamp(circleScale.x, 0.25f, 1), Mathf.Clamp(circleScale.x, 0.25f, 1), 0); ;
        if (circleScale.x > mainCircle.transform.localScale.x)
        {
            mainCircle.transform.localScale += new Vector3(Time.deltaTime / 2, Time.deltaTime / 2, 0);
            SamuLight.GetComponent<Light2D>().pointLightInnerAngle += Time.deltaTime * 2;
            SamuLight.GetComponent<Light2D>().pointLightOuterRadius += Time.deltaTime * 2;
        }

        if (circleScale.x < mainCircle.transform.localScale.x)
        {
            mainCircle.transform.localScale -= new Vector3(Time.deltaTime / 2, Time.deltaTime / 2, 0);
            SamuLight.GetComponent<Light2D>().pointLightInnerRadius -= Time.deltaTime * 2;
            SamuLight.GetComponent<Light2D>().pointLightOuterRadius -= Time.deltaTime * 2;
        }
    
}
    private void ThrowFireballs(Samu_BigFireball[] FBs)
    {

    }
    public void GetUnstuck()
    {
        if (true/*IsStuck*/)
        {
            if (tick_counter != 0)
            {
                if (tickingTimer_time < tickingTimer)
                { tickingTimer_time += Time.deltaTime; }
                else
                {
                    if (!unstuck_cycle_counter)
                    {
                        Body1Parts[0].GetComponent<SpriteRenderer>().material = WhiteMaterial;
                        Body1Parts[1].GetComponent<SpriteRenderer>().material = WhiteMaterial;
                        Body1Parts[2].GetComponent<SpriteRenderer>().material = WhiteMaterial;
                    }
                    else
                    {
                        Body1Parts[0].GetComponent<SpriteRenderer>().material = coreMaterial;
                        Body1Parts[1].GetComponent<SpriteRenderer>().material = ringMaterial;
                        Body1Parts[2].GetComponent<SpriteRenderer>().material = ringMaterial;
                        tick_counter--;
                    }
                    unstuck_cycle_counter = !unstuck_cycle_counter;
                    tickingTimer_time = 0;
                }
            }
            else
            {
                if (increaseIntensity)
                {
                    base_intesity += Time.deltaTime * 20;
                    Body1Parts[1].GetComponent<SpriteRenderer>().material.SetFloat("_Intensity", base_intesity);
                    Body1Parts[2].GetComponent<SpriteRenderer>().material.SetFloat("_Intensity", base_intesity);

                    if (base_intesity > max_intesity / 5)
                    {
                        for (int i = 0; i < eyes.Length; i++)
                        {
                            eyes_init_pos[i].SetActive(true);
                            Body1Parts[(int)(BodyParts.OUTER_RING)].GetComponent<SpriteRenderer>().forceRenderingOff = false;
                            MainBodyParts[(int)(BodyParts.OUTER_RING)].GetComponent<SpriteRenderer>().forceRenderingOff = false;
                        }
                    }

                    if (base_intesity >= max_intesity) { increaseIntensity = false; }
                }
                else
                {
                    MoveToPoint(currentBody.transform, new Vector3(currentBody.transform.position.x, init_wPos.y, currentBody.transform.position.z), 1.5f, 1);

                    base_intesity -= Time.deltaTime * 15;
                    if (base_intesity < 1.5f)
                    {
                        base_intesity = 1.5f;
                    }
                    Body1Parts[1].GetComponent<SpriteRenderer>().material.SetFloat("_Intensity", base_intesity);
                    Body1Parts[2].GetComponent<SpriteRenderer>().material.SetFloat("_Intensity", base_intesity);
                    
                }
                if (currentBody.transform.position.y == init_wPos.y && base_intesity <= 1.5F)
                {
                    //Atk2Finished = true;
                    increaseIntensity = true;
                    //IsStuck = false;
                    Samu_bodies[(int)Bodies.MAIN].transform.position = currentBody.transform.position;
                    TransferBody(Samu_bodies[(int)(Bodies.MAIN)]);
                    m_fullyRecoveredEvent.Invoke();
                    Debug.Log("FULLY RECOVERED");
                }
            }
        }
    }
    /// <summary>
    /// Moves entity to point 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="WhereTo"></param>
    /// <param name="Magnitude"></param>
    /// <param name="Mode">Mode 0 = Move in local position; Mode 1 = Move in Global position </param>
    private void MoveToPoint(Transform entity, Vector3 WhereTo, float Magnitude, int Mode = 0)
    {
        Vector3 offset;
        Vector3 direction;
        switch (Mode)
        {
            //Local Position
            case 0:
                offset = entity.localPosition - WhereTo;
                direction = Vector3.ClampMagnitude(offset, Magnitude);
                direction = direction * Time.deltaTime * 10;

                entity.localPosition += -direction;

                break;
            //Global Position
            case 1:
                offset = entity.position - WhereTo;
                direction = Vector3.ClampMagnitude(offset, Magnitude);
                direction = direction * Time.deltaTime * 10;
                entity.position += -direction;
                break;
        }       
    }
    private void MoveToPoint(Transform entity, Vector3 WhereTo, float MagnitudeX, float MagnitudeY, int Mode = 0)
    {

        Vector3 offset;
        Vector3 direction;
        float newX;
        float newY;

        switch (Mode)
        {
            //Local Position
            case 0:
                offset = entity.localPosition - WhereTo;
                newX = Mathf.Clamp(offset.x, -MagnitudeX, MagnitudeX);
                newY = Mathf.Clamp(offset.y, -MagnitudeY, MagnitudeY);
                direction = new Vector3(newX, newY, entity.localPosition.z);
                direction = direction * Time.deltaTime * 10;

                entity.localPosition += -direction;

                break;
            //Global Position
            case 1:
                offset = entity.position - WhereTo;
                newX = Mathf.Clamp(offset.x, -MagnitudeX, MagnitudeX);
                newY = Mathf.Clamp(offset.y, -MagnitudeY, MagnitudeY);
                direction = new Vector3(newX, newY, entity.localPosition.z);
                direction = direction * Time.deltaTime * 10;

                entity.position += -direction;
                break;
        }
    }

    private void Back2Origin_f()
    {
        MoveToPoint(currentBody.transform, init_pos, 25f);

        changeState = false;
    }

    private void GoOffscreen()
    {
        if (currentBody.transform.localPosition == init_pos)
        {
            canGoOffscreen = true;
        }

        if (currentBody.transform.localPosition != init_pos && !canGoOffscreen)
        {
            Back2Origin_f();
        }

        if (canGoOffscreen && Samu_bodies[(int)(Bodies.MAIN)].activeSelf)
        {
            MoveToPoint(currentBody.transform, point, 2.5f, 1);
        }
        if (currentBody.transform.position.y >= cameraOffscreenPoint.y - init_pos.y && !Samu_bodies[1].activeSelf)
        {
            TransferBody(Samu_bodies[1]);
        }
        if (Samu_bodies[(int)(Bodies.BODY1)].activeSelf)
        {
            MoveOnScreen(dash_number);
        }
    }
    private void Move_track_dash(int direction)
    {
        float dir;
        float max_dir;
        if (direction != 2)
        {
            //RIGHT 2 LEFT
            if (direction == 0)
            {
                dir = cam_bounds_.x + init_pos.x / 2 - 15;
                max_dir = cam_bounds_.x - init_pos.x / 2 - 100;
                if (!tracking)
                {
                    if (currentBody.transform.position.x <= dir)
                    {
                        tracking = true;
                    }
                    else
                    {
                        Vector3 EndPos = new Vector3(dir, player.transform.position.y + 10, init_wPos.z - init_pos.z);

                        float damp_f = 2.5f;
                        MoveToPoint(currentBody.transform, EndPos, damp_f, 1);
                    }
                }
            }
            // LEFT 2 RIGHT
            else
            {
                dir = cam_bounds_.x - init_pos.x / 2 + 15;
                max_dir = cam_bounds_.x + init_pos.x / 2 + 100;
                if (!tracking)
                {
                    if (currentBody.transform.position.x >= dir)
                    {
                        tracking = true;
                    }
                    else
                    {
                        Vector3 EndPos = new Vector3(dir, player.transform.position.y + 10, init_wPos.z - init_pos.z);

                        float damp_f = 2.5f;
                        MoveToPoint(currentBody.transform, EndPos, damp_f, 1);
                    }
                }
            }

            if (tracking)
            {
                if (trackingTimer_time < trackingTimer)
                {
                    MoveToPoint(currentBody.transform, new Vector3(dir, player.transform.position.y + 10, init_wPos.z - init_pos.z), 5f, 1f, 1); ;
                    trackingTimer_time += Time.deltaTime;
                }
                else
                {
                    Vector3 EndPos = new Vector3(max_dir, currentBody.transform.position.y, currentBody.transform.position.z);
                     
                    if ((direction == 0 && currentBody.transform.position.x < cam_bounds_.x) || (direction == 1 && currentBody.transform.position.x > cam_bounds_.x)) {
                        damp_f -=  Time.deltaTime*0.75f ;
                        damp_f = Mathf.Clamp(damp_f, 0.3f, 1f);


                            }
                    if (direction==1 && currentBody.transform.rotation.eulerAngles.z > -25 && currentBody.transform.position.x < cam_bounds_.x) { currentBody.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * 20)); }
                    if (direction == 1& currentBody.transform.rotation.eulerAngles.z < 0 && currentBody.transform.position.x > cam_bounds_.x) { currentBody.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 15)); }




                    if (direction ==0 && currentBody.transform.rotation.eulerAngles.z < 25 && currentBody.transform.position.x > cam_bounds_.x) { currentBody.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 20)); }
                    if (direction ==0 && currentBody.transform.rotation.eulerAngles.z > 0  && currentBody.transform.position.x < cam_bounds_.x) { currentBody.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * 15)); }

                    MoveToPoint(currentBody.transform, EndPos, dash_velocity*damp_f, 1);
                    
                }
                if (currentBody.transform.position.x == max_dir)
                {
                    trackingTimer_time = 0;
                    dash_number++;
                    tracking = false;
                    damp_f = 1;
                    currentBody.transform.rotation = Quaternion.Euler(Vector3.zero);
                    if (enraged && dash_number > 2)
                    {
                        currentBody.transform.position = new Vector3(player.transform.position.x, cam_bounds_.y - init_pos.y / 2 + 100, init_wPos.z - init_pos.z);
                    }
                }
            }
        }

        /// FROM TOP TO BOTTOM
        else
        {
            if (!tracking)
            {
                if (currentBody.transform.position.y <= cam_bounds_.y - init_pos.y / 2 - 30)
                {
                    tracking = true;
                }
                else
                {
                    Vector3 EndPos = new Vector3(player.transform.position.x, cam_bounds_.y - init_pos.y / 2 - 30, init_wPos.z - init_pos.z);

                    float damp_f = 2.5f;
                    MoveToPoint(currentBody.transform, EndPos, damp_f, 1);
                }
            }

            if (tracking)
            {
                if (trackingTimer_time < trackingTimer)
                {
                    MoveToPoint(currentBody.transform, new Vector3(player.transform.position.x, cam_bounds_.y - init_pos.y / 2 - 30, init_wPos.z - init_pos.z), 1f, 5f, 1);
                    trackingTimer_time += Time.deltaTime;
                }
                else
                {
                    Vector3 EndPos = new Vector3(currentBody.transform.position.x, cam_bounds_.y - init_pos.y / 2 - 30, currentBody.transform.position.z);
                    float damp_f = 1;

                    MoveToPoint(currentBody.transform, new Vector3(currentBody.transform.position.x, floorLevelpos.position.y + 15, currentBody.transform.position.z), dash_velocity * damp_f, 1);
                }
                if (currentBody.transform.position.y == floorLevelpos.position.y + 15)
                {
                    IsStuck = true;
                    m_endOfEnragedChargeEvent.Invoke();
                    Atk2Finished = false;
                    canGoOffscreen = false;
                    tracking = false;

                    Debug.Log("end of enraged phase");
                }
            }
        }
    }
    private void MoveOnScreen(int dash_number)
    {
        MainBodyParts[(int)(BodyParts.INNER_RING)].SetActive(false);
        MainBodyParts[(int)(BodyParts.OUTER_RING)].SetActive(false);
        MainBodyParts[(int)(BodyParts.CORE)].SetActive(false);

        float prev_x = currentBody.transform.position.x;

        switch (dash_number)
        {
            default:break;
            case 1:
                Move_track_dash(0);
                break;
            case 2:
                Move_track_dash(1);
                

                break;
            case 3:
                if (enraged)
                {
                    Move_track_dash(2);
                }
                else
                {
                    Move_track_dash(0);
                if(currentBody.transform.rotation.eulerAngles.z < 15) { currentBody.transform.Rotate(new Vector3(0, 0, Time.deltaTime)); }

                }
                break;
            case 4:
                break;
        }

        if (dash_number > max_dash_number)
        {
            Atk2Finished = true;
        }
        float cur_x = currentBody.transform.position.x;
    }

    public void TransferBody(GameObject Body)
    {
        Vector3 cam_bounds_ = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        Vector3 init_wPos = transform.TransformPoint(init_pos);
        if (Body == Samu_bodies[1])
        {
            Body.transform.position = new Vector3(cam_bounds_.x + init_pos.x / 2 + 100, player.transform.position.y + 10, init_wPos.z - init_pos.z);
        }
        mainCircle.transform.position = Body.transform.position;
        eye_obj.transform.position = Body.transform.position;
        SamuLight.transform.position = Body.transform.position;
        currentBody.SetActive(false);
        Body.SetActiveRecursively(true);
        currentBody = Body;
    }

    public Samu_BigFireball[] ATK1()
    {
        prev_state = state;
        next_state = Anim_States.ATK1;
        changeState = true;
        Atk1_fireballs = new Samu_BigFireball[Atk1_fireball_init_pos.Length];

        return Atk1_fireballs;
    }
    public Samu_BigFireball[] ATK1VAR()
    {
        prev_state = state;
        next_state = Anim_States.ATK1VAR;
        changeState = true;
        Atk1var_fireballs = new Samu_BigFireball[Atk1var_fireball_init_pos.Length];

        return Atk1var_fireballs;
    }

    public void GoBackCenter()
    {
        prev_state = state;
        next_state = Anim_States.BACK2ORIGIN;
        changeState = true;
    }

    public void ATK2()
    {
        prev_state = state;
        next_state = Anim_States.ATK2;
        max_dash_number = 2;
        if (enraged) { max_dash_number = 3; }
        dash_number = 1;
        changeState = true;
        cameraOffscreenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        point = new Vector3(transform.TransformPoint(init_pos).x, cameraOffscreenPoint.y - init_pos.y, 500);
    }
    public void Stop()
    {
        prev_state = state;
        next_state = Anim_States.STOP;
        changeState = true;
    }
    public void MoveIdle()
    {
        prev_state = state;
        next_state = Anim_States.IDLE;
        changeState = true;
    }

    private void KillFB()
    {
        if (Atk1_fireballs.Length > 0)
        {
            for (int i = 0; i < Atk1_fireballs.Length; i++)
            {
                Atk1_fireballs[i].gameObject.SetActive(false);
            }
        }
        if (Atk1var_fireballs.Length > 0)
        {
            for (int i = 0; i < Atk1var_fireballs.Length; i++)
            {
                Atk1var_fireballs[i].gameObject.SetActive(false);
            }
        }
    }
    private void SummonFireballs(GameObject[] spawnPoints, Samu_BigFireball[] FBs)
    {
        if (changeState && !fireballsSummoned)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].transform.localScale.x < 1)
                {
                    spawnPoints[i].transform.localScale += new Vector3(Time.deltaTime * 1.5f, Time.deltaTime * 1.5f);
                    //Circles SFX
                    if (!spawnPoints[i].GetComponent<MagicCircle>().GetComponent<AudioSource>().isPlaying)
                    {
                        spawnPoints[i].GetComponent<MagicCircle>().PlaySound(); Debug.Log("Circle SFX played");
                    }
                    return;
                }
                if (FBs[i] == null)
                {
                    FBs[i] = Instantiate(fireball_pref, spawnPoints[i].transform.position, Quaternion.identity, atk1var_Fireball_posObj.transform);
                    FBs[i].transform.localScale = Vector3.zero;
                    return;
                }
                else
                {
                    FBs[i].transform.position = spawnPoints[i].transform.position;
                    FBs[i].gameObject.SetActive(true);

                }
            }

            for (int i = 0; i < FBs.Length; i++)
            {
                if (FBs[i].transform.localScale.x < 1.5f)
                {
                    FBs[i].transform.localScale += new Vector3(Time.deltaTime * 1.5f, Time.deltaTime * 1.5f);
                }
            }
            if (FBs[FBs.Length - 1].transform.localScale.x >= 1.5f)
            {
                //Play FB Spawn SFX
                FBs[FBs.Length - 1].GetComponent<Samu_BigFireball>().PlaySound();
                Debug.Log("FB SFX played");

                state = prev_state;
                changeState = false;
                fireballsSummoned = true;
                ThrowFireballs(FBs);
                m_endOfFireBallSummon.Invoke();
            }
        }
    }
    public void UnsummonCircles_1()
    {
        for (int i = 0; i < Atk1_fireball_init_pos.Length; i++)
        {
            if (Atk1_fireball_init_pos[i].transform.localScale.x > 0)
            {
                Atk1_fireball_init_pos[i].transform.localScale -= new Vector3(Time.deltaTime * 1.5f, Time.deltaTime * 1.5f);
                if (Atk1_fireball_init_pos[i].transform.localScale.x < 0) { Atk1_fireball_init_pos[i].transform.localScale = Vector3.zero; }
            }
            else { 
                m_unsummonCirclesEventAtk1.Invoke(); 
            }
        }

        if (Atk1_fireball_init_pos[Atk1_fireball_init_pos.Length - 1].transform.localScale == Vector3.zero && Atk1var_fireball_init_pos[Atk1_fireball_init_pos.Length - 1].transform.localScale == Vector3.zero)
        {
            fireballsSummoned = false;
        }
    }

    public void UnsummonCircles_2()
    {
        for (int i = 0; i < Atk1var_fireball_init_pos.Length; i++)
        {
            if (Atk1var_fireball_init_pos[i].transform.localScale.x > 0)
            {
                Atk1var_fireball_init_pos[i].transform.localScale -= new Vector3(Time.deltaTime * 1.5f, Time.deltaTime * 1.5f);
                if (Atk1var_fireball_init_pos[i].transform.localScale.x < 0) 
                { 
                    Atk1var_fireball_init_pos[i].transform.localScale = Vector3.zero; 
                }
            }else{
                m_unsummonCirclesEventAtk2.Invoke();
            }
        }
        if (Atk1_fireball_init_pos[Atk1_fireball_init_pos.Length - 1].transform.localScale == Vector3.zero && Atk1var_fireball_init_pos[Atk1_fireball_init_pos.Length - 1].transform.localScale == Vector3.zero)
        {
            fireballsSummoned = false;
        }
    }

    public void AnimationInputController()
    {
        if (state != next_state && changeState) { state = next_state; }
    }
    private bool NumberInRange(float num, float Range)
    {
        if (num < Range && num > -Range)
        { return true; }
        else { return false; }
    }
    private bool NumberInRange(float num, float PivotNum, float Range)
    {
        if (num < PivotNum + Range && num > PivotNum - Range)
        { return true; }
        else { return false; }
    }

    public bool playerInRange { set { playerOnSight = value; } }
    public float dash_Speed { set { dash_velocity = value; } }
    public GameObject player_ref { set { player = value; } }
    public Samu_BigFireball[] GetFireBalls() { return Atk1_fireballs; }
    public Samu_BigFireball[] GetFireBalls_1() { return Atk1var_fireballs; }

    public bool AreEyesAlive
    {
        set { m_areEyesAlive = value; }
        get { return m_areEyesAlive; }
    }

    public void SetEyesToAlive(){
        m_areEyesAlive = true;
        enraged = false;
        MainBodyParts[(int)(BodyParts.OUTER_RING)].GetComponent<SpriteRenderer>().forceRenderingOff = false;
        Body1Parts[(int)(BodyParts.OUTER_RING)].GetComponent<SpriteRenderer>().forceRenderingOff = false;
    }

    public bool Enraged{
        set { enraged = value;}
    }

    public void Reset(){
        UnsummonCircles_1();
        currentBody = Samu_bodies[(int)Bodies.MAIN];
        currentBody.transform.localPosition = init_pos;
        GoBackCenter();
        //state = Anim_States.STOP;
    }

}