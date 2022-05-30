using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{   
    [SerializeField] GameObject[] m_entity;
    [SerializeField] bool m_isCameraScripted = false;
    [SerializeField] bool m_isFireSpiritCutScene = false;
    [SerializeField] bool m_isDarknessSpiritCutScene = false;
    [SerializeField] bool m_hideHud = false;
    [SerializeField] bool m_playerCanMove = false;
    public bool m_cutSceneStartsWithDialog = false;
    [SerializeField] GameObject[] m_GameObjectsToActivate;

    public PauseMenu pausemenu;
    public PlayableDirector m_director;
    public Canvas m_HUD;
    private bool hasPlayed;
    private bool hasStartedPlaying;
    bool m_hasBeenPlayed = false;

    private void Start() 
    {
        for (int i = 0; i < m_GameObjectsToActivate.Length; i++) { m_GameObjectsToActivate[i].SetActive(false); }
        pausemenu = FindObjectOfType<PauseMenu>();
        hasPlayed = false;
        hasStartedPlaying = false; 
    }

    private void Update()
    {
        if ((int)m_director.time == (int)m_director.duration && !hasPlayed) { hasPlayed = true; }

        if (m_director.state != PlayState.Playing && hasPlayed) { endCutScene(); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { startCutScene(); }
    }

    public void startCutScene()
    {
        //Debug.Log("Empieza CutScene");
        for (int i = 0; i < m_GameObjectsToActivate.Length; i++) { m_GameObjectsToActivate[i].SetActive(true); }
        pausemenu.IsCutScenePlaying = true;
        if (m_isCameraScripted) { CameraManager.Instance.SetCameraToStatic(); }
        if (m_hideHud) { m_HUD.enabled = false; }
        if (!m_playerCanMove) { Player.Instance.SetPlayerToScripted(); }

        m_director.Play();
        hasStartedPlaying = true;
    }
    public void endCutScene()
    {
        //Debug.Log("Termina CutScene y me puedo mover");
        pausemenu.IsCutScenePlaying = false;
        if (!m_playerCanMove) { Player.Instance.StopScripting(); }

        if (m_isCameraScripted) { CameraManager.Instance.SetCameraToNormal(); }
        if (m_hideHud) { m_HUD.enabled = true; }
        if (m_isFireSpiritCutScene) { GameManager.Instance.SetIsSkillAvailable(SKILLS.FIRE_BALL, true); }
        if (m_isDarknessSpiritCutScene) { GameManager.Instance.SetIsSkillAvailable(SKILLS.DASH, true); }
        gameObject.SetActive(false);
    }

    public bool HasStartedplaying { get { return hasStartedPlaying; } }
    public bool PlayerCanMove { get { return m_playerCanMove; } }

    public void SetEntitiesTo(bool p_isActive){

        for(int i = 0; i < m_entity.Length; i++){
            m_entity[i].SetActive(p_isActive);
        }

        
    }

}