using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager m_instance;
    [SerializeField] AudioSource m_effectsSource;
    [SerializeField] AudioSource m_backgroundSource;
    AudioClip[] m_audioClips;
    AudioClip[] m_backgroundClips;

    // Start is called before the first frame update
    private void Awake()
    {
        if (m_instance == null) { m_instance = this; Initiate(); }
        else { Destroy(this.gameObject); }
    }

    private void Start()
    {
        
    }

    public static SoundManager Instance { get { return m_instance; } private set { } }

    void Initiate()
    {
        m_backgroundSource.loop = true;
        m_effectsSource.loop = false;

        m_audioClips = new AudioClip[(int)AudioClipName.LAST_NO_USE];
        m_backgroundClips = new AudioClip[(int)BACKGROUND_CLIP.LAST_NO_USE];

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

        m_audioClips[(int)AudioClipName.EGG_CRACK_1] = Resources.Load<AudioClip>("Sound/EggCrack1");
        m_audioClips[(int)AudioClipName.EGG_CRACK_2] = Resources.Load<AudioClip>("Sound/EggCrack2");

        m_audioClips[(int)AudioClipName.SPIDER_BOSS_LOSE_LEG] = Resources.Load<AudioClip>("Sound/AracneLoseLeg");
        m_audioClips[(int)AudioClipName.SPIDER_BOSS_CRY] = Resources.Load<AudioClip>("Sound/AracneRoar");
        m_audioClips[(int)AudioClipName.DRILL_HIT_1] = Resources.Load<AudioClip>("Sound/DrillHitted1");
        m_audioClips[(int)AudioClipName.DRILL_HIT_2] = Resources.Load<AudioClip>("Sound/DrillHitted2");

        m_backgroundClips[(int)BACKGROUND_CLIP.BACKGROUND_1] = Resources.Load<AudioClip>("Sound/haunted");
        m_backgroundClips[(int)BACKGROUND_CLIP.SPIDER_BOSS] = Resources.Load<AudioClip>("Sound/BattleBGM2");

    }

    public void PlayOnce(AudioClipName p_name)
    {
        m_effectsSource.PlayOneShot(m_audioClips[(int)p_name]);   
    }

    public void PlayBackground(BACKGROUND_CLIP p_name)
    {
        m_backgroundSource.clip = m_backgroundClips[(int)p_name];
        m_backgroundSource.Play();
    }

    public void StopBackground(){
        m_backgroundSource.Stop();
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
