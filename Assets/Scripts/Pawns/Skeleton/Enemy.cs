using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AnimatedCharacter
{    Player m_playerScript;
    [SerializeField] float MAX_HEALTH = 3;
    [SerializeField] protected float m_health = 3;
    [SerializeField] protected int m_damage = 1;
    [SerializeField] ROOMS m_room; 
    protected Vector2 m_spawnPos;
    Timer m_animationTimer;
    protected float m_hitAnimationDuration;
    protected float m_dieAnimationDuration;
    protected bool m_isDying = false;
    bool m_isBeingHit = false;
    protected Collider2D m_collider;

    public bool m_isNewController = false;

    /// PLAYER COLLITION
    [SerializeField] float m_playerInvulnerableDuration = 0.5f;
    [SerializeField] float m_playerNoControlDuration = 0.5f;
    [SerializeField] Vector2 m_pushAwayPlayerVelocity = new Vector2(50.0f, 100.0f);
    Rigidbody2D m_rb2d;
    bool m_isBeingPushed = false;
    Timer m_pushedTimer;
    [SerializeField] float m_pushedDuration = 0.1f;
    [SerializeField] float m_pushedSpeed = 50.0f;

    protected bool m_isStateMachineActive = true;

    protected override void Awake() { 
        base.Awake();
        m_spawnPos = transform.position; 
        m_animationTimer = gameObject.AddComponent<Timer>();
        m_collider = GetComponent<Collider2D>();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_pushedTimer = gameObject.AddComponent<Timer>();
        m_pushedTimer.Duration = m_pushedDuration;
    }

    protected virtual void Start()
    {
        m_playerScript = Player.Instance;
        m_health = MAX_HEALTH;
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.IsGamePaused) { return; }

        if (m_isBeingPushed && m_pushedTimer.IsFinished)
        {
            m_isBeingPushed = false;
            m_rb2d.velocity = new Vector2(0, m_rb2d.velocity.y);
        }

        if (m_isNewController) { return; }
        else
        {
            if (m_isDying && m_animationTimer.IsFinished)
            {
                Die();
                m_isDying = false;
            }
            else if (m_isBeingHit && m_animationTimer.IsFinished)
            {
                EndHit();
                m_isBeingHit = false;
            }
        }

    }

    public virtual void Damage(float p_damage)
    {
        m_health -= p_damage;
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), true);

        if (m_health <= 0) {
            SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_KILL); 
            m_isDying = true;
            m_animationTimer.Duration = m_dieAnimationDuration;
            m_animationTimer.Stop();
            m_animationTimer.Run();
            m_rb2d.velocity = new Vector2(0, m_rb2d.velocity.y);
            m_isStateMachineActive = false;
            }
        else {
            SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_HIT); 
            m_isBeingHit = true;
            m_animationTimer.Duration = m_hitAnimationDuration;
            m_animationTimer.Stop();
            m_animationTimer.Run();
            m_isBeingPushed = true;
            m_pushedTimer.Run();

            Vector2 velocity = new Vector2();

            if(Player.Instance.transform.position.x < transform.position.x){
                velocity.x = m_pushedSpeed;
            }
            else{
                velocity.x = -m_pushedSpeed;
            }

        m_rb2d.velocity = velocity;

        }
    }

    public virtual void EndHit(){
        if (!m_isDying)
        {
            m_isStateMachineActive = true;
        }
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), false);
    }

    protected virtual bool OnCollisionStay2D(Collision2D p_collider)
    {
        if (p_collider.gameObject.tag == "Player" && !Player.Instance.IsInvulnerable && Player.Instance.CanPlayerGetHit())
        {
            float distanceToEnemyX = p_collider.gameObject.transform.position.x - transform.position.x;
            //float distanceToEnemyY = p_collider.gameObject.transform.position.y - transform.position.y;
            float distanceToEnemyY = 1;
            Vector2 direction = new Vector2(distanceToEnemyX/Mathf.Abs(distanceToEnemyX), distanceToEnemyY/Mathf.Abs(distanceToEnemyY));
            PushPlayer(direction);
            return false;
        }
        return true;
    }

    protected virtual void PushPlayer(Vector2 p_direction)
    {
        Player.Instance.HandleHostileCollision(m_pushAwayPlayerVelocity, p_direction, m_playerNoControlDuration, m_playerInvulnerableDuration, m_damage);
    }

    public virtual void Reset()
    {
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), false);
        transform.position = new Vector3(m_spawnPos.x, m_spawnPos.y, CameraManager.Instance.MainSceneDepth);
        m_health = MAX_HEALTH;
        m_animationTimer.Stop();
        m_isDying = false;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    protected virtual void Die()
    {
        m_isStateMachineActive = false;
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), false);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public ROOMS Room { get { return m_room;}}
    public bool IsDying{ get { return m_isDying;}}
}