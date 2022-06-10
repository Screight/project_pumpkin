using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    Rigidbody2D m_rb2d;
    [SerializeField] ParticleSystem m_movingParticles;
    [SerializeField] ParticleSystem m_stillParticles;
    [SerializeField] float m_speed;
    SpriteRenderer m_renderer;
    bool m_isFacingRight = false;
    bool m_isMoving = false;
    private void Awake() {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        var emission = m_movingParticles.emission;
        emission.rateOverTimeMultiplier = 0;
    }

    Vector2 m_workVector;
    private void Update() {
        // DIRECTION
        m_workVector.x = Input.GetAxis("Horizontal");
        m_workVector.y = Input.GetAxis("Vertical");
        m_workVector.Normalize();

        m_rb2d.velocity = m_workVector * m_speed;

        if(m_isFacingRight && m_workVector.x < 0 || !m_isFacingRight && m_workVector.x > 0){
            m_isFacingRight = !m_isFacingRight;
            m_renderer.flipX = !m_renderer.flipX;
        }

        if(m_rb2d.velocity.magnitude != 0 && !m_isMoving){
            m_isMoving = true;
            var emission = m_movingParticles.emission;
            emission.enabled = true;

            emission = m_stillParticles.emission;
            emission.enabled = false;   
        }else if (m_rb2d.velocity.magnitude == 0 && m_isMoving){
            m_isMoving = false;
            var emission = m_movingParticles.emission;
            emission.enabled = false;

            emission = m_stillParticles.emission;
            emission.enabled = true;
        }

    }
}
