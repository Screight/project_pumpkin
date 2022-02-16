using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{

    [SerializeField] GameObject m_marker;
    Vector3 m_markerInitialRaycastPosition;
    float m_markerSpeed = 50.0f;
    float m_markerDirection = 10.0f;
    float m_markerMaxDistance = 100.0f;
    bool m_isHoldingPilar = false;
    Rigidbody2D m_rb2D;

    [SerializeField] GameObject m_pilar;
    BoxCollider2D m_pilarColider;
    Vector2 m_pilarSummonDistance;
    [SerializeField] float m_pilarCooldown = 2;

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
        m_rb2D = m_player.GetComponent<Rigidbody2D>();
    }

    private void Upadate()
    {

        if (InputManager.Instance.Skill1ButtonHold && m_player.IsGrounded && !m_isPilarOnCooldown)
        {
            //Debug.Log(m_player.State);
            if (!m_isHoldingPilar)
            {
                m_isHoldingPilar = true;
                if (m_player.IsFacingRight) m_markerDirection = 1;
                else m_markerDirection = -1;
                m_player.SetPlayerState(PLAYER_STATE.CAST);
                m_player.SetPlayerAnimation(PLAYER_ANIMATION.IDLE);
                m_rb2D.velocity = Vector2.zero;

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
            Debug.Log(m_player.State);
            //m_pilarSummonDistance = m_marker.transform.position.x;
            m_marker.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            m_isHoldingPilar = false;
            SummonPilar();
            m_player.SetPlayerState(PLAYER_STATE.IDLE);
        }

        // pillar cooldown
        if (m_isPilarOnCooldown)
        {
            if (m_pilarDuration < m_pilarCooldown) { m_pilarDuration += Time.deltaTime; }
            else
            {
                m_pilarDuration = 0;
                m_isPilarOnCooldown = false;
            }
        }

    }

    private void Update()
    {
        if (InputManager.Instance.Skill1ButtonHold && m_player.IsGrounded)
        {
            if (!m_isHoldingPilar)
            {
                m_isHoldingPilar = true;
                if (m_player.IsFacingRight) m_markerDirection = 1;
                else m_markerDirection = -1;
                m_player.SetPlayerState(PLAYER_STATE.CAST);
                m_player.SetPlayerAnimation(PLAYER_ANIMATION.IDLE);
                m_rb2D.velocity = Vector2.zero;
                m_markerInitialRaycastPosition = m_player.transform.position;
            }

            if (m_markerInitialRaycastPosition.x > transform.position.x + m_markerMaxDistance)
            {
                m_markerDirection *= -1;
                m_marker.transform.position = new Vector3(transform.position.x + m_markerMaxDistance, m_marker.transform.position.y, m_marker.transform.position.z);
            }
            else if (m_markerInitialRaycastPosition.x < transform.position.x - m_markerMaxDistance)
            {
                m_markerDirection *= -1;
                m_markerInitialRaycastPosition = new Vector3(transform.position.x - m_markerMaxDistance, m_markerInitialRaycastPosition.y, m_markerInitialRaycastPosition.z);
            }

            if (m_player.IsFacingRight && m_markerInitialRaycastPosition.x < transform.position.x)
            {
                m_markerDirection *= -1;
                m_markerInitialRaycastPosition = new Vector3(transform.position.x, m_markerInitialRaycastPosition.y, m_markerInitialRaycastPosition.z);

            }
            else if (!m_player.IsFacingRight && m_markerInitialRaycastPosition.x > transform.position.x)
            {
                m_markerDirection *= -1;
            }

            m_markerInitialRaycastPosition += new Vector3(m_markerDirection * m_markerSpeed * Time.deltaTime, 0, 0);

            m_marker.transform.position = CheckPositionForPillar(m_markerInitialRaycastPosition);
        }

        else if (InputManager.Instance.Skill1buttonReleased)
        {
            Debug.Log(m_player.State);
            m_pilarSummonDistance = new Vector2 (m_marker.transform.position.x, m_marker.transform.position.y);
            m_marker.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            m_isHoldingPilar = false;
            SummonPilar();
            m_player.SetPlayerState(PLAYER_STATE.IDLE);
        }
    }

    private Vector3 CheckPositionForPillar(Vector2 initialPositionToRaycastFrom)
    {
        Vector3 floorPosition = Vector3.zero;
        RaycastHit2D[] hits;
        bool hasFoundFloor = false;

        hits = Physics2D.RaycastAll(initialPositionToRaycastFrom, Vector2.down, 1000);
        int i = 0;

        while (!hasFoundFloor && i < hits.Length && !hasFoundFloor)
        {
            if (hits[i].collider != null)
            {
                if (hits[i].collider.gameObject.CompareTag("floor"))
                {
                    floorPosition = hits[i].point;
                    hasFoundFloor = true;
                }
            }
            i++;
        }

        return floorPosition;
    }

    private void SummonPilar()
    {
        m_isPilarOnCooldown = true;
        m_isPilarSummoned = true;

        bool canPilarBeSummoned;

        bool verticalCollision = false; 

        float centerPositionX = m_pilarSummonDistance.x;
        float rightPositionX = m_pilarSummonDistance.x + m_pilarColider.size.x / 2;
        float leftPositionX = m_pilarSummonDistance.x - m_pilarColider.size.x / 2;

        float commonPositionY = m_pilarSummonDistance.y;

        Vector2 centerBottomPosition = new Vector2(centerPositionX, commonPositionY);
        Vector2 rightBottomPosition = new Vector2(rightPositionX, commonPositionY);
        Vector2 leftBottomPosition = new Vector2(leftPositionX, commonPositionY);

        Vector2 rightTopPosition = new Vector2(rightPositionX, commonPositionY + m_pilarColider.size.y);

        Collider2D[] collisions = Physics2D.OverlapAreaAll(leftBottomPosition + new Vector2(0,2), rightTopPosition);

        canPilarBeSummoned = CheckBoxCastWithScenario(collisions);

        RaycastHit2D[] hits;

        if (canPilarBeSummoned)
        {
            hits = Physics2D.RaycastAll(centerBottomPosition, Vector2.down, 2);
            canPilarBeSummoned = canPilarBeSummoned && CheckRayCastWithScenario(hits);
        }

        if(canPilarBeSummoned)
        {
            m_pilar.SetActive(true);
            Vector3 pilarPosition = centerBottomPosition + Vector2.up * m_pilarColider.size.y / 2;
            m_pilarScript.Summon(pilarPosition);
            
            Debug.Log("Pillar summoned");
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

    public bool IsHoldingPilar()
    {
        return m_isHoldingPilar;
    }

}
