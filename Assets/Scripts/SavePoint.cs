using UnityEngine;

public class SavePoint : InteractiveItem
{
    [SerializeField] Transform m_respawnPoint;
    [SerializeField] ParticleSystem m_sparkles;
    private Timer m_saveCooldown;
    [SerializeField] SavePointText m_savePanel;
    [SerializeField] BACKGROUND_CLIP m_music;
    [SerializeField] ZONE m_zone;
    protected override void Awake() 
    {
        base.Awake();
        if (m_savePanel == null) { Debug.Log("Save: Dame la refe del panel de Saved! >:("); }
    }

    private void Start()
    {
        m_sparkles = GetComponentInChildren<ParticleSystem>();
        m_saveCooldown = gameObject.AddComponent<Timer>();
        m_saveCooldown.Duration = 2.0f;
    }
    protected override void Update()
    {
        base.Update();
        if (m_icon != null)
        {
            if (m_saveCooldown.IsFinished && !m_icon.activeInHierarchy) { m_icon.SetActive(true); }
            else if (m_saveCooldown.IsRunning && m_icon.activeInHierarchy) { m_icon.SetActive(false); }
        }
    }
    protected override void HandleInteraction()
    {
        if (m_saveCooldown.IsFinished)
        {
            base.HandleInteraction();
            m_sparkles.Play();
            m_savePanel.ActivateText();
            SoundManager.Instance.PlayOnce(AudioClipName.SAVESFX);
            CheckpointsManager.Instance.SetGlobalCheckPoint(m_respawnPoint);
            Debug.Log("<color=red>Saved!</color>");
            GameManager.Instance.RestorePlayerToFullHealth();
            CheckpointsManager.Instance.Music = m_music;
            CheckpointsManager.Instance.Zone = m_zone;
            SaveLoadGame.Instance.Save();
            m_saveCooldown.Run();
        }
    }
}