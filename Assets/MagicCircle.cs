using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] AudioClip[] m_spawnSound;
    AudioSource m_audioSrc;
    private void Awake()
    {
        m_audioSrc = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        int randNum = Random.Range(0, 2);
        if (randNum == 0) { m_audioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SAMAEL_CIRCLE_SPAWN_1)); }
        else if (randNum == 1) { m_audioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SAMAEL_CIRCLE_SPAWN_2)); }
    }
}