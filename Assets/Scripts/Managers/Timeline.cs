using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    public PlayableDirector m_director;
    public Canvas m_HUD;
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
        if (collision.gameObject.tag == "Player") { 
            startCutScene(); }
    }

    public void startCutScene()
    {
        //m_HUD.enabled = false;
        m_playerScript.SetPlayerToScripted();
        CameraManager.Instance.SetCameraToStatic();
        m_director.Play();
        hasPlayed = true;
    }
    public void endCutScene()
    {
        //m_HUD.enabled = true;
        m_playerScript.StopScripting(); 
        CameraManager.Instance.SetCameraToNormal();
        gameObject.SetActive(false);
    }
}