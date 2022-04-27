using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipName { ENEMY_HIT, ENEMY_KILL, FIREBALL, PILAR, ARCHER_ATTACK, PLAYER_ATTACK_1, PLAYER_ATTACK_2, PLAYER_ATTACK_3, LOW_HP, GROUNDBREAKER, ITEM_PICK_UP, DASH, ARROW, LAST_NO_USE }

public class SoundManager : MonoBehaviour
{
    static SoundManager m_instance;
    [SerializeField] AudioSource m_effectsSource;
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
        m_effectsSource.loop = false;
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
        m_audioClips[(int)AudioClipName.ITEM_PICK_UP] = Resources.Load<AudioClip>("Sound/UraItemPickUpSFX");
        m_audioClips[(int)AudioClipName.DASH] = Resources.Load<AudioClip>("Sound/UraDashSFX");
        m_audioClips[(int)AudioClipName.ARROW] = Resources.Load<AudioClip>("Sound/ArrowSFX");
    }

    public void PlayOnce(AudioClipName p_name)
    {
        m_effectsSource.PlayOneShot(m_audioClips[(int)p_name]);   
    }

    public void PlayBackground(BACKGROUND_CLIP p_name)
    {
        m_effectsSource.Play();
    }

    public void SetBackgroundVolume(float p_volume){
        m_backgroundSource.volume = p_volume; 
    }

    public void SetEffectsVolume(float p_volume){
        m_effectsSource.volume = p_volume;
    }

    public float EffectVolume { get { return m_effectsSource.volume; }}
    public float BackgroundVolume { get { return m_backgroundSource.volume; }}

}
