using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager m_instance;
    AudioSource m_effectsSource;
    AudioSource m_backgroundSource;
    AudioClip[] m_audioClips;
    AudioClip[] m_backgroundClips;
    [SerializeField] float m_startBackgroundVolume = 0.5f;
    [SerializeField] float m_startEffectsVolume = 0.5f;

    private void Awake()
    {
        if (m_instance == null) { m_instance = this; Initiate(); }
        else { Destroy(this); }
    }

    public static SoundManager Instance { get { return m_instance; } private set { } }

    void Initiate()
    {
        m_backgroundSource = gameObject.AddComponent<AudioSource>();
        m_effectsSource = gameObject.AddComponent<AudioSource>();

        m_backgroundSource.volume = m_startBackgroundVolume;
        m_effectsSource.volume = m_startEffectsVolume;

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
        m_audioClips[(int)AudioClipName.URA_HIT_1] = Resources.Load<AudioClip>("Sound/UraCrunch1");
        m_audioClips[(int)AudioClipName.URA_HIT_2] = Resources.Load<AudioClip>("Sound/UraCrunch2");
        m_audioClips[(int)AudioClipName.URA_HIT_3] = Resources.Load<AudioClip>("Sound/UraCrunch3");
        m_audioClips[(int)AudioClipName.URA_HIT_4] = Resources.Load<AudioClip>("Sound/UraCrunch4");
        m_audioClips[(int)AudioClipName.JUMP] = Resources.Load<AudioClip>("Sound/jump");


        m_audioClips[(int)AudioClipName.ITEM_PICK_UP] = Resources.Load<AudioClip>("Sound/ItemPickUpSFX");
        m_audioClips[(int)AudioClipName.DASH] = Resources.Load<AudioClip>("Sound/UraDashSFX");
        m_audioClips[(int)AudioClipName.ARROW] = Resources.Load<AudioClip>("Sound/ArrowSFX");

        m_audioClips[(int)AudioClipName.EGG_CRACK_1] = Resources.Load<AudioClip>("Sound/EggCrack1");
        m_audioClips[(int)AudioClipName.EGG_CRACK_2] = Resources.Load<AudioClip>("Sound/EggCrack2");

        m_audioClips[(int)AudioClipName.SPIDER_BOSS_LOSE_LEG] = Resources.Load<AudioClip>("Sound/AracneLoseLeg");
        m_audioClips[(int)AudioClipName.SPIDER_BOSS_CRY] = Resources.Load<AudioClip>("Sound/AracneRoar");
        m_audioClips[(int)AudioClipName.SPIDER_BOSS_CRY_LITE] = Resources.Load<AudioClip>("Sound/AracneRoarLite");
        m_audioClips[(int)AudioClipName.DRILL_HIT_1] = Resources.Load<AudioClip>("Sound/DrillHitted1");
        m_audioClips[(int)AudioClipName.DRILL_HIT_2] = Resources.Load<AudioClip>("Sound/DrillHitted2");
        m_audioClips[(int)AudioClipName.DRILL_ATTACK] = Resources.Load<AudioClip>("Sound/DrillAttack");
        m_audioClips[(int)AudioClipName.DRILL_STUCKED] = Resources.Load<AudioClip>("Sound/DrillStucked");

        m_audioClips[(int)AudioClipName.RESPAWN] = Resources.Load<AudioClip>("Sound/RespawnSFX");

        m_backgroundClips[(int)BACKGROUND_CLIP.BACKGROUND_1] = Resources.Load<AudioClip>("Sound/haunted");
        m_backgroundClips[(int)BACKGROUND_CLIP.SPIDER_BOSS] = Resources.Load<AudioClip>("Sound/AracneTheme");

        m_audioClips[(int)AudioClipName.DIALOGUECLIC1] = Resources.Load<AudioClip>("Sound/DialogueClic1");
        m_audioClips[(int)AudioClipName.DIALOGUECLIC2] = Resources.Load<AudioClip>("Sound/DialogueClic2");
        m_audioClips[(int)AudioClipName.BUTTONSWITCH] = Resources.Load<AudioClip>("Sound/ButtonSwitch");
        m_audioClips[(int)AudioClipName.BUTTONCLICKED] = Resources.Load<AudioClip>("Sound/ButtonClicked");

        m_audioClips[(int)AudioClipName.PORTALUSE] = Resources.Load<AudioClip>("Sound/PortalUse");
        m_audioClips[(int)AudioClipName.PORTALOPEN] = Resources.Load<AudioClip>("Sound/PortalOpen");
        m_audioClips[(int)AudioClipName.PORTALLOOP] = Resources.Load<AudioClip>("Sound/PortalLoop");
        m_audioClips[(int)AudioClipName.PORTALCLOSE] = Resources.Load<AudioClip>("Sound/PortalClose");
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

    public void StopBackground() { m_backgroundSource.Stop(); }

    public void SetBackgroundVolume(float p_volume)
    {
        if (m_backgroundSource == null) { return; }
        m_backgroundSource.volume = p_volume;
    }

    public void SetEffectsVolume(float p_volume)
    {
        if (m_effectsSource == null) { return; }
        m_effectsSource.volume = p_volume;
    }

    public float EffectVolume { get { return m_effectsSource.volume; }}
    public float BackgroundVolume { get { return m_backgroundSource.volume; }}
}