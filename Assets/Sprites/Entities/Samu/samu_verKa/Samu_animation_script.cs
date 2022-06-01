using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samu_animation_script : MonoBehaviour
{
    enum Anim_States { STOP,BACK2ORIGIN , IDLE, ATK1, ATK1VAR, ATK2};
    Anim_States state = Anim_States.STOP;
    Anim_States next_state = Anim_States.STOP;
    Anim_States prev_state = Anim_States.STOP;


    [SerializeField] GameObject innerRing;
    [SerializeField] GameObject outerRing;
    [SerializeField] GameObject mainCircle;
    [SerializeField] GameObject eye_obj;


    [SerializeField] GameObject[] Samu_bodies;
    
    [SerializeField] GameObject atk1_Fireball_pos_obj;
    [SerializeField] Samu_BigFireball fireball_pref;
    private GameObject[] Atk1_fireball_init_pos;
    public Samu_BigFireball[] Atk1_fireballs;

    [SerializeField] GameObject atk1var_Fireball_posObj;
    private GameObject[] Atk1var_fireball_init_pos;
    public Samu_BigFireball[] Atk1var_fireballs;


    private GameObject[] eyes;
    private GameObject[] eyes_init_pos;
    private GameObject player;
    Samu_BigFireball[] fb;


    Vector3 init_pos;
    float time;
    private bool playerOnSight = false;
    private bool canMove = true;
    private bool changeState = false;
    private bool fireballsSummoned = false;
    private bool fireballsThrown = false;

    private float trackingTimer = 3;
    private float trackingTimer_time = 0;
    private bool canGoOffscreen = false;
    Vector3 cameraOffscreenPoint;
    Vector3 point;


    float softening_movement_mod = 0.3f;

    private Vector3 rot_speed = new Vector3(0, 0, 0.3f);

    // Start is called before the first frame update
    void Start()
    {
       // 
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
        init_pos = gameObject.transform.localPosition;
        Samu_bodies[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ///ROTATION 
        float sin = Mathf.Cos(Time.time / 2) * 1.5f;
        //Debug.Log(sin);
        innerRing.transform.Rotate(new Vector3(0, 0, sin));
        outerRing.transform.Rotate(new Vector3(0, 0, -sin));
        eye_obj.transform.Rotate(new Vector3(0, 0, -sin));
        mainCircle.transform.Rotate(rot_speed);


        for (int i = 0; i < Atk1_fireball_init_pos.Length; i++)
        {
            Atk1_fireball_init_pos[i].transform.Rotate(rot_speed*4f);
        }

        for (int i = 0; i < Atk1var_fireball_init_pos.Length; i++)
        {
            Atk1var_fireball_init_pos[i].transform.Rotate(rot_speed*4f);
        }

        ///EYETRACKING
        for (int i = 0; i < eyes.Length; i++)
        {

            if (playerOnSight)
            {
                Vector3 offset = player.transform.position - eyes_init_pos[i].transform.position;
                Vector3 direction = Vector3.ClampMagnitude(offset, 1.5f);


                eyes[i].transform.localPosition = (new Vector3(eyes_init_pos[i].transform.position.x, eyes_init_pos[i].transform.position.y, 0) + new Vector3(eyes_init_pos[i].transform.position.x - direction.x, eyes_init_pos[i].transform.position.y - direction.y, 0) * -1);
            }
            else { eyes[i].transform.position = eyes_init_pos[i].transform.position; }
            eyes_init_pos[i].transform.Rotate(new Vector3(0, 0, sin));


        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ATK2();

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
                Debug.Log(x_pos);
                Debug.Log(y_pos);

                if (changeState)
                {
                    if (!(NumberInRange(x_pos, transform.localPosition.x, max_range) && NumberInRange(y_pos, transform.localPosition.y, max_range)))
                    {
                        Vector3 offset = transform.localPosition - (init_pos + new Vector3(x_pos , -y_pos , 0));
                        Vector3 direction = Vector3.ClampMagnitude(offset, 2.5f);
                        transform.localPosition += -direction;
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

                    transform.localPosition = motion; }
                  
                    break;

                case Anim_States.BACK2ORIGIN:
                if (transform.localPosition != init_pos)
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
                GoOffscreen();

                break;

            }
       


        if (fireballsSummoned && ((Atk1var_fireballs.Length == 0 || !Atk1var_fireballs[0].isActiveAndEnabled) && (Atk1_fireballs.Length == 0 || !Atk1_fireballs[0].isActiveAndEnabled)))
        {
            UnsummonCircles();
        }

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

        MoveToPoint(transform, init_pos, 2.5f);
        
        changeState = false;


    }
    private void GoOffscreen() {

        if (transform.localPosition == init_pos)
        {
            canGoOffscreen = true;
        }

        if (transform.localPosition != init_pos && !canGoOffscreen )
        {
            Back2Origin_f();
        }

        if (canGoOffscreen)
        {
             
            
            MoveToPoint(transform, point, 2.5f, 1);

        }

        if (transform.position.y >= cameraOffscreenPoint.y-init_pos.y && !Samu_bodies[1].activeSelf)
        {
            TransferBody();
        }
        
        if (Samu_bodies[1].activeSelf)
        {
            Vector3 cam_bounds_ = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            Vector3 init_wPos = transform.TransformPoint(init_pos);

            MoveToPoint(Samu_bodies[1].transform, new Vector3(cam_bounds_.x+init_pos.x/2-15, player.transform.position.y, init_wPos.z - init_pos.z), 1f,1);
            mainCircle.transform.position = Samu_bodies[1].transform.position;
            eye_obj.transform.position = Samu_bodies[1].transform.position;
            if(Samu_bodies[1].transform.position.x <= cam_bounds_.x + init_pos.x/2)
            {
                Debug.Log("AAA");
            }
        }

    }


    private void TransferBody()
    {
       
        Vector3 cam_bounds_ = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        Vector3 init_wPos = transform.TransformPoint(init_pos);
        Samu_bodies[1].transform.position = new Vector3(cam_bounds_.x + init_pos.x / 2 + 100, player.transform.position.y, init_wPos.z-init_pos.z) ;
        mainCircle.transform.position = Samu_bodies[1].transform.position;
        eye_obj.transform.position = Samu_bodies[1].transform.position;
        Samu_bodies[1].SetActive(true);
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
        changeState = true;
        cameraOffscreenPoint =  Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        point = new Vector3(transform.TransformPoint(init_pos).x-init_pos.x , cameraOffscreenPoint.y - init_pos.y, 500);

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

    public GameObject player_ref
    {
        set { player = value; }
    }
}