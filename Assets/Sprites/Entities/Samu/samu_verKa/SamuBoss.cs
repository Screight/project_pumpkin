using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuBoss : MonoBehaviour
{
    AudioSource m_source;
    enum STATE { SUMMON_FIREBALLS,RETURN_TO_CENTER, MOVE_AROUND, FIRE_BALLS, BULLET_HELL, CHARGE_ATTACK, LAST_NO_USE }

    [SerializeField] Samu_animation_script m_controller;

    STATE m_state = STATE.LAST_NO_USE;
    List<Samu_BigFireball> m_fireBalls;
    Timer m_evenTimer;
    float m_timeBetweenFireBalls = 0.5f;
    float m_timeToMoveAround = 5.0f;
    bool m_isInCenter = false;
    bool m_isNextStateCharge = false;

    [SerializeField] float m_fireBall_1Speed = 50.0f;

    private void Awake() 
    {
        m_evenTimer = gameObject.AddComponent<Timer>();
        m_source = GetComponent<AudioSource>();
    }

    private void Start() 
    {
        m_fireBalls = new List<Samu_BigFireball>();
        m_controller.EndOfFireballSummonEvent.AddListener(InitializeFireBalls);
        m_controller.ArriveToCenterEvent.AddListener(SetIsInCenterToTrue);
        m_controller.UnsummonCirclesEventAtk1.AddListener(InitializeMoveAround);
        m_controller.EyesDeadEvent.AddListener(InitializeReturnToCenter);
        m_controller.EndNormalChargesEvent.AddListener(InitializeReturnToCenter);
    }
    bool test = false;
    private void Update()
    {
        if (!test)
        {
            //InitializeSummonFireBalls();
            test = true;
        }

        switch (m_state)
        {
            case STATE.FIRE_BALLS:
                HandleFireBalls();
                break;
            case STATE.MOVE_AROUND:
                HandleMoveAround();
                break;
            case STATE.RETURN_TO_CENTER:
                HandleReturnToCenter();
                break;
            case STATE.CHARGE_ATTACK:
                HandleChargeAttack();
                break;
        }
    }

    void InitializeSummonFireBalls()
    {
        m_state = STATE.SUMMON_FIREBALLS;
        m_controller.ATK1();
        m_isNextStateCharge = true;
    }

    void InitializeFireBalls()
    {
        m_state = STATE.FIRE_BALLS;
        foreach (Samu_BigFireball fireball in m_controller.GetFireBalls())
        {
            m_fireBalls.Add(fireball);
        }
        m_evenTimer.Duration = m_timeBetweenFireBalls;
        m_evenTimer.Run();
    }

    void HandleFireBalls()
    {
        int numberOfFireballs = m_fireBalls.Count;

        if (m_evenTimer.IsFinished && numberOfFireballs > 0)
        {
            m_fireBalls[0].FireToPlayer(m_fireBall_1Speed);
            m_fireBalls.RemoveAt(0);
            m_evenTimer.Run();
        }
        else if (numberOfFireballs == 0)
        {
            m_controller.UnsummonCircles();
        }
    }

    void InitializeMoveAround()
    {
        m_state = STATE.MOVE_AROUND;
        m_evenTimer.Duration = m_timeToMoveAround;
        m_evenTimer.Run();
        m_controller.MoveIdle();
    }

    void HandleMoveAround()
    {
        if (m_evenTimer.IsFinished)
        {
            InitializeReturnToCenter();
        }
    }

    void InitializeReturnToCenter()
    {
        m_state = STATE.RETURN_TO_CENTER;
        m_controller.GoBackCenter();
    }

    void HandleReturnToCenter()
    {
        if (m_evenTimer.IsFinished && m_isInCenter)
        {
            if (!m_controller.AreEyesAlive)
            {
                InitializeChargeAttack();
            }
            else if (m_isNextStateCharge)
            {
                InitializeChargeAttack();
            }
            else { InitializeSummonFireBalls(); }
            m_isInCenter = false;
        }
    }
    void InitializeChargeAttack()
    {
        m_controller.ATK2();
        m_isNextStateCharge = false;
    }
    void HandleChargeAttack()
    {

    }
    void SetIsInCenterToTrue()
    {
        m_isInCenter = true;
    }
}