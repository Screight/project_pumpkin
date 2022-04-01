using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator m_animator;
    Player m_playerScript;
    [SerializeField] float MAX_HEALTH = 3;
    [SerializeField] protected float m_health = 3;
    [SerializeField] protected int m_damage = 1;
    [SerializeField] ROOMS m_room; 
    protected Vector2 m_spawnPos;
    Timer m_dieTimer;
    bool m_isDying = false;
    protected Collider2D m_collider;

    /// PLAYER COLLITION
    [SerializeField] float m_playerInvulnerableDuration = 0.5f;
    [SerializeField] float m_playerNoControlDuration = 0.5f;
    [SerializeField] Vector2 m_pushAwayPlayerVelocity = new Vector2(50.0f, 100.0f);
    protected virtual void Awake() { 
        m_spawnPos = transform.position; 
        m_dieTimer = gameObject.AddComponent<Timer>();
        m_collider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        m_playerScript = Player.Instance;
        m_health = MAX_HEALTH;
        
        foreach(AnimationClip animationClip in m_animator.runtimeAnimatorController.animationClips)
        {
            if(animationClip.name == "Die"){
                m_dieTimer.Duration = animationClip.length;
            }
        }
    }

    protected virtual void Update() {
        if(m_isDying && m_dieTimer.IsFinished){
            Die();
            m_isDying = false;
        }
    }

    public virtual void Damage(float p_damage)
    {
        m_health -= p_damage;
        Physics2D.IgnoreCollision(m_collider, Player.Instance.GetCollider(), true);
        if (m_health <= 0) {
            SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_KILL); 
            m_isDying = true;
            m_dieTimer.Run();
            }
        else { SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_HIT); }
    }
    protected virtual void OnCollisionStay2D(Collision2D p_collider)
    {
        if (p_collider.gameObject.tag == "Player" && !m_playerScript.IsInvulnerable && Player.Instance.CanPlayerGetHit())
        {
            float distanceToEnemyX = p_collider.gameObject.transform.position.x - transform.position.x;
            //float distanceToEnemyY = p_collider.gameObject.transform.position.y - transform.position.y;
            float distanceToEnemyY = 1;
            Vector2 direction = new Vector2(distanceToEnemyX/Mathf.Abs(distanceToEnemyX), distanceToEnemyY/Mathf.Abs(distanceToEnemyY));
            PushPlayer(direction);
        }
    }

    protected virtual void PushPlayer(Vector2 p_direction)
    {
        m_playerScript.HandleHostileCollision(m_pushAwayPlayerVelocity, p_direction, m_playerNoControlDuration, m_playerInvulnerableDuration, m_damage);
    }

    public virtual void Reset()
    {
        transform.position = new Vector3(m_spawnPos.x, m_spawnPos.y, CameraManager.Instance.MainSceneDepth);
        m_health = MAX_HEALTH;
        m_dieTimer.Stop();
        m_isDying = false;
    }

    void Die(){
        this.gameObject.SetActive(false);
        
    }

    public ROOMS Room { get { return m_room;}}
    public bool IsDying{ get { return m_isDying;}}
}