using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    float m_speed = 200.0f;
    Player m_playerScript;
    Rigidbody2D m_rb2D;
    [SerializeField] Skill_Fireball m_skillFireBallScript;
    Timer m_maxDurationTimer;
    float m_maxDuration = 5.0f;

    int m_damage = 1;

    private void Awake()
    {
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_rb2D = GetComponent<Rigidbody2D>();
        m_maxDurationTimer = gameObject.AddComponent<Timer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_maxDurationTimer.Duration = m_maxDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_maxDurationTimer.IsFinished)
        {
            DesactivateFireBall();
        }
    }

    public void Fire()
    {
        m_rb2D.velocity = new Vector2(m_playerScript.FacingDirection() * m_speed,0);
        transform.position = m_playerScript.gameObject.transform.position + new Vector3(0,10,0);
        m_maxDurationTimer.Run();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "floor" || collision.CompareTag("enemy") || collision.CompareTag("platform"))
        {
            DesactivateFireBall();
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Damage(m_damage);
        }
    }

    void DesactivateFireBall()
    {
        gameObject.SetActive(false);
        m_skillFireBallScript.IsFireBallAvailable = true;
        m_maxDurationTimer.Stop();
    }

}
