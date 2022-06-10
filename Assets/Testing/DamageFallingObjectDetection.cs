using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFallingObjectDetection : InteractiveItem
{
    [SerializeField] Projectile m_projectile;
    [SerializeField] float m_speed = 40.0f;
    [SerializeField] Vector2 m_direction;
    [SerializeField] float m_reactionTime = 0.5f;
    Timer m_eventTimer;
    bool m_hasBeenShoot = false;

    protected override void Awake() {
        base.Awake();
        m_eventTimer = gameObject.AddComponent<Timer>();
        m_eventTimer.Duration = m_reactionTime;
    }

    protected override void Update() {
        base.Update();
        if(m_hasBeenUsed && m_eventTimer.IsFinished && !m_hasBeenShoot){

            m_hasBeenShoot = true;
            if(m_direction == Vector2.zero){
                m_direction = (Player.Instance.transform.position - m_projectile.gameObject.transform.position).normalized;
            }
            m_projectile.Shoot(m_direction.normalized);
        }
    }

    protected override void HandleInteraction()
    {
        base.HandleInteraction();
        m_eventTimer.Run();
    }
}
