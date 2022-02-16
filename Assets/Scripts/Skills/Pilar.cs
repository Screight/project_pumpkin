using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilar : MonoBehaviour
{
    BoxCollider2D m_boxCollider2D;
    SpriteRenderer m_spriteRenderer;

    GameObject m_pilar;

    float m_height;
    float m_width;

    float m_emergeDuration = 2.0f;
    float m_currentEmergeDuration = 2.0f;

    float m_pilarDuration = 2.0f;
    float m_pilarCurrentDuration;

    bool m_isEmerging;

    float m_emergingSpeed;

    float m_finalPositionY;

    private void Awake()
    {
        m_boxCollider2D = GetComponent<BoxCollider2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_isEmerging = false;

        m_height = m_boxCollider2D.size.y;
        m_width = m_boxCollider2D.size.x;

    }

    private void Start()
    {

        m_emergingSpeed = m_height / m_emergeDuration;
        m_currentEmergeDuration = 0;

        //m_spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (m_isEmerging)
        {
            if(m_currentEmergeDuration < m_emergeDuration)
            {
                m_currentEmergeDuration += Time.deltaTime;
                EmergeFromGround();
            }
            else
            {
                m_currentEmergeDuration = 0;
                m_isEmerging = false;
            }
        }

        if (!m_isEmerging)
        {
            if (m_pilarCurrentDuration < m_pilarDuration)
            {
                m_pilarCurrentDuration += Time.deltaTime;
            }
            else
            {
                m_pilarCurrentDuration = 0;
                m_spriteRenderer.enabled = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    private void EmergeFromGround()
    {
        if(m_currentEmergeDuration < m_emergeDuration)
        {
            m_boxCollider2D.size += new Vector2(0, m_emergingSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, m_finalPositionY - (m_height - m_boxCollider2D.size.y), transform.position.z);
            m_boxCollider2D.offset = new Vector2(0, (m_height -  m_boxCollider2D.size.y) / 2);
        }
    }

    public void Summon(Vector3 p_finalPosition)
    {
        if (m_isEmerging) return;
        m_isEmerging = true;
        m_spriteRenderer.enabled = true;
        transform.position = p_finalPosition - new Vector3(0, m_height, 0);
        m_boxCollider2D.size = new Vector2(m_boxCollider2D.size.x, 0);
        m_finalPositionY = p_finalPosition.y;
    }

}
