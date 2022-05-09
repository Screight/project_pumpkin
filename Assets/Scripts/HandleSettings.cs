using UnityEngine;
using UnityEngine.UI;

public class HandleSettings : MonoBehaviour
{
    [SerializeField] Slider m_effectsVolumeLevel;
    [SerializeField] Slider m_backgroundVolumeLevel;
    [SerializeField] Sprite m_greenFill;
    [SerializeField] Sprite m_darkGreenFill;
    [SerializeField] Image[] m_fillSlider;

    private void Start()
    {
        m_effectsVolumeLevel.value = SoundManager.Instance.EffectVolume;
        m_backgroundVolumeLevel.value = SoundManager.Instance.BackgroundVolume;
    }

    public void SetBackgroundVolume()
    {
        SoundManager.Instance.SetBackgroundVolume(m_backgroundVolumeLevel.value);
    }

    public void SetEffectsVolume()
    {
        SoundManager.Instance.SetEffectsVolume(m_effectsVolumeLevel.value);
    }
}