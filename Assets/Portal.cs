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
    protected override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animator>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_transicion = GameObject.FindObjectOfType<Transicion>();
        m_map = GameObject.FindObjectOfType<MiniMap>();
    }

    private void Start() {
        m_renderer.enabled = false;
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
        AnimationManager.Instance.PlayAnimation(m_animator, ANIMATION.PORTAL_OPEN);
    }

    public void ClosePortal(){
        m_renderer.enabled = true;
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
