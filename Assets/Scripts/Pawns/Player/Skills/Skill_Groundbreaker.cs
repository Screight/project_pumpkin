using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Groundbreaker : MonoBehaviour
{
    [SerializeField] SpellCooldown m_spellCooldownScript;
    float m_cooldown = 3.0f;
    [SerializeField] float m_maxSpeed;
    Player m_player;
    Rigidbody2D m_rb2D;

    [SerializeField] Animator m_impactAnimator;
    string m_groundbrekerImpactAnimationName = "impact";
    int m_animationHash;

    [SerializeField] LayerMask m_enemyLayer;

    bool m_isUsingGroundBreaker = false;
    Timer m_cooldownTimer;

    private void Awake()
    {
        m_cooldownTimer = gameObject.AddComponent<Timer>();
        m_player = GetComponent<Player>();
        m_rb2D = GetComponent<Rigidbody2D>();

    }

    private void Start() {
        m_cooldownTimer.Duration = m_cooldown;
        m_animationHash = Animator.StringToHash(m_groundbrekerImpactAnimationName); ;
    }

    private void Update()
    {
        if (m_cooldownTimer.IsRunning)
        {
            m_spellCooldownScript.FillGroundbreakerCooldownUI(m_cooldownTimer.CurrentTime / m_cooldownTimer.Duration);
        }
        else
        {
            m_spellCooldownScript.FillGroundbreakerCooldownUI(1);
        }
    }

    public void Groundbreaker(bool p_isUnlocked)
    {
        if (!p_isUnlocked) { return; }
        if (InputManager.Instance.Skill2ButtonPressed && m_cooldownTimer.IsFinished && !m_player.IsGrounded)
        {
            m_rb2D.velocity = new Vector2(0, m_maxSpeed);
            m_player.State = PLAYER_STATE.GROUNDBREAKER;
            m_player.ChangeAnimationState(PLAYER_ANIMATION.GROUNDBREAKER);
            m_rb2D.gravityScale = 0;
            Physics2D.IgnoreLayerCollision(6, 7, true);
            m_player.IsUsingGroundBreaker = true;
            m_isUsingGroundBreaker = true;
        }

        if (m_player.IsGrounded && m_isUsingGroundBreaker)
        {
            Collider2D[] enemiesInAttackRange = Physics2D.OverlapCircleAll(transform.position, 16, m_enemyLayer);
            foreach (Collider2D enemy in enemiesInAttackRange)
            {
                if (enemy.gameObject.tag == "enemy")
                {
                    enemy.gameObject.GetComponent<Enemy>().Damage(1);
                    float velocityX;
                    if (transform.position.x == enemy.transform.position.x) { velocityX = 0; }
                    else { velocityX = -50 * (transform.position - enemy.transform.position).normalized.x / Mathf.Abs((transform.position - enemy.transform.position).normalized.x); };
                    float velocityY = 100;
                    enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX, velocityY);
                }
            }
            SoundManager.Instance.PlayOnce(AudioClipName.GROUNDBREAKER);
            m_impactAnimator.Play(m_animationHash);
            m_isUsingGroundBreaker = false;
            m_player.IsUsingGroundBreaker = false;
            m_cooldownTimer.Run();
            Player.Instance.ChangeAnimationState(PLAYER_ANIMATION.IDLE);
            Player.Instance.State = PLAYER_STATE.IDLE;
        }
    }

    public void ResetGroundbreakerState() { m_isUsingGroundBreaker = false; }
}            