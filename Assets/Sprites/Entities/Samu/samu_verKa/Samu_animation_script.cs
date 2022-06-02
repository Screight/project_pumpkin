using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samu_animation_script : MonoBehaviour
{
    enum Anim_States { STOP,BACK2ORIGIN , IDLE, ATK1, ATK1VAR, ATK2};
    enum BodyParts { CORE, INNER_RING , OUTER_RING}
    enum Bodies { MAIN, BODY1}
    Anim_States state = Anim_States.STOP;
    Anim_States next_state = Anim_States.STOP;
    Anim_States prev_state = Anim_States.STOP;


    //[SerializeField] GameObject innerRing;
    //[SerializeField] GameObject outerRing;
    //[SerializeField] GameObject core;
    [SerializeField] GameObject mainCircle;
    [SerializeField] GameObject eye_obj;


    [SerializeField] GameObject[] Samu_bodies;
    [SerializeField] GameObject[] MainBodyParts;
    [SerializeField] GameObject[] Body1Parts;
    
    
        
    [SerializeField] GameObject atk1_Fireball_pos_obj;
    [SerializeField] Samu_BigFireball fireball_pref;
    private GameObject[] Atk1_fireball_init_pos;
    public Samu_BigFireball[] Atk1_fireballs;

    [SerializeField] GameObject atk1var_Fireball_posObj;
    private GameObject[] Atk1var_fireball_init_pos;
    public Samu_BigFireball[] Atk1var_fireballs;


    private GameObject currentBody;
    private GameObject[] eyes;
    private GameObject[] eyes_init_pos;
    private GameObject player;
    Samu_BigFireball[] fb;


    Vector3 init_pos;
    float time;
    private float dash_velocity = 5f;
    private int max_dash_number = 2;
    private int dash_number = 1;
    private int dir = 0;

    private bool playerOnSight = false;
    private bool canMove = true;
    private bool changeState = false;
    private bool fireballsSummoned = false;
    private bool fireballsThrown = false;

    private float trackingTimer = 1;
    private float trackingTimer_time = 0;
    private bool tracking = false;
    private bool canGoOffscreen = false;
    private bool Atk2Finished = false;
    private bool enraged = false;


    Vector3 cam_bounds_;
    Vector3 init_wPos ;

    Vector3 cameraOffscreenPoint;
    Vector3 point;


    float softening_movement_mod = 0.3f;

    private Vector3 rot_speed = new Vector3(0, 0, 0.3f);

    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        ///ROTATION 
        float sin = Mathf.Cos(Time.time / 2) * 1.5f;
        //Debug.Log(sin);
        if (currentBody == Samu_bodies[((int)(Bodies.MAIN))] )
        {
            MainBodyParts[(int)(BodyParts.INNER_RING)].transform.Rotate(new Vector3(0, 0, sin));
            MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.Rotate(new Vector3(0, 0, -sin));
        }
        

        if (!Samu_bodies[1].activeSelf)
        {
            eye_obj.transform.rotation = new Quaternion(MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.x, MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.y, MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.z, MainBodyParts[(int)(BodyParts.OUTER_RING)].transform.rotation.w);
        }
        else
        {
            eye_obj.transform.rotation = new Quaternion(Samu_bodies[1].transform.rotation.x, Samu_bodies[1].transform.rotation.y, Samu_bodies[1].transform.rotation.z, -Samu_bodies[1].transform.rotation.w);
        }


        mainCircle.transform.Rotate(rot_speed);


        for (int i = 0; i < Atk1_fireball_init_pos.Length; i++)
        {
            Atk1_fireball_init_pos[i].transform.Rotate(rot_speed*4f);
        }

        for (int i = 0; i < Atk1var_fireball_init_pos.Length; i++)
        {
            Atk1var_fireball_init_pos[i].transform.Rotate(rot_speed*4f);
        }
        int eyes_alive = eyes.Length;
        ///EYETRACKING
        for (int i = 0; i < eyes.Length; i++)
        {
            if (!eyes[i].activeSelf)
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
        if (eyes_alive == 0) { enraged = true; }
        else { enraged =false; }

        cam_bounds_ = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        init_wPos = transform.TransformPoint(init_pos);
        //INPUT PROVISIONAL
        if (Input.GetKeyDown(KeyCode.E))
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

        }
       
        AnimationInputController();
        
        
            //MOVEMENT
            switch (state)
            {
                case Anim_States.IDLE:
                
                time += Time.deltaTime;
                    float x_pos = Mathf.Cos(time) * 200;
                    float y_pos = Mathf.Sin(time * 2f) * 60;
                int max_range = 2;
                //Debug.Log(x_pos);
               // Debug.Log(y_pos);

                if (changeState)
                {
                    if (!(NumberInRange(x_pos, currentBody.transform.localPosition.x, max_range) && NumberInRange(y_pos, currentBody.transform.localPosition.y, max_range)))
                    {
                        Vector3 offset = currentBody.transform.localPosition - (init_pos + new Vector3(x_pos , -y_pos , 0));
                        Vector3 direction = Vector3.ClampMagnitude(offset, 2.5f);
                        currentBody.transform.localPosition += -direction;
                        canMove = false; }

                    else{
                        softening_movement_mod = 0.3f;
                            changeState = false;
                            canMove = true;
                        }
                }
                if (canMove)
                { 
                    if(softening_movement_mod < 1) { softening_movement_mod += Time.deltaTime;  }

                    Vector3 motion = init_pos + new Vector3(x_pos* softening_movement_mod, -y_pos* softening_movement_mod, 0);

                    currentBody.transform.localPosition = motion; }
                  
                    break;

                case Anim_States.BACK2ORIGIN:
                if (currentBody.transform.localPosition != init_pos)
                {
                    Back2Origin_f();
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
                    GoOffscreen();
                }
                else
                {
                    if (currentBody != Samu_bodies[(int)(Bodies.MAIN)]) { TransferBody(Samu_bodies[(int)(Bodies.MAIN)]); }
                    Back2Origin_f();
                }
                if (currentBody.transform.localPosition == init_pos && Atk2Finished)
                {
                    Atk2Finished = false;
                    canGoOffscreen = false;
                    state = prev_state; 
                }
                    break;

            }
       


        if (fireballsSummoned && ((Atk1var_fireballs.Length == 0 || !Atk1var_fireballs[0].isActiveAndEnabled) && (Atk1_fireballs.Length == 0 || !Atk1_fireballs[0].isActiveAndEnabled)))
        {
            UnsummonCircles();
        }
        mainCircle.transform.position = currentBody.transform.position;
        eye_obj.transform.position = currentBody.transform.position;
        Debug.Log(state);
    }
    private void ThrowFireballs(Samu_BigFireball[] FBs)
    {



    }

    private void MoveToPoint (Transform entity, Vector3 WhereTo, float Magnitude, int Mode = 0)
    {
        //Local Position
        if (Mode == 0)
        {

            Vector3 offset = entity.localPosition - WhereTo;
            Vector3 direction = Vector3.ClampMagnitude(offset, Magnitude);
            entity.localPosition += -direction;
        }
        else {
            Vector3 offset = entity.position - WhereTo;
            Vector3 direction = Vector3.ClampMagnitude(offset, Magnitude);
            entity.position += -direction;
        }
    }


    private void Back2Origin_f() {

        MoveToPoint(currentBody.transform, init_pos, 2.5f);
        
        changeState = false;
       
    }
    private void GoOffscreen() {

        if (currentBody.transform.localPosition == init_pos)
        {
            canGoOffscreen = true;
        }

        if (currentBody.transform.localPosition != init_pos && !canGoOffscreen )
        {
            Back2Origin_f();
        }

        if (canGoOffscreen && Samu_bodies[(int)(Bodies.MAIN)].activeSelf)
        {
             
            
            MoveToPoint(currentBody.transform, point, 2.5f, 1);

        }

        if (currentBody.transform.position.y >= cameraOffscreenPoint.y-init_pos.y && !Samu_bodies[1].activeSelf)
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

                        float damp_f = 1f;

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

                        float damp_f = 1f;

                        MoveToPoint(currentBody.transform, EndPos, damp_f, 1);
                    }
                }
            }






            if (tracking)
            {
                if (trackingTimer_time < trackingTimer)
                {
                    MoveToPoint(currentBody.transform, new Vector3(dir, player.transform.position.y + 10, init_wPos.z - init_pos.z), 1.5f, 1);
                    currentBody.transform.position = new Vector3(dir, currentBody.transform.position.y, currentBody.transform.position.z);
                    trackingTimer_time += Time.deltaTime;

                }
                else
                {
                    Vector3 EndPos = new Vector3(max_dir, currentBody.transform.position.y, currentBody.transform.position.z);
                    float damp_f = 1;


                    MoveToPoint(currentBody.transform, new Vector3(max_dir, currentBody.transform.position.y, currentBody.transform.position.z), dash_velocity * damp_f, 1);
                }
                if (currentBody.transform.position.x == max_dir)
                {
                    trackingTimer_time = 0;
                    dash_number++;
                    tracking = false;

                }
            }
        }
        else
        {


        }





    }
    private void MoveOnScreen(int dash_number) {

        MainBodyParts[(int)(BodyParts.INNER_RING)].SetActive(false);
        MainBodyParts[(int)(BodyParts.OUTER_RING)].SetActive(false);
        MainBodyParts[(int)(BodyParts.CORE)].SetActive(false);
      
        float prev_x = currentBody.transform.position.x;


        switch (dash_number)
        {
            case 1:

                Move_track_dash(0);

                break;
               
            case 2:
                Move_track_dash(1);

                break;
                
            case 3:
                if (enraged) { Move_track_dash(2); }
                else
                {
                    Move_track_dash(0);
                }
                break;
            case 4:
                break;

        }

        if (dash_number > max_dash_number) { Atk2Finished = true; }
        float cur_x = currentBody.transform.position.x;


    }

     private void TransferBody(GameObject Body )
    {
        Vector3 cam_bounds_ = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        Vector3 init_wPos = transform.TransformPoint(init_pos);
        if (Body == Samu_bodies[1])
        {
            Body.transform.position = new Vector3(cam_bounds_.x + init_pos.x / 2 + 100, player.transform.position.y+10, init_wPos.z - init_pos.z);
        }
        mainCircle.transform.position = Body.transform.position;
        eye_obj.transform.position = Body.transform.position;
        currentBody.SetActive(false);
        Body.SetActiveRecursively(true);
        currentBody = Body;

    }

    public Samu_BigFireball[] ATK1 () {

        prev_state = state;
        next_state = Anim_States.ATK1;
        changeState = true;
       Atk1_fireballs = new Samu_BigFireball[Atk1_fireball_init_pos.Length];
        
        return Atk1_fireballs;

    }
    public Samu_BigFireball[] ATK1VAR () {

        prev_state = state;
        next_state = Anim_States.ATK1VAR;
        changeState = true;
       Atk1var_fireballs = new Samu_BigFireball[Atk1var_fireball_init_pos.Length];
        
        return Atk1var_fireballs;

    }
    
    public void GoBackCenter () {

        prev_state = state;
        next_state = Anim_States.BACK2ORIGIN;
        changeState = true;
        

    }
    
    public void ATK2 () {

        prev_state = state;
        next_state = Anim_States.ATK2;
        max_dash_number = 2;
        if (enraged) { max_dash_number = 3; }
        changeState = true;
        cameraOffscreenPoint =  Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        point = new Vector3(transform.TransformPoint(init_pos).x , cameraOffscreenPoint.y - init_pos.y, 500);

    }
    public void Stop () {

        prev_state = state;
        next_state = Anim_States.STOP;
        changeState = true;
        

    }
    public void MoveIdle () {

        prev_state = state;
        next_state = Anim_States.IDLE;
        changeState = true;
        

    }

    private void KillFB() {
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
                    spawnPoints[i].transform.localScale += new Vector3(Time.deltaTime*1.5f, Time.deltaTime*1.5f);

                    return;    

                }
                if (FBs[i] == null)
                {
                    FBs[i] = Instantiate(fireball_pref, spawnPoints[i].transform.position, Quaternion.identity, atk1var_Fireball_posObj.transform);
                    FBs[i].transform.localScale = Vector3.zero;
                    return;
                }else
                {
                    FBs[i].transform.position = spawnPoints[i].transform.position;
                    FBs[i].gameObject.SetActive(true);

                }

            }

            for (int i = 0; i < FBs.Length; i++)
            {
                if (FBs[i].transform.localScale.x < 1.5f)
                {
                    FBs[i].transform.localScale += new Vector3(Time.deltaTime*1.5f, Time.deltaTime*1.5f);
                }

            }
            if (FBs[FBs.Length - 1].transform.localScale.x >= 1.5f)
            {
                state = prev_state;
                changeState = false;
                fireballsSummoned = true;
                ThrowFireballs(FBs);
            }
        }
       
    }
    private void UnsummonCircles()
    {
            for (int i = 0; i < Atk1_fireball_init_pos.Length; i++)
            {
                if (Atk1_fireball_init_pos[i].transform.localScale.x > 0)
                {
                    Atk1_fireball_init_pos[i].transform.localScale -= new Vector3(Time.deltaTime * 1.5f, Time.deltaTime * 1.5f);
                    if (Atk1_fireball_init_pos[i].transform.localScale.x < 0) { Atk1_fireball_init_pos[i].transform.localScale = Vector3.zero; }
                }

            }

            for (int i = 0; i < Atk1var_fireball_init_pos.Length; i++)
            {
                if (Atk1var_fireball_init_pos[i].transform.localScale.x > 0)
                {
                    Atk1var_fireball_init_pos[i].transform.localScale -= new Vector3(Time.deltaTime * 1.5f, Time.deltaTime * 1.5f);
                    if (Atk1var_fireball_init_pos[i].transform.localScale.x < 0) { Atk1var_fireball_init_pos[i].transform.localScale = Vector3.zero; }
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
        if (num < Range && num> -Range)
        { return true; }
        else { return false; }
    }
     private bool NumberInRange(float num, float PivotNum,float Range)
    {
        if (num < PivotNum+Range && num> PivotNum-Range)
        { return true; }
        else { return false; }
    }
    


    public bool playerInRange
    {
        set { playerOnSight = value; }
    }
    public float dash_Speed
    {
        set { dash_velocity = value; }
    }

    public GameObject player_ref
    {
        set { player = value; }
    }
}