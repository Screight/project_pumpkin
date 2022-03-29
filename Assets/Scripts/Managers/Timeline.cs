using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{   
    [SerializeField] bool m_isCameraScripted = false;
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
        if (collision.gameObject.tag == "Player") { startCutScene(); }
    }

    public void startCutScene()
    {
        MenuScript.Instance.CutSceneIsPlaying = true;
        if (m_isCameraScripted) { CameraManager.Instance.SetCameraToStatic(); }
        m_playerScript.SetPlayerToScripted();

        m_director.Play();
        hasPlayed = true;
    }
    public void endCutScene()
    {
        MenuScript.Instance.CutSceneIsPlaying = false;
        m_playerScript.StopScripting();

        if (m_isCameraScripted) { CameraManager.Instance.SetCameraToNormal(); }
        gameObject.SetActive(false);
    }
}