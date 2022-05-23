using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    Rigidbody2D m_rb2d;
    ENEMY_STATE m_state;
    [SerializeField] float m_speed;
    [SerializeField] Transform m_leftPatrolPoint;
    [SerializeField] Transform m_rightPatrolPoint;
    [SerializeField] ParticleSystem m_hatchParticles;
    [SerializeField] ParticleSystem m_deathParticles;
    [SerializeField] bool canPlayerActivateEggs = true;

    [SerializeField] float m_eggDuration = 2.0f;
    Timer m_eventTimer;
    float m_eclosionDuration;
    bool m_hasHatched = false;
    bool m_canDamagePlayer = false;

    protected override void Awake()
    {
        base.Awake();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_eventTimer = gameObject.AddComponent<Timer>();

    }

    protected override void Start()
    {
        base.Start();

        if (m_leftPatrolPoint.position.x > m_rightPatrolPoint.transform.position.x)
        {
            Transform provitional = m_leftPatrolPoint;
            m_leftPatrolPoint = m_rightPatrolPoint;
            m_rightPatrolPoint = provitional;
        }
        InitializeEggState();
        m_collider.enabled = false;
        m_rb2d.gravityScale = 0;
        m_eclosionDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.SPIDER_ECLOSION);
    }

    protected override void Update()
    {
        base.Update();

        if (m_rb2d.velocity.x > 0 && transform.localScale.x < 0 || m_rb2d.velocity.x < 0 && transform.localScale.x > 0)
        {
            FlipX();
        }

        switch (m_state)
        {
            default: break;
            case ENEMY_STATE.PATROL:
                { HandlePatrol(); }
                break;
            case ENEMY_STATE.EGG:
                { HandleEgg(); }
                break;
            case ENEMY_STATE.ECLOSION:
                { HandleEclosion(); }
                break;
        }
    }

    public void InitializeEggState()
    {
        m_state = ENEMY_STATE.EGG;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_EGG, false);
        if (!canPlayerActivateEggs && m_eggDuration > 0)
        {
            m_eventTimer.Duration = m_eggDuration;
            m_eventTimer.Run();
        }
    }

    void HandleEgg()
    {
        if (!canPlayerActivateEggs && m_eggDuration > 0 && m_eventTimer.IsFinished)
        {
            InitializeEclosion();
        }
    }

    public void InitializeEclosion()
    {
        m_state = ENEMY_STATE.ECLOSION;
        m_collider.enabled = true;
        m_hasHatched = true;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_ECLOSION, false);
        m_eventTimer.Duration = m_eclosionDuration;
        m_eventTimer.Restart();
        SoundManager.Instance.PlayOnce(AudioClipName.EGG_CRACK_1);
    }

    public void HandleEclosion()
    {
        if (m_eventTimer.IsFinished) { Hatch(); }
    }

    public void Hatch()
    {
        m_hatchParticles.Play();
        m_canDamagePlayer = true;
        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        m_rb2d.gravityScale = 1;
        InitializePatrol();
    }

    void InitializePatrol()
    {
        m_state = ENEMY_STATE.PATROL;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_WALK, false);

        bool startLeft = false;
        float randomNumber = Random.Range(0, 2);

        if (randomNumber == 0) { startLeft = true; }

        if (startLeft)
        {
            m_rb2d.velocity = new Vector2(-m_speed, 0);
            FlipX();
        }
        else { m_rb2d.velocity = new Vector2(m_speed, 0); }
    }

    void HandlePatrol()
    {
        if (transform.position.x > m_rightPatrolPoint.transform.position.x)
        {
            transform.position = new Vector3(m_rightPatrolPoint.position.x, transform.position.y, transform.position.z);
            m_rb2d.velocity = new Vector2(-m_rb2d.velocity.x, m_rb2d.velocity.y);
            //FlipX();
        }
        else if (transform.position.x < m_leftPatrolPoint.transform.position.x)
        {
            transform.position = new Vector3(m_leftPatrolPoint.position.x, transform.position.y, transform.position.z);
            m_rb2d.velocity = new Vector2(-m_rb2d.velocity.x, m_rb2d.velocity.y);
            //FlipX();
        }
    }
    protected override void Die()
    {
        m_deathParticles.transform.position = transform.position;
        m_deathParticles.Play();
        base.Die();
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public override void Reset()
    {
        base.Reset();
        InitializeEggState();
        m_collider.enabled = false;
        m_rb2d.gravityScale = 0;
        m_hasHatched = false;
        m_rb2d.velocity = Vector2.zero;
        m_canDamagePlayer = false;
    }

    public bool CanHatch() { return !m_hasHatched && canPlayerActivateEggs; }
}