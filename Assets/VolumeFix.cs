using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeFix : MonoBehaviour
{
    AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        m_audioSource.volume = SoundManager.Instance.EffectVolume;
    }
}
