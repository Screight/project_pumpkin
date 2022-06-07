using UnityEngine;
using UnityEngine.UI;

public class HandleSettings : MonoBehaviour
{
    private static HandleSettings m_instance;
    [SerializeField] Slider m_effectsVolumeLevel;
    [SerializeField] Slider m_backgroundVolumeLevel;
    [SerializeField] Sprite m_greenFill;
    [SerializeField] Sprite m_darkGreenFill;
    [SerializeField] Image[] m_fillSlider;

    private void Awake()
    {
        if (m_instance == null) { m_instance = this; }
        else { Destroy(this); }
    }
    public static HandleSettings Instance { get { return m_instance; } private set { } }

    private void Start()
    {
        if (m_effectsVolumeLevel != null) { m_effectsVolumeLevel.value = SoundManager.Instance.EffectVolume; }
        if (m_backgroundVolumeLevel != null) { m_backgroundVolumeLevel.value = SoundManager.Instance.BackgroundVolume; }
    }

    public void SetBackgroundVolume()
    {
        if (m_backgroundVolumeLevel != null) { SoundManager.Instance.SetBackgroundVolume(m_backgroundVolumeLevel.value); }
    }

    public void SetEffectsVolume()
    {
        if (m_effectsVolumeLevel != null) { SoundManager.Instance.SetEffectsVolume(m_effectsVolumeLevel.value); }
    }

    public float getCurrentBgVolume { get { return m_backgroundVolumeLevel.value; } }
}