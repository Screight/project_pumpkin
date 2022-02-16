using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{

    [SerializeField] GameObject m_marker;
    float m_markerSpeed = 50.0f;
    float m_markerDirection = 10.0f;
    float m_markerMaxDistance = 100.0f;
    bool m_isHoldingPilar = false;

    [SerializeField] GameObject m_pilar;
    BoxCollider2D m_pilarColider;
    [SerializeField] float m_pilarSummonDistance = 50;
    [SerializeField] float m_pilarCooldown = 1;

    Pilar m_pilarScript;

    float m_pilarDuration;
    bool m_isPilarSummoned;
    bool m_isPilarOnCooldown;

    bool m_canPlayerUseSkill;

    Player m_player;

    private void Awake()
    {
        m_pilar.SetActive(false);

        m_player = GetComponent<Player>();
        m_pilarColider = m_pilar.GetComponent<BoxCollider2D>();

        m_isPilarOnCooldown = false;
        m_isPilarSummoned = false;
        m_canPlayerUseSkill = false;

        m_pilarDuration = 0;
    }

    private void Start()
    {
        m_pilarScript = m_pilar.GetComponent<Pilar>();
    }

    private void Update()
    {
        // pillar cooldown
        //if (m_isPilarOnCooldown)
        //{
        //    if (m_pilarDuration < m_pilarCooldown) { m_pilarDuration += Time.deltaTime; }
        //    else
        //    {
        //        m_pilarDuration = 0;
        //        m_isPilarOnCooldown = false;
        //    }
        //}

        //m_canPlayerUseSkill = (m_player.State == PLAYER_STATE.IDLE || m_player.State == PLAYER_STATE.MOVE) && m_player.IsGrounded;

        //if (m_canPlayerUseSkill && !m_isPilarOnCooldown && InputManager.Instance.Skill1ButtonPressed)
        //{
        //    SummonPilar();
        //}

        if (InputManager.Instance.Skill1ButtonHold)
        {
            if (!m_isHoldingPilar)
            {
                m_isHoldingPilar = true;
                if (m_player.IsFacingRight) m_markerDirection = 1;
                else m_markerDirection = -1;
            }

            if (m_marker.transform.position.x > transform.position.x + m_markerMaxDistance)
            {
                m_markerDirection *= -1;
                m_marker.transform.position = new Vector3(transform.position.x + m_markerMaxDistance, m_marker.transform.position.y, m_marker.transform.position.z);
            }
            else if (m_marker.transform.position.x < transform.position.x - m_markerMaxDistance)
            {
                m_markerDirection *= -1;
                m_marker.transform.position = new Vector3(transform.position.x - m_markerMaxDistance, m_marker.transform.position.y, m_marker.transform.position.z);
            }

            if (m_player.IsFacingRight && m_marker.transform.position.x < transform.position.x)
            {
                m_markerDirection *= -1;
                m_marker.transform.position = new Vector3(transform.position.x, m_marker.transform.position.y, m_marker.transform.position.z);

            }
            else if (!m_player.IsFacingRight && m_marker.transform.position.x > transform.position.x)
            {
                m_markerDirection *= -1;
            }

            m_marker.transform.position += new Vector3( m_markerDirection * m_markerSpeed * Time.deltaTime, 0, 0);
        }
        else if (InputManager.Instance.Skill1buttonReleased)
        {
            m_isHoldingPilar = false;
        }

    }

    private void SummonPilar()
    {
        m_isPilarOnCooldown = true;
        m_isPilarSummoned = true;

        bool canPilarBeSummoned;

        bool verticalCollision = false; 

        float offSet;
        if (m_player.IsFacingRight) { offSet = m_pilarSummonDistance; }
        else { offSet = -m_pilarSummonDistance; }

        float centerPositionX = transform.position.x + offSet;
        float rightPositionX = transform.position.x + offSet + m_pilarColider.size.x / 2;
        float leftPositionX = transform.position.x + offSet - m_pilarColider.size.x / 2;

        float commonPositionY = transform.position.y;

        Vector2 centerBottomPosition = new Vector2(centerPositionX, commonPositionY);
        Vector2 rightBottomPosition = new Vector2(rightPositionX, commonPositionY);
        Vector2  leftBottomPosition= new Vector2(leftPositionX, commonPositionY);

        Vector2 rightTopPosition = new Vector2(rightPositionX, commonPositionY + m_pilarColider.size.y);

        Collider2D[] collisions = Physics2D.OverlapAreaAll(leftBottomPosition + new Vector2(0,2), rightTopPosition);

        canPilarBeSummoned = CheckBoxCastWithScenario(collisions);

        RaycastHit2D[] hits;

        if (canPilarBeSummoned)
        {
            hits = Physics2D.RaycastAll(centerBottomPosition, Vector2.down, 2);
            canPilarBeSummoned = canPilarBeSummoned && CheckRayCastWithScenario(hits);
        }

        if (canPilarBeSummoned)
        {
            hits = Physics2D.RaycastAll(rightBottomPosition, Vector2.down, 2);
            canPilarBeSummoned = canPilarBeSummoned && CheckRayCastWithScenario(hits);
        }

        if(canPilarBeSummoned)
        {
            hits = Physics2D.RaycastAll(leftBottomPosition, Vector2.down, 2);
            canPilarBeSummoned = canPilarBeSummoned && CheckRayCastWithScenario(hits);
        }

        if(canPilarBeSummoned)
        {
            m_pilar.SetActive(true);
            Vector3 pilarPosition = centerBottomPosition + Vector2.up * m_pilarColider.size.y / 2;
            m_pilarScript.Summon(pilarPosition);
            
            Debug.Log("Pillar sumonned");
        }
        else
        {
            Debug.Log("ERROR at summoning pillar");
        }
    }

    private bool CheckRayCastWithScenario(RaycastHit2D[] p_hits)
    {
        foreach (RaycastHit2D hit in p_hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("floor"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckBoxCastWithScenario(Collider2D[] p_collisions)
    {
        foreach (Collider2D collision in p_collisions)
        {
            if (collision.gameObject.CompareTag("floor"))
            {
                return false;
            }
        }
        return true;
    }

}
