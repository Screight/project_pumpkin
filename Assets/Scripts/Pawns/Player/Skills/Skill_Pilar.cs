using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Pilar : MonoBehaviour
{
    [SerializeField] SpellCooldown m_spellCooldownScript;
    Timer m_cooldownTimer;
    float m_cooldown = 3.0f;
    [SerializeField] SpriteRenderer m_markerSprite;

    [SerializeField] GameObject m_marker;
    Vector3 m_markerInitialRaycastPosition;
    Vector3 m_markerLastRaycastPosition;
    float m_markerSpeed = 50.0f;
    float m_markerMaxDepth = 120;
    float m_markerDirection = 10.0f;
    float m_markerMaxDistance = 100.0f;
    const float MARKER_MAX_DISTANCE = 100.0f;

    bool m_isCasting = false;
    Rigidbody2D m_rb2D;

    [SerializeField] GameObject m_pilar;
    BoxCollider2D m_pilarColider;
    Vector2 m_pilarSummonDistance;

    Pilar m_pilarScript;
    bool m_isPilarOnCooldown;

    bool m_canPlayerUseSkill;

    Player m_player;

    private void Awake()
    {
        m_cooldownTimer = gameObject.AddComponent<Timer>();
        m_pilar.SetActive(false);

        m_player = GetComponent<Player>();
        m_pilarColider = m_pilar.GetComponent<BoxCollider2D>();

        m_isPilarOnCooldown = false;
        m_canPlayerUseSkill = false;
    }

    private void Update()
    {
        if (m_cooldownTimer.IsRunning) { m_spellCooldownScript.FillPilarCooldownUI(m_cooldownTimer.CurrentTime / m_cooldownTimer.Duration); }
        else { m_spellCooldownScript.FillPilarCooldownUI(1); }
    }
    
    private void Start()
    {
        m_pilarScript = m_pilar.GetComponent<Pilar>();
        m_cooldownTimer.Duration = m_cooldown;
        m_rb2D = m_player.GetComponent<Rigidbody2D>();
        m_markerSprite.enabled = false;
    }

    public void Pilar()
    {
        if (!GameManager.Instance.GetIsSkillAvailable(SKILLS.PILAR)) { return ; }
        if (InputManager.Instance.Skill3ButtonHold && m_player.IsGrounded && !m_isPilarOnCooldown && m_cooldownTimer.IsFinished)
        {
            if (!m_isCasting) { StartCasting(); }
            else { MoveMarker(); }
        }
        else if (InputManager.Instance.Skill3buttonReleased && m_canPlayerUseSkill)
        {
            m_pilarSummonDistance = new Vector2(m_marker.transform.position.x, m_marker.transform.position.y);
            m_marker.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            m_isCasting = false;
            SummonPilar();
            m_player.State = PLAYER_STATE.IDLE;
            m_canPlayerUseSkill = false;
            m_markerSprite.enabled = false;
        }
    }

    void StartCasting()
    {
        m_markerSprite.enabled = true;
        m_isCasting = true;
        if (m_player.IsFacingRight) m_markerDirection = 1;
        else m_markerDirection = -1;
        m_player.State = PLAYER_STATE.CAST;
        AnimationManager.Instance.PlayAnimation(m_player, ANIMATION.PLAYER_IDLE, false);
        m_rb2D.velocity = Vector2.zero;
        m_markerInitialRaycastPosition = m_player.transform.position + 5 * Vector3.up;
        m_markerLastRaycastPosition = m_markerInitialRaycastPosition;

        // Raycast to see if there is a wall nearby, if there is change max distance to it

        RaycastHit2D[] hits;

       hits = Physics2D.RaycastAll(transform.position + Vector3.up, new Vector3(m_player.FacingDirection(), 0, transform.position.z), MARKER_MAX_DISTANCE);

        bool hasFoundWall = false;

        for (int i = 0; i < hits.Length && !hasFoundWall; i++)
        {
            if (hits[i].collider.CompareTag("floor") || hits[i].collider.CompareTag("platform"))
            {
                hasFoundWall = true;
                m_markerMaxDistance = hits[i].distance;
            }
        }

        if (!hasFoundWall) { m_markerMaxDistance = MARKER_MAX_DISTANCE; }
    }

    void MoveMarker()
    {
        // check if the marker is within limtis, if not change direction

        // check for minimum distance (the player position)
        if (m_player.IsFacingRight && m_markerInitialRaycastPosition.x < transform.position.x)
        {
            m_markerDirection *= -1;
            //m_markerInitialRaycastPosition = new Vector3(transform.position.x, m_marker.transform.position.y, m_marker.transform.position.z);
            m_markerInitialRaycastPosition = m_markerLastRaycastPosition;
        }
        else if (!m_player.IsFacingRight && m_markerInitialRaycastPosition.x > transform.position.x)
        {
            m_markerDirection *= -1;
            //m_markerInitialRaycastPosition = new Vector3(transform.position.x, m_marker.transform.position.y, m_marker.transform.position.z);
            m_markerInitialRaycastPosition = m_markerLastRaycastPosition;
        }

        // move the imaginary point from where the raycast starts from
        m_markerLastRaycastPosition = m_markerInitialRaycastPosition;
        m_markerInitialRaycastPosition += new Vector3(m_markerDirection * m_markerSpeed * Time.deltaTime, 0, 0);

        // check for max distance
        if (m_markerInitialRaycastPosition.x > transform.position.x + m_markerMaxDistance)
        {
            m_markerDirection *= -1;
            //m_markerInitialRaycastPosition = new Vector3(transform.position.x + m_markerMaxDistance, m_marker.transform.position.y, m_marker.transform.position.z);
            m_markerInitialRaycastPosition = m_markerLastRaycastPosition;
        }
        else if (m_markerInitialRaycastPosition.x < transform.position.x - m_markerMaxDistance)
        {
            m_markerDirection *= -1;
            //m_markerInitialRaycastPosition = new Vector3(transform.position.x - m_markerMaxDistance, m_marker.transform.position.y, m_markerInitialRaycastPosition.z);
            m_markerInitialRaycastPosition = m_markerLastRaycastPosition;
        }
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
                if (hits[i].collider.gameObject.CompareTag("floor") || hits[i].collider.gameObject.CompareTag("platform"))
                {
                    floorPosition = hits[i].point;
                    hasFoundFloor = true;
                }
            }
            i++;
        }

        if (hasFoundFloor)
        {
            m_marker.transform.position = new Vector3(floorPosition.x, floorPosition.y, transform.position.z);
            return true;
        }
        else
        {
            m_markerInitialRaycastPosition = m_markerLastRaycastPosition;
            m_markerDirection *= -1;
        }
        return false;
    }

    private void SummonPilar()
    {
        bool canPilarBeSummoned;

        float centerPositionX = m_pilarSummonDistance.x;
        float rightPositionX = m_pilarSummonDistance.x/* + m_pilarColider.size.x / 2*/;
        float leftPositionX = m_pilarSummonDistance.x/* - m_pilarColider.size.x / 2*/;

        float commonPositionY = m_pilarSummonDistance.y;

        Vector2 centerBottomPosition = new Vector2(centerPositionX, commonPositionY);
        Vector2 leftBottomPosition = new Vector2(leftPositionX, commonPositionY);
        Vector2 rightTopPosition = new Vector2(rightPositionX, commonPositionY + m_pilarColider.size.y);

        // check if the pillar would collide with a anything when summoned
        Collider2D[] collisions = Physics2D.OverlapAreaAll(leftBottomPosition + new Vector2(0,2), rightTopPosition);

        // check if any of the hits is a floor
        canPilarBeSummoned = CheckBoxCastWithScenario(collisions);

        if (canPilarBeSummoned)
        {
            m_pilar.SetActive(true);
            Vector3 pilarPosition = centerBottomPosition + Vector2.up * m_pilarColider.size.y / 2;

            // check if any of the pilar is inside a wall

            RaycastHit2D[] hits;

            hits = Physics2D.RaycastAll(m_markerInitialRaycastPosition, new Vector2(m_player.FacingDirection(), 0), m_pilarColider.size.x / 2);

            bool hasFoundWall = false;

            for (int i = 0; i < hits.Length && !hasFoundWall; i++)
            {
                if (hits[i].collider.CompareTag("floor") || hits[i].collider.CompareTag("platform"))
                {
                    hasFoundWall = true;
                    Debug.Log("Wall found");
                    pilarPosition.x = hits[i].point.x - m_player.FacingDirection() * m_pilarColider.size.x / 2;
                }
            }
            pilarPosition = new Vector3(pilarPosition.x, pilarPosition.y, transform.position.z);
            m_pilarScript.Summon(pilarPosition);

            Debug.Log("Pillar summoned");
            m_cooldownTimer.Run();
        }
        else { Debug.Log("ERROR at summoning pillar"); }
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
            if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("platform"))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsOnCooldown { set { m_isPilarOnCooldown = value; } }
}
