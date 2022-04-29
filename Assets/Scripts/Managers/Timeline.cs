using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{   
    [SerializeField] bool m_isCameraScripted = false;
    [SerializeField] bool m_isFireSpiritCutScene = false;
    [SerializeField] bool m_hideHud = false;
    [SerializeField] bool m_playerCanMove = false;
    public bool m_cutSceneStartsWithDialog = false;

    public PauseMenu pausemenu;
    public PlayableDirector m_director;
    public Canvas m_HUD;
    private bool hasPlayed;
    private bool hasStartedPlaying;

    private void Start() 
    {
        pausemenu = FindObjectOfType<PauseMenu>();
        hasPlayed = false;
        hasStartedPlaying = false; 
    }

    private void Update()
    {
        if ((int)m_director.time == (int)m_director.duration) { hasPlayed = true; Debug.Log("Hasplayed puesto a true"); }

        if (m_director.state != PlayState.Playing && hasPlayed) { endCutScene(); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { startCutScene(); }
    }

    public void startCutScene()
    {
        Debug.Log("Empieza CutScene");
        pausemenu.IsCutScenePlaying = true;
        if (m_isCameraScripted) { CameraManager.Instance.SetCameraToStatic(); }
        if (m_hideHud) { m_HUD.enabled = false; }
        if (!m_playerCanMove) { Player.Instance.SetPlayerToScripted(); }

        m_director.Play();
        hasStartedPlaying = true;
    }
    public void endCutScene()
    {
        Debug.Log("Termina CutScene y me puedo mover");
        pausemenu.IsCutScenePlaying = false;
        if (!m_playerCanMove) { Player.Instance.StopScripting(); }

        if (m_isCameraScripted) { CameraManager.Instance.SetCameraToNormal(); }
        if (m_hideHud) { m_HUD.enabled = true; }
        if (m_isFireSpiritCutScene) { GameManager.Instance.SetIsSkillAvailable(SKILLS.FIRE_BALL, true); }
        gameObject.SetActive(false);
    }

    public bool HasStartedplaying { get { return hasStartedPlaying; } }
}