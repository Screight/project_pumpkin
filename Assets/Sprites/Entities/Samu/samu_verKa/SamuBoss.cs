using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SamuBoss : MonoBehaviour
{
    AudioSource m_source;

    private enum STATE { SUMMON_FIREBALLS, RETURN_TO_CENTER, MOVE_AROUND, FIRE_BALLS, BULLET_HELL, CHARGE_ATTACK, WEAK, RECOVER_FROM_WEAK, DEAD, LAST_NO_USE }

    private Samu_animation_script m_controller;
    private STATE m_state = STATE.LAST_NO_USE;
    private List<Samu_BigFireball> m_fireBalls;
    private Timer m_evenTimer;
    [SerializeField] private float m_timeBetweenFireBalls = 0.5f;
    [SerializeField] private float m_timeToMoveAround = 5.0f;
    private bool m_isInCenter = false;
    private bool m_isNextStateCharge = false;
    bool m_hasDoneBulletHell = false;

    [SerializeField] GameObject m_fireballPrefab;
    [SerializeField] float m_fireBall_1Speed = 50.0f;
    [SerializeField] float m_fireBallBulletHellSpeed = 100.0f;
    [SerializeField] float m_weakDuration = 5.0f;
    [SerializeField] Image m_healthBar;
    [SerializeField] float m_maxHealth;
    float m_health;
    [SerializeField] Attack m_attack;
    [SerializeField] GameObject[] m_eyes;

    public void StartFight()
    {
        m_healthBar.transform.parent.gameObject.SetActive(true);
        InitializeReturnToCenter();
        GameManager.Instance.IsPlayerInFinalBossFight = true;
        SoundManager.Instance.PlayBackground(BACKGROUND_CLIP.SAMAELTHEME);
    }

    public void Reset()
    {
        m_controller.Reset();
        m_state = STATE.LAST_NO_USE;
        m_health = m_maxHealth;
        m_healthBar.fillAmount = 1;
        m_healthBar.transform.parent.gameObject.SetActive(false);
        m_hasDoneBulletHell = false;
        m_isNextStateCharge = false;
        m_isInCenter = false;
    }

    public void Damage()
    {
        if (m_state == STATE.DEAD) { return; }
        m_health -= GameManager.Instance.PlayerAttackDamage;
        m_healthBar.fillAmount = m_health / m_maxHealth;
        if (m_health <= 0)
        {
            m_state = STATE.DEAD;
            Debug.Log("BOSS IS DEAD");
            m_controller.Stop();
        }
    }

    private void Awake()
    {
        m_evenTimer = gameObject.AddComponent<Timer>();
        m_source = GetComponent<AudioSource>();
        m_controller = GetComponent<Samu_animation_script>();
        m_health = m_maxHealth;
        m_healthBar.transform.parent.gameObject.SetActive(false);
    }

    private void Start()
    {
        m_attack.DamageSamuBossEvent.AddListener(Damage);
        m_fireBalls = new List<Samu_BigFireball>();
        m_controller.EndOfFireballSummonEvent.AddListener(ChooseFireballVariant);
        m_controller.ArriveToCenterEvent.AddListener(SetIsInCenterToTrue);
        m_controller.UnsummonCirclesEventAtk1.AddListener(InitializeMoveAround);
        m_controller.UnsummonCirclesEventAtk2.AddListener(InitializeMoveAround);
        m_controller.EyesDeadEvent.AddListener(InitializeReturnToCenter);
        m_controller.EndNormalChargesEvent.AddListener(InitializeReturnToCenter);
        m_controller.EndOfEnragedChargeEvent.AddListener(InitializeWeak);
        m_controller.FullyRecoveredEvent.AddListener(ExitRecoverFromWeak);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartFight();
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
            case STATE.BULLET_HELL:
                HandleBulletHell();
                break;
            case STATE.WEAK:
                HandleWeak();
                break;
            case STATE.RECOVER_FROM_WEAK:
                HandleRecoverFromWeak();
                break;
        }
    }

    void InitializeWeak()
    {
        m_state = STATE.WEAK;
        m_controller.Stop();
        m_evenTimer.Duration = m_weakDuration;
        m_evenTimer.Run();
    }

    void HandleWeak()
    {
        if (m_evenTimer.IsFinished)
        {
            InitializeRecoverFromWeak();
        }
    }

    void InitializeRecoverFromWeak()
    {
        m_state = STATE.RECOVER_FROM_WEAK;
    }

    void HandleRecoverFromWeak()
    {
        m_controller.GetUnstuck();
    }

    void ExitRecoverFromWeak()
    {
        if (m_evenTimer.IsFinished)
        {
            InitializeReturnToCenter();
            foreach (GameObject eye in m_eyes)
            {
                eye.SetActive(true);
            }
            m_controller.SetEyesToAlive();
            m_controller.Enraged = false;
        }
    }

    void InitializeSummonFireBalls()
    {
        m_state = STATE.SUMMON_FIREBALLS;
        m_controller.ATK1();
        m_isNextStateCharge = true;
    }

    void ChooseFireballVariant()
    {
        if (m_hasDoneBulletHell)
        {
            InitializeBulletHell();
            Debug.Log("BULLETHELL");
        }
        else
        {
            InitializeFireBalls();
            Debug.Log("FIREBALLS");
        }
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

    void InitializeSummonBulletHell()
    {
        m_state = STATE.SUMMON_FIREBALLS;
        m_controller.ATK1VAR();
        m_isNextStateCharge = true;
    }

    void InitializeBulletHell()
    {
        m_state = STATE.BULLET_HELL;
        foreach (Samu_BigFireball fireball in m_controller.GetFireBalls_1())
        {
            m_fireBalls.Add(fireball);
        }
        m_evenTimer.Duration = 1.0f;
        m_evenTimer.Run();
    }

    void InitializeFireBulletHell()
    {
        Vector2 direction = new Vector2();
        Samu_BigFireball[] fireballs = new Samu_BigFireball[4];

        for (int i = 0; i < m_fireBalls.Count; i++)
        {
            fireballs[0] = m_fireBalls[0];
            fireballs[1] = Instantiate(m_fireballPrefab, fireballs[0].transform.position, Quaternion.identity).GetComponent<Samu_BigFireball>();
            fireballs[2] = Instantiate(m_fireballPrefab, fireballs[0].transform.position, Quaternion.identity).GetComponent<Samu_BigFireball>();
            fireballs[3] = Instantiate(m_fireballPrefab, fireballs[0].transform.position, Quaternion.identity).GetComponent<Samu_BigFireball>();

            direction.x = 1;
            direction.y = 1;
            fireballs[0].Fire(direction, m_fireBallBulletHellSpeed);
            direction.x = -1;
            direction.y = 1;
            fireballs[1].Fire(direction, m_fireBallBulletHellSpeed);
            direction.x = -1;
            direction.y = -1;
            fireballs[2].Fire(direction, m_fireBallBulletHellSpeed);
            direction.x = 1;
            direction.y = -1;
            fireballs[3].Fire(direction, m_fireBallBulletHellSpeed);

            m_fireBalls.RemoveAt(0);
        }
        m_controller.UnsummonCircles_2();
    }

    void HandleBulletHell()
    {
        if (m_evenTimer.IsFinished)
        {
            InitializeFireBulletHell();
        }
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
            m_controller.UnsummonCircles_1();
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
        if (m_isInCenter)
        {
            if (!m_controller.AreEyesAlive)
            {
                InitializeChargeAttack();
            }
            else if (m_isNextStateCharge)
            {
                InitializeChargeAttack();
            }
            else
            {
                float healthPercentage = m_health / m_maxHealth;
                if (healthPercentage >= 0.5)
                {
                    InitializeSummonFireBalls();
                }
                else if (healthPercentage >= 0.25)
                {
                    if (m_hasDoneBulletHell)
                    {
                        InitializeSummonFireBalls();
                    }
                    else { InitializeSummonBulletHell(); }
                    m_hasDoneBulletHell = !m_hasDoneBulletHell;
                }
                else { InitializeSummonBulletHell(); }
            }
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