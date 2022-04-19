using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VineDestroyer : MonoBehaviour
{
    [SerializeField] bool m_respawnOnRoomCHange = true;
    private Tilemap vineSprite;
    Timer m_burnTimer;
    [SerializeField] float m_burnDuration = 0.5f;
    bool m_isBurning = false;
    [SerializeField] ROOMS m_room;

    private void Awake()
    {
        vineSprite = GetComponent<Tilemap>();
        m_burnTimer = gameObject.AddComponent<Timer>();
    }

    private void Start()
    {
        m_burnTimer.Duration = m_burnDuration;
    }

    private void Update()
    {
        if (m_burnTimer.IsFinished && m_isBurning)
        {
            gameObject.SetActive(false);
            m_isBurning = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fireball")|| collision.CompareTag("Spirit"))
        {
            vineSprite.color = Color.Lerp(Color.white, Color.red, 10.0f);
            m_burnTimer.Run();
            m_isBurning = true;
        }
    }

    public void Reset()
    {
        vineSprite.color = Color.white;
    }

    public ROOMS Room { get { return m_room;}}
    public bool RespawnOnRoomCHange { get { return m_respawnOnRoomCHange; }}
}