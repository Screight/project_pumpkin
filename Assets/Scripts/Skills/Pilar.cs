using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilar : MonoBehaviour
{
    [SerializeField] Skills m_skills;

    BoxCollider2D m_boxCollider2D;
    SpriteRenderer m_spriteRenderer;

    GameObject m_pilar;

    float m_height;
    float m_width;

    Timer m_emergeTimer;
    float m_emergeDuration = 2.0f;

    Timer m_pilarTimer;
    float m_pilarDuration = 2.0f;

    bool m_isEmerging;
    bool m_isActive;

    float m_emergingSpeed;

    float m_finalPositionY;

    private void Awake()
    {
        m_boxCollider2D = GetComponent<BoxCollider2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_isEmerging = false;

        m_height = m_boxCollider2D.size.y;
        m_width = m_boxCollider2D.size.x;

        m_emergeTimer = gameObject.AddComponent<Timer>();
        m_pilarTimer = gameObject.AddComponent<Timer>();

        m_emergingSpeed = m_height / m_emergeDuration;
        m_emergeTimer.Duration = m_emergeDuration;
        m_pilarTimer.Duration = m_pilarDuration;

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if(m_isEmerging && !m_emergeTimer.IsFinished) { EmergeFromGround(); }
        else if (m_isEmerging && m_emergeTimer.IsFinished)
        {
            m_isEmerging = false;
            m_isActive = true;
            m_pilarTimer.Run();
        }

        if(m_isActive && m_pilarTimer.IsFinished)
        {
            m_spriteRenderer.enabled = false;
            m_isActive = false;
            m_skills.IsOnCooldown = false;
            this.gameObject.SetActive(false);
        }
    }

    private void EmergeFromGround()
    {
        m_boxCollider2D.size += new Vector2(0, m_emergingSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, m_finalPositionY - (m_height - m_boxCollider2D.size.y), transform.position.z);
        m_boxCollider2D.offset = new Vector2(0, (m_height - m_boxCollider2D.size.y) / 2);
    }

    public void Summon(Vector3 p_finalPosition)
    {
        if (m_isEmerging) { return; }
        m_isEmerging = true;
        m_spriteRenderer.enabled = true;
        transform.position = p_finalPosition - new Vector3(0, m_height, 0);
        m_boxCollider2D.size = new Vector2(m_boxCollider2D.size.x, 0);
        m_finalPositionY = p_finalPosition.y;
        m_isEmerging = true;
        m_skills.IsOnCooldown = true;
        m_emergeTimer.Run();
    }
}
