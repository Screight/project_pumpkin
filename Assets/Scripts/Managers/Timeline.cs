using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    public PlayableDirector m_director;
    [SerializeField] GameObject m_player;
    Player m_playerScript;
    private bool hasPlayed;

    private void Start()
    {
        hasPlayed = false;
        m_player = GameObject.FindGameObjectWithTag("Player"); m_playerScript = m_player.GetComponent<Player>();
    }

    private void Update()
    {
        if (m_director.state != PlayState.Playing && hasPlayed) { endCutScene(); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { startCutScene(); }
    }

    public void startCutScene()
    {
        m_playerScript.SetPlayerToScripted();
        m_director.Play();
        hasPlayed = true;
    }
    public void endCutScene()
    {
        m_playerScript.StopScripting(); 
        gameObject.SetActive(false);
    }
}