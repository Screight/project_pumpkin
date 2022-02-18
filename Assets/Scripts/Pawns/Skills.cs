using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [SerializeField] GameObject m_marker;
    Vector3 m_markerInitialRaycastPosition;
    float m_markerSpeed = 50.0f;
    float m_markerMaxDepth = 120;
    float m_markerDirection = 10.0f;
    float m_markerMaxDistance = 100.0f;

    bool m_isCasting = false;
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

    private void Update()
    {
        switch (m_player.State)
        {
            default: { } break;
            case PLAYER_STATE.MOVE: { Pilar(); } break;
            case PLAYER_STATE.IDLE: { Pilar(); } break;
            case PLAYER_STATE.CAST: { Pilar(); } break;
        }
    }

    private void Pilar()
    {
        if (InputManager.Instance.Skill1ButtonHold && m_player.IsGrounded && !m_isPilarOnCooldown)
        {
            if(!m_isCasting) { StartCasting(); }
            else {
                MoveMarker();
                m_canPlayerUseSkill = CheckPositionForPillar(m_markerInitialRaycastPosition);
            }
        }
        else if (InputManager.Instance.Skill1buttonReleased && m_canPlayerUseSkill)
        {
            m_pilarSummonDistance = new Vector2(m_marker.transform.position.x, m_marker.transform.position.y);
            m_marker.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            m_isCasting = false;
            SummonPilar();
            m_player.SetPlayerState(PLAYER_STATE.IDLE);
            m_canPlayerUseSkill = false;
        }
    }

    void StartCasting()
    {
        m_isCasting = true;
        if (m_player.IsFacingRight) m_markerDirection = 1;
        else m_markerDirection = -1;
        m_player.SetPlayerState(PLAYER_STATE.CAST);
        m_player.SetPlayerAnimation(PLAYER_ANIMATION.IDLE);
        m_rb2D.velocity = Vector2.zero;
        m_markerInitialRaycastPosition = m_player.transform.position;
    }

    void MoveMarker()
    {
        // check if the marker is within limtis, if not change direction

        // check for max distance
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

        // check for minimum distance (the player position)
        if (m_player.IsFacingRight && m_markerInitialRaycastPosition.x < transform.position.x)
        {
            m_markerDirection *= -1;
        }
        else if (!m_player.IsFacingRight && m_markerInitialRaycastPosition.x > transform.position.x)
        {
            m_markerDirection *= -1;
        }

        // move the imaginary point from where the raycast starts from
        m_markerInitialRaycastPosition += new Vector3(m_markerDirection * m_markerSpeed * Time.deltaTime, 0, 0);
        m_canPlayerUseSkill = CheckPositionForPillar(m_markerInitialRaycastPosition);
    }

    /// <summary>
    /// Checks for a correct position for the marker
    /// </summary>
    /// <param name="initialPositionToRaycastFrom"></param>
    /// <returns> True if there is a suitable position for the marker/pillar false otherwise</returns>
    private bool CheckPositionForPillar(Vector2 initialPositionToRaycastFrom)
    {
        Vector3 floorPosition = Vector3.zero;
        RaycastHit2D[] hits;
        bool hasFoundFloor = false;

        // raycast from the imaginary top to the botoom
        hits = Physics2D.RaycastAll(initialPositionToRaycastFrom, Vector2.down, m_markerMaxDepth);
        int i = 0;

        // check all hits from raycast and see if any of them is a floor and take the closest one
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

        if (hasFoundFloor)
        {
            m_marker.transform.position = floorPosition;
            return true;
        }
        else
        {
            m_markerDirection *= -1;
        }
        return false;
    }

    private void SummonPilar()
    {
        m_isPilarSummoned = true;
        bool canPilarBeSummoned;
        bool verticalCollision = false; 

        float centerPositionX = m_pilarSummonDistance.x;
        float rightPositionX = m_pilarSummonDistance.x + m_pilarColider.size.x / 2;
        float leftPositionX = m_pilarSummonDistance.x - m_pilarColider.size.x / 2;

        float commonPositionY = m_pilarSummonDistance.y;

        Vector2 centerBottomPosition = new Vector2(centerPositionX, commonPositionY);
        Vector2 leftBottomPosition = new Vector2(leftPositionX, commonPositionY);
        Vector2 rightTopPosition = new Vector2(rightPositionX, commonPositionY + m_pilarColider.size.y);

        // check if the pillar would collide with a anything when summoned
        Collider2D[] collisions = Physics2D.OverlapAreaAll(leftBottomPosition + new Vector2(0,2), rightTopPosition);

        // check if any of the hits is a floor
        canPilarBeSummoned = CheckBoxCastWithScenario(collisions);

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

    /// <summary>
    /// Check a list of colliders and determine
    /// </summary>
    /// <param name="p_collisions"></param>
    /// <returns> True if any of the tags is "floor" false otherwise.</returns>
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

    public bool IsOnCooldown
    {
        set { m_isPilarOnCooldown = value; }
    }
}
