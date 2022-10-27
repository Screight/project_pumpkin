using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    [SerializeField] GameObject[] m_entity;
    private Vector3 m_firstEntity;
    [SerializeField] bool m_isCameraScripted = false;
    [SerializeField] bool m_isFireSpiritCutScene = false;
    [SerializeField] bool m_isDarknessSpiritCutScene = false;
    [SerializeField] bool m_isSamaelCutScene = false;
    [SerializeField] bool m_hideHud = false;
    [SerializeField] bool m_playerCanMove = false;

    private bool m_movingUra = false;

    public bool m_cutSceneStartsWithDialog = false;
    [SerializeField] GameObject[] m_GameObjectsToActivateAtEnd;

    private PauseMenu pausemenu;
    [SerializeField] Samu_animation_script samaelScript;
    private PlayableDirector m_director;
    private GameObject m_HUD;
    private bool hasPlayed;
    private bool hasStartedPlaying;

    private void Awake()
    {
        m_director = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        //Get PauseMenu
        pausemenu = FindObjectOfType<PauseMenu>();
        if (pausemenu == null) { Debug.LogError("No se ha encontrado el PauseMenu!!"); }
        //Get Hud
        m_HUD = GameObject.Find("hudCanvas");
        if (m_HUD == null) { Debug.LogError("Nombrad al hud \"hudCanvas\"!"); }

        //if (m_isSamaelCutScene) { samaelScript = FindObjectOfType<Samu_animation_script>(); }

        for (int i = 0; i < m_GameObjectsToActivateAtEnd.Length; i++) { m_GameObjectsToActivateAtEnd[i].SetActive(false); }
        hasPlayed = false;
        hasStartedPlaying = false;
        m_firstEntity = m_entity[0].transform.localPosition;
    }

    private void Update()
    {
        if (Player.Instance.IsGrounded && m_movingUra) { m_movingUra = false; startCutScene(); }
        if (m_movingUra) { return; }

        if ((int)m_director.time == (int)m_director.duration && !hasPlayed) { hasPlayed = true; }

        if (m_director.state != PlayState.Playing && hasPlayed) { endCutScene(); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (Player.Instance.IsGrounded || m_playerCanMove || m_isDarknessSpiritCutScene) { startCutScene(); }
            else
            {
                Player.Instance.SetPlayerToScripted();
                Player.Instance.ScriptFall();
                m_movingUra = true; 
            }
        }
    }

    public void startCutScene()
    {
        pausemenu.IsCutScenePlaying = true;
        if (m_isCameraScripted) { CameraManager.Instance.CameraAtCutScene = true; }
        if (m_hideHud) { m_HUD.SetActive(false); }
        if (!m_playerCanMove) { Player.Instance.SetPlayerToScripted(); }
        //Stop BGM at CutScene6
        if (m_isSamaelCutScene && samaelScript != null) 
        {
            StartCoroutine(FadeAudioSource.StartFade(SoundManager.Instance.audioSrcBGM, 3.0f, 0.0f));
        }

        //play!
        m_director.Play();
        hasStartedPlaying = true;
    }
    public void endCutScene()
    {
        pausemenu.IsCutScenePlaying = false;
        if (!m_playerCanMove) { Player.Instance.StopScripting(); }
        if (m_isCameraScripted) { CameraManager.Instance.CameraAtCutScene = false; }
        if (m_hideHud) { m_HUD.SetActive(true); }

        //CutScene 3
        if (m_isFireSpiritCutScene) { GameManager.Instance.SetIsSkillAvailable(SKILLS.FIRE_BALL, true); }
        //CutScene 4.5
        if (m_isDarknessSpiritCutScene) { GameManager.Instance.SetIsSkillAvailable(SKILLS.DASH, true); }
        //CutScene 6
        if (m_isSamaelCutScene && samaelScript!=null) { samaelScript.StartTimer(); }
        else if(m_isSamaelCutScene && samaelScript == null) { Debug.LogError("No hay refe de Samael, activalo en la jerarquia o ponlo en escena mijo"); }

        for (int i = 0; i < m_GameObjectsToActivateAtEnd.Length; i++) { m_GameObjectsToActivateAtEnd[i].SetActive(true); }
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

    public void Reset()
    {
        m_movingUra = false;
        hasPlayed = false;
        m_director.RebindPlayableGraphOutputs();
        m_director.Pause();
        m_entity[0].transform.localPosition = m_firstEntity;
        m_entity[0].GetComponent<SpriteRenderer>().flipX = true;
    }
}