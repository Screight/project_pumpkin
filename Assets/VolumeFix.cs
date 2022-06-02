using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeFix : MonoBehaviour
{
    AudioSource[] m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AudioSource m_audio in m_audioSource) { m_audio.volume = SoundManager.Instance.EffectVolume; }
    }
}
