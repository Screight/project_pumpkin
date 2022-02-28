using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipName { WALK, LAST_NO_USE }

public class SoundManager : MonoBehaviour
{
    static SoundManager m_instance;
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioSource m_backgroundSource;
    AudioClip[] m_audioClips;



    // Start is called before the first frame update
    private void Awake()
    {
        if (m_instance == null) { m_instance = this; Initiate(); }
        else { Destroy(this.gameObject); }
    }

    private void Start()
    {
        m_backgroundSource.loop = true;
        m_audioSource.loop = false;
    }

    public static SoundManager Instance { get { return m_instance; } private set { } }

    void Initiate()
    {
        m_audioClips = new AudioClip[(int)AudioClipName.LAST_NO_USE];
        m_audioClips[(int)AudioClipName.WALK] = Resources.Load<AudioClip>("Sound/UraWalkFX");

    }

    public void PlayOnce(AudioClipName p_name, float p_volumeLevel)
    {
        m_audioSource.volume = p_volumeLevel;
        m_audioSource.PlayOneShot(m_audioClips[(int)p_name]);

    }

    public void PlayBackground(AudioClipName p_name, float p_volumeLevel)
    {
        m_audioSource.volume = p_volumeLevel;
        m_audioSource.Play();

    }

}
