using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipName { ENEMY_HIT, ENEMY_KILL, FIREBALL, PILAR, ARCHER_ATTACK, PLAYER_ATTACK_1, PLAYER_ATTACK_2, PLAYER_ATTACK_3, LOW_HP, GROUNDBREAKER, LAST_NO_USE }

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
        m_audioClips[(int)AudioClipName.ENEMY_HIT] = Resources.Load<AudioClip>("Sound/EnemyDamagedSFX");
        m_audioClips[(int)AudioClipName.ENEMY_KILL] = Resources.Load<AudioClip>("Sound/EnemyKilledSFX");
        m_audioClips[(int)AudioClipName.FIREBALL] = Resources.Load<AudioClip>("Sound/FireballSFX");
        m_audioClips[(int)AudioClipName.PILAR] = Resources.Load<AudioClip>("Sound/PilarEmergeSFX");
        m_audioClips[(int)AudioClipName.ARCHER_ATTACK] = Resources.Load<AudioClip>("Sound/SkeletonReleaseSFX");
        m_audioClips[(int)AudioClipName.PLAYER_ATTACK_1] = Resources.Load<AudioClip>("Sound/Slash1SFX");
        m_audioClips[(int)AudioClipName.PLAYER_ATTACK_2] = Resources.Load<AudioClip>("Sound/Slash2SFX");
        m_audioClips[(int)AudioClipName.PLAYER_ATTACK_3] = Resources.Load<AudioClip>("Sound/Slash3SFX");
        m_audioClips[(int)AudioClipName.LOW_HP] = Resources.Load<AudioClip>("Sound/Ura1HearthLeftSFX");
        m_audioClips[(int)AudioClipName.GROUNDBREAKER] = Resources.Load<AudioClip>("Sound/UraGroundbreakerSFX");

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
