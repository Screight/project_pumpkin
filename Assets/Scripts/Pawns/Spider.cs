using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    private Rigidbody2D m_rb2d;
    private ENEMY_STATE m_state;
    [SerializeField] float m_speed;
    [SerializeField] Transform m_leftPatrolPoint;
    [SerializeField] Transform m_rightPatrolPoint;
    [SerializeField] ParticleSystem m_hatchParticles;
    [SerializeField] ParticleSystem m_deathParticles;
    [SerializeField] bool canPlayerActivateEggs = true;

    [SerializeField] float m_eggDuration = 2.0f;
    private Timer m_eventTimer;
    private Timer m_hissTimer;
    private float m_eclosionDuration;
    private bool m_hasHatched = false;
    private bool m_firtsHiss = false;
    private bool m_isDead = false;
    private AudioSource m_audioSrc;

    protected override void Awake()
    {
        base.Awake();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_audioSrc = GetComponent<AudioSource>();
        m_eventTimer = gameObject.AddComponent<Timer>();
        m_hissTimer = gameObject.AddComponent<Timer>();
    }

    protected override void Start()
    {
        base.Start();
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), true);
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
        m_hissTimer.Duration = 0.5f;
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

        //HISS SFX
        if (m_firtsHiss) { m_hissTimer.Run(); m_firtsHiss = false; }
        if (m_hasHatched && m_hissTimer.IsFinished && !m_audioSrc.isPlaying && !m_isDead) 
        {
            int randNum = Random.Range(0, 2);
            if (randNum == 0) { m_audioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SPIDER_HISS_1)); }
            else { m_audioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SPIDER_HISS_2)); }

            m_hissTimer.Duration = Random.Range(3, 5);
            m_hissTimer.Run();
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
        m_firtsHiss = true;
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.SPIDER_ECLOSION, false);Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), true);
        m_eventTimer.Duration = m_eclosionDuration;
        m_eventTimer.Restart();
        SoundManager.Instance.PlayOnce(AudioClipName.EGG_CRACK_1);
    }

    public void HandleEclosion()
    {
        if (m_eventTimer.IsFinished) {
            Hatch(); 
        }
    }

    public void Hatch()
    {
        m_hatchParticles.Play();
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), false);
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
        }
        else if (transform.position.x < m_leftPatrolPoint.transform.position.x)
        {
            transform.position = new Vector3(m_leftPatrolPoint.position.x, transform.position.y, transform.position.z);
            m_rb2d.velocity = new Vector2(-m_rb2d.velocity.x, m_rb2d.velocity.y);
        }
    }
    protected override void Die()
    {
        m_deathParticles.transform.position = transform.position;
        m_deathParticles.Play();
        m_hissTimer.Stop();
        m_audioSrc.Stop();
        m_isDead = true;
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
        m_isDead = false;
        m_rb2d.velocity = Vector2.zero;
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), true);
    }

    public bool CanHatch() { return !m_hasHatched && canPlayerActivateEggs; }
}