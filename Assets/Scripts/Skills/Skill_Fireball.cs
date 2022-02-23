using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Fireball : MonoBehaviour
{
    float m_cooldown;
    Timer m_timer;
    Player m_playerScript;

    bool m_isFireBallAvailable = true;

    Fireball m_fireBallScript;
    [SerializeField] GameObject m_fireBall;

    private void Awake()
    {
        m_timer = gameObject.AddComponent<Timer>();
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_fireBallScript = m_fireBall.GetComponent<Fireball>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_timer.Duration = m_cooldown;
        m_fireBall.SetActive(false);
        m_timer.Duration = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_playerScript.State)
        {
            default: { } break;
            case PLAYER_STATE.MOVE: { Fireball(); } break;
            case PLAYER_STATE.IDLE: { Fireball(); } break;
            case PLAYER_STATE.CAST: { Fireball(); } break;
            case PLAYER_STATE.JUMP: { Fireball(); } break;
            case PLAYER_STATE.FALL: { Fireball(); } break;
        }

    }

    void Fireball()
    {
        if (InputManager.Instance.Skill3ButtonPressed && m_timer.IsFinished && m_isFireBallAvailable)
        {
            m_fireBall.SetActive(true);
            m_isFireBallAvailable = false;
            m_fireBallScript.Fire();
            m_timer.Run();
        }
    }

    public bool IsFireBallAvailable
    {
        set { m_isFireBallAvailable = value; }
    }

}
