using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : InteractiveItem
{
    [SerializeField] Portal m_destinyPortal;
    [SerializeField] Transform m_playerPosition;
    Transicion m_transicion;
    [SerializeField] ZONE m_zone = ZONE.MINE;
    Animator m_animator;
    SpriteRenderer m_renderer;
    MiniMap m_map;

    Timer m_eventTimer;
    float m_closeDuration;
    bool m_isPortalClosing;
    protected override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animator>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_transicion = GameObject.FindObjectOfType<Transicion>();
        m_map = GameObject.FindObjectOfType<MiniMap>();
        m_eventTimer = gameObject.AddComponent<Timer>();
    }

    private void Start() {
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
        SoundManager.Instance.PlayOnce(AudioClipName.RESPAWN);
    }

    public void OpenPortal(){
        m_renderer.enabled = true;
        m_isPortalClosing = false;
        AnimationManager.Instance.PlayAnimation(m_animator, ANIMATION.PORTAL_OPEN);
    }

    public void ClosePortal(){
        m_isPortalClosing = true;
        m_eventTimer.Restart();
        AnimationManager.Instance.PlayAnimation(m_animator, ANIMATION.PORTAL_CLOSE);
    }

    public void TransportPlayer(){
        Player.Instance.transform.position = new Vector3(m_destinyPortal.SpawnPosition.x, m_destinyPortal.SpawnPosition.y, Player.Instance.transform.position.z);
        m_transicion.RemoveListenerToEndOfFadeIn(TransportPlayer);
        GameManager.Instance.CurrentZone = m_destinyPortal.Zone;
    }

    public void EndTransportPlayer(){
        Player.Instance.StopScripting();
        GameManager.Instance.IsGamePaused = false;
        m_transicion.RemoveListenerToEndOfTransition(EndTransportPlayer);
    }

    public Vector3 SpawnPosition{
        get { return m_playerPosition.position; }
    }

    public ZONE Zone {
        get { return m_zone; }
    }

}
