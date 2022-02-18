using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundbreaker : MonoBehaviour
{
    float m_maxSpeed = -600;
    Player m_player;
    Rigidbody2D m_rb2D;

    private void Awake()
    {
        m_player = GetComponent<Player>();
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (m_player.State)
        {
            default:
                {
                    if (InputManager.Instance.Skill2ButtonPressed && !m_player.IsGrounded)
                    {
                        m_rb2D.velocity = new Vector2(0, m_maxSpeed);
                        m_player.SetPlayerState(PLAYER_STATE.GROUNDBREAKER);
                        m_rb2D.gravityScale = 0;
                    }
                }
                break;
            case PLAYER_STATE.GROUNDBREAKER:
                {
                    
                }
                break;
        }
    }
}
