using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBall : AnimatedCharacter
{

    [SerializeField] float m_speed;
    [SerializeField] Vector2 m_pushAwayVelocity = new Vector2(0,0);
    [SerializeField] float m_noControlDuration = 0.4f;
    [SerializeField] float m_invulnerableDuration = 0.2f;
    [SerializeField] int m_damage;
    Rigidbody2D m_rb2D;
    Vector2 m_direction;
    Timer m_eventTimer;
    float m_explositonDuration;
    bool m_hasExploded = false;

    protected override void Awake() {
        base.Awake();
        m_rb2D = GetComponent<Rigidbody2D>();
        m_eventTimer = gameObject.AddComponent<Timer>();
    }

    private void Update() {
        if(m_eventTimer.IsFinished && m_hasExploded){
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        m_rb2D.velocity = new Vector2(m_direction.x * m_speed, m_direction.y * m_speed);
        m_explositonDuration = AnimationManager.Instance.GetClipDuration(this, ANIMATION.EFFECT_ACID_EXPLOSION);
        m_eventTimer.Duration = m_explositonDuration;
    }

    public void Initialize(Vector2 p_direction){
        m_direction = p_direction;
    }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag != "Player" && p_collider.tag != "floor" || m_hasExploded){ return ;}

        if(p_collider.tag == "Player"){
            Vector2 direction = new Vector2(1,1);
        if(Player.Instance.transform.position.x < transform.position.x){
            direction.x = -1;
        }
        else{ direction.x = 1; }
        Player.Instance.HandleHostileCollision(m_pushAwayVelocity, direction,m_noControlDuration, m_invulnerableDuration, m_damage);
        }

        AnimationManager.Instance.PlayAnimation(this, ANIMATION.EFFECT_ACID_EXPLOSION, false);
        m_hasExploded = true;
        m_eventTimer.Run();
        m_rb2D.velocity = Vector2.zero;
        SoundManager.Instance.PlayOnce(AudioClipName.ENEMY_HIT);
    }
}
