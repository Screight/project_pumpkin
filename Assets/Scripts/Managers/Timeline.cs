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

    [SerializeField] Vector2 m_startCutscenePlayerPos;
    private Vector2 m_urasPosition;
    private bool m_movingUra = false;
    private float desiredDuration;
    private float elapsedTime;

    public bool m_cutSceneStartsWithDialog = false;
    [SerializeField] GameObject[] m_GameObjectsToActivate;

    private PauseMenu pausemenu;
    public PlayableDirector m_director;
    public Canvas m_HUD;
    private bool hasPlayed;
    private bool hasStartedPlaying;

    private void Start()
    {
        for (int i = 0; i < m_GameObjectsToActivate.Length; i++) { m_GameObjectsToActivate[i].SetActive(false); }
        pausemenu = FindObjectOfType<PauseMenu>();
        if (pausemenu == null) { Debug.LogError("No se ha encontrado un PauseMenu!!"); }
        hasPlayed = false;
        hasStartedPlaying = false;
    }

    private void Update()
    {
        if (m_movingUra) 
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            Player.Instance.LerpPlayerToPosition(m_urasPosition, m_startCutscenePlayerPos, percentageComplete);
        }
        if (m_movingUra && m_startCutscenePlayerPos == Player.Instance.PlayerCurrPos) { m_movingUra = false; startCutScene(); }

        if ((int)m_director.time == (int)m_director.duration && !hasPlayed) { hasPlayed = true; }

        if (m_director.state != PlayState.Playing && hasPlayed) { endCutScene(); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (m_startCutscenePlayerPos == Vector2.zero) { startCutScene(); }
            else if (!m_movingUra) 
            {
                float distX = Mathf.Abs(m_startCutscenePlayerPos.x - Player.Instance.PlayerCurrPos.x);
                float distY = Mathf.Abs(m_startCutscenePlayerPos.y - Player.Instance.PlayerCurrPos.y);
                float dist = Mathf.Sqrt(distX * distX + distY * distY);
                desiredDuration = dist/100*0.95f;

                m_urasPosition = Player.Instance.PlayerCurrPos;
                Player.Instance.SetPlayerToScripted();
                m_movingUra = true; 
            }
        }
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

    public void SetEntitiesTo(bool p_isActive)
    {

        for (int i = 0; i < m_entity.Length; i++)
        {
            m_entity[i].SetActive(p_isActive);
        }
    }
}