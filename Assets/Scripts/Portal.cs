using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : InteractiveItem
{
    [SerializeField] Vector2 m_portalImpulse = new Vector2(30, 100);
    [SerializeField] Portal m_destinyPortal;
    [SerializeField] Transform m_playerPosition;
    Transicion m_transicion;
    [SerializeField] ZONE m_zone = ZONE.MINE;
    Animator m_animator;
    SpriteRenderer m_renderer;
    AudioSource m_PortalLoopSFX;
    MiniMap m_map;

    Timer m_eventTimer;
    float m_closeDuration;
    bool m_isPortalClosing;
    protected override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animator>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_transicion = FindObjectOfType<Transicion>();
        m_PortalLoopSFX = GetComponent<AudioSource>();
        m_map = FindObjectOfType<MiniMap>();
        m_eventTimer = gameObject.AddComponent<Timer>();
    }

    private void Start()
    {
        m_renderer.enabled = false;
        m_closeDuration = AnimationManager.Instance.GetClipDuration(m_animator, ANIMATION.PORTAL_CLOSE);
        m_eventTimer.Duration = m_closeDuration;
    }

    protected override void Update()
    {
        base.Update();
        if(m_isPortalClosing && m_eventTimer.IsFinished)
        {
            m_isPortalClosing = false;
            m_renderer.enabled = false;
        }
    }

    protected override void HandleInteraction()
    {
        base.HandleInteraction();

        Player.Instance.SetPlayerToScripted();
        m_transicion.AddListenerToEndOfFadeIn(TransportPlayer);
        m_transicion.AddListenerToEndOfTransition(EndTransportPlayer);
        m_transicion.FadeIn();
        AnimationManager.Instance.PlayAnimation(m_animator, ANIMATION.PORTAL_CLOSE);
        GameManager.Instance.IsGamePaused = true;
        SoundManager.Instance.PlayOnce(AudioClipName.PORTALUSE);
        Player.Instance.SetPlayerToInvisible(true);
    }

    public void OpenPortal()
    {
        m_renderer.enabled = true;
        m_isPortalClosing = false;
        AnimationManager.Instance.PlayAnimation(m_animator, ANIMATION.PORTAL_OPEN);
    }

    public void ClosePortal()
    {
        m_isPortalClosing = true;
        m_eventTimer.Restart();
        AnimationManager.Instance.PlayAnimation(m_animator, ANIMATION.PORTAL_CLOSE);
    }

    public void TransportPlayer()
    {
        Player.Instance.transform.position = new Vector3(m_destinyPortal.SpawnPosition.x, m_destinyPortal.SpawnPosition.y, Player.Instance.transform.position.z);
        m_transicion.RemoveListenerToEndOfFadeIn(TransportPlayer);
        GameManager.Instance.CurrentZone = m_destinyPortal.Zone;

        BACKGROUND_CLIP clip = BACKGROUND_CLIP.ABANDONEDMINE;
        if (m_zone == ZONE.FOREST)
        {
            clip = BACKGROUND_CLIP.FORESTOFSOULS;
        }
        else if (m_zone == ZONE.MINE)
        {
            clip = BACKGROUND_CLIP.ABANDONEDMINE;
        }
        SoundManager.Instance.SetBackgroundMusicTo(clip);

    }

    public void EndTransportPlayer()
    {
        Player.Instance.StopScripting();
        GameManager.Instance.IsGamePaused = false;
        m_transicion.RemoveListenerToEndOfTransition(EndTransportPlayer);
        Player.Instance.SetPlayerToInvisible(false);
        Player.Instance.Speed = m_portalImpulse;
        Player.Instance.transform.position = new Vector3(m_destinyPortal.SpawnPosition.x, m_destinyPortal.SpawnPosition.y, Player.Instance.transform.position.z);
        Player.Instance.SetScriptedFor(0.3f);
        if(Player.Instance.transform.position.x < m_destinyPortal.SpawnPosition.x){
            Player.Instance.FacePlayerToLeft();
        }
        else{
            Player.Instance.FacePlayerToRight();
        }

    }

    public Vector3 SpawnPosition
    {
        get { return m_playerPosition.position; }
    }

    public ZONE Zone 
    {
        get { return m_zone; }
    }

    public void PlayOpeningSFX() { SoundManager.Instance.PlayOnce(AudioClipName.PORTALOPEN); }
    public void PlayClosingSFX() { SoundManager.Instance.PlayOnce(AudioClipName.PORTALCLOSE); }
    public void PlayLoopSFX() { m_PortalLoopSFX.Play(); }
    public void StopLoopSFX() { m_PortalLoopSFX.Stop(); }
}