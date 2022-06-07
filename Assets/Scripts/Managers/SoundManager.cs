using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager m_instance;
    AudioSource m_effectsSource;
    AudioSource m_backgroundSource;
    private AudioClip[] m_audioClips;
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
        //Enemy Hit
        m_audioClips[(int)AudioClipName.ENEMY_HIT] = Resources.Load<AudioClip>("Sound/EnemyDamagedSFX");
        m_audioClips[(int)AudioClipName.ENEMY_KILL] = Resources.Load<AudioClip>("Sound/EnemyKilledSFX");
        //FB
        m_audioClips[(int)AudioClipName.FIREBALL] = Resources.Load<AudioClip>("Sound/FireballSFX");
        //Pilar --> Doors
        m_audioClips[(int)AudioClipName.PILAR] = Resources.Load<AudioClip>("Sound/PilarEmergeSFX");
        //Projectile
        m_audioClips[(int)AudioClipName.SKELLY_SHOOT] = Resources.Load<AudioClip>("Sound/SkellyShootSFX");
        //Ura Attack
        m_audioClips[(int)AudioClipName.PLAYER_ATTACK_1] = Resources.Load<AudioClip>("Sound/Slash1SFX");
        m_audioClips[(int)AudioClipName.PLAYER_ATTACK_2] = Resources.Load<AudioClip>("Sound/Slash2SFX");
        m_audioClips[(int)AudioClipName.PLAYER_ATTACK_3] = Resources.Load<AudioClip>("Sound/Slash3SFX");
        // Ura 1Heart --> NOT IN USE
        m_audioClips[(int)AudioClipName.LOW_HP] = Resources.Load<AudioClip>("Sound/Ura1HearthLeftSFX");
        //GB + Drill Stucked
        m_audioClips[(int)AudioClipName.GROUNDBREAKER] = Resources.Load<AudioClip>("Sound/UraGroundbreakerSFX");
        //Ura Hitted
        m_audioClips[(int)AudioClipName.URA_HIT_1] = Resources.Load<AudioClip>("Sound/UraCrunch1");
        m_audioClips[(int)AudioClipName.URA_HIT_2] = Resources.Load<AudioClip>("Sound/UraCrunch2");
        m_audioClips[(int)AudioClipName.URA_HIT_3] = Resources.Load<AudioClip>("Sound/UraCrunch3");
        m_audioClips[(int)AudioClipName.URA_HIT_4] = Resources.Load<AudioClip>("Sound/UraCrunch4");
        //Jump
        m_audioClips[(int)AudioClipName.JUMP] = Resources.Load<AudioClip>("Sound/jump");
        //PickUp Upgrade
        m_audioClips[(int)AudioClipName.ITEM_PICK_UP] = Resources.Load<AudioClip>("Sound/ItemPickUpSFX");
        //Dash
        m_audioClips[(int)AudioClipName.DASH] = Resources.Load<AudioClip>("Sound/UraDashSFX");
        //Spider Spawner
        m_audioClips[(int)AudioClipName.EGG_CRACK_1] = Resources.Load<AudioClip>("Sound/EggCrack1");
        m_audioClips[(int)AudioClipName.EGG_CRACK_2] = Resources.Load<AudioClip>("Sound/EggCrack2");
        //Aracne
        m_audioClips[(int)AudioClipName.SPIDER_BOSS_LOSE_LEG] = Resources.Load<AudioClip>("Sound/AracneLoseLeg");
        m_audioClips[(int)AudioClipName.SPIDER_BOSS_CRY] = Resources.Load<AudioClip>("Sound/AracneRoar");
        m_audioClips[(int)AudioClipName.SPIDER_BOSS_CRY_LITE] = Resources.Load<AudioClip>("Sound/AracneRoarLite");
        m_audioClips[(int)AudioClipName.DRILL_HIT_1] = Resources.Load<AudioClip>("Sound/DrillHitted1");
        m_audioClips[(int)AudioClipName.DRILL_HIT_2] = Resources.Load<AudioClip>("Sound/DrillHitted2");
        m_audioClips[(int)AudioClipName.DRILL_ATTACK] = Resources.Load<AudioClip>("Sound/DrillAttack");
        m_audioClips[(int)AudioClipName.DRILL_STUCKED] = Resources.Load<AudioClip>("Sound/DrillStucked");
        //Respawn
        m_audioClips[(int)AudioClipName.RESPAWN] = Resources.Load<AudioClip>("Sound/RespawnSFX");
        //Soundtracks
        m_backgroundClips[(int)BACKGROUND_CLIP.MAINTITLE] = Resources.Load<AudioClip>("Sound/MainTitleBGM");
        m_backgroundClips[(int)BACKGROUND_CLIP.FORESTOFSOULS] = Resources.Load<AudioClip>("Sound/ForestOfSoulsBGM");
        m_backgroundClips[(int)BACKGROUND_CLIP.ABANDONEDMINE] = Resources.Load<AudioClip>("Sound/AbandonedMineBGM");
        m_backgroundClips[(int)BACKGROUND_CLIP.SPIDER_BOSS] = Resources.Load<AudioClip>("Sound/AracneTheme");
        m_backgroundClips[(int)BACKGROUND_CLIP.SAMAELTHEME] = Resources.Load<AudioClip>("Sound/SamaelTheme");
        //Click
        m_audioClips[(int)AudioClipName.DIALOGUECLIC1] = Resources.Load<AudioClip>("Sound/DialogueClic1");
        m_audioClips[(int)AudioClipName.DIALOGUECLIC2] = Resources.Load<AudioClip>("Sound/DialogueClic2");
        m_audioClips[(int)AudioClipName.BUTTONSWITCH] = Resources.Load<AudioClip>("Sound/ButtonSwitch");
        m_audioClips[(int)AudioClipName.BUTTONCLICKED] = Resources.Load<AudioClip>("Sound/ButtonClicked");
        //Portal
        m_audioClips[(int)AudioClipName.PORTALUSE] = Resources.Load<AudioClip>("Sound/PortalUse");
        m_audioClips[(int)AudioClipName.PORTALOPEN] = Resources.Load<AudioClip>("Sound/PortalOpen");
        m_audioClips[(int)AudioClipName.PORTALLOOP] = Resources.Load<AudioClip>("Sound/PortalLoop");
        m_audioClips[(int)AudioClipName.PORTALCLOSE] = Resources.Load<AudioClip>("Sound/PortalClose");
        //GB Rune
        m_audioClips[(int)AudioClipName.RUNESFX] = Resources.Load<AudioClip>("Sound/RuneHum");
        //Save
        m_audioClips[(int)AudioClipName.SAVESFX] = Resources.Load<AudioClip>("Sound/GameSavedSFX");
        //Spiders
        m_audioClips[(int)AudioClipName.SPIDER_HISS_1] = Resources.Load<AudioClip>("Sound/SpiderHiss1SFX");
        m_audioClips[(int)AudioClipName.SPIDER_HISS_2] = Resources.Load<AudioClip>("Sound/SpiderHiss2SFX");
        //Ghoul
        m_audioClips[(int)AudioClipName.GHOUL_NOISE_1] = Resources.Load<AudioClip>("Sound/GhoulNoise1");
        m_audioClips[(int)AudioClipName.GHOUL_NOISE_2] = Resources.Load<AudioClip>("Sound/GhoulNoise2");
        m_audioClips[(int)AudioClipName.GHOUL_NOISE_3] = Resources.Load<AudioClip>("Sound/GhoulNoise3");
        m_audioClips[(int)AudioClipName.GHOUL_ATK_1] = Resources.Load<AudioClip>("Sound/GhoulAttack1");
        m_audioClips[(int)AudioClipName.GHOUL_ATK_2] = Resources.Load<AudioClip>("Sound/GhoulAttack2");
        m_audioClips[(int)AudioClipName.GHOUL_ATK_3] = Resources.Load<AudioClip>("Sound/GhoulAttack3");
        //Skelly
        m_audioClips[(int)AudioClipName.SKELLY_GROWL_1] = Resources.Load<AudioClip>("Sound/SkellyGrowl1");
        m_audioClips[(int)AudioClipName.SKELLY_GROWL_2] = Resources.Load<AudioClip>("Sound/SkellyGrowl2");
        //Spirits
        m_audioClips[(int)AudioClipName.SPIRIT_TRAIL] = Resources.Load<AudioClip>("Sound/SpiritTrailSFX");
        //Little Bat
        m_audioClips[(int)AudioClipName.LILBAT_SWING_1] = Resources.Load<AudioClip>("Sound/BatSwing1");
        m_audioClips[(int)AudioClipName.LILBAT_SWING_2] = Resources.Load<AudioClip>("Sound/BatSwing2");
        m_audioClips[(int)AudioClipName.LILBAT_SWING_3] = Resources.Load<AudioClip>("Sound/BatSwing3");
        //Samael
        m_audioClips[(int)AudioClipName.SAMAEL_WHISPER_1] = Resources.Load<AudioClip>("Sound/SamaelWhisper1");
        m_audioClips[(int)AudioClipName.SAMAEL_WHISPER_2] = Resources.Load<AudioClip>("Sound/SamaelWhisper2");
        m_audioClips[(int)AudioClipName.SAMAEL_WHISPER_3] = Resources.Load<AudioClip>("Sound/SamaelWhisper3");
        m_audioClips[(int)AudioClipName.SAMAEL_WHISPER_4] = Resources.Load<AudioClip>("Sound/SamaelWhisper4");

        m_audioClips[(int)AudioClipName.SAMAEL_FB_NOISE] = Resources.Load<AudioClip>("Sound/SamaelFireballSFX");
        m_audioClips[(int)AudioClipName.SAMAEL_FB_HIT] = Resources.Load<AudioClip>("Sound/SamaelFireballHitSFX");
        m_audioClips[(int)AudioClipName.SAMAEL_FB_SPAWN_1] = Resources.Load<AudioClip>("Sound/SamaelFireSpawn1");
        m_audioClips[(int)AudioClipName.SAMAEL_FB_SPAWN_2] = Resources.Load<AudioClip>("Sound/SamaelFireSpawn2");
        m_audioClips[(int)AudioClipName.SAMAEL_CIRCLE_SPAWN_1] = Resources.Load<AudioClip>("Sound/SamaelCircle1");
        m_audioClips[(int)AudioClipName.SAMAEL_CIRCLE_SPAWN_2] = Resources.Load<AudioClip>("Sound/SamaelCircle2");
        m_audioClips[(int)AudioClipName.SAMAEL_HORIZ_1] = Resources.Load<AudioClip>("Sound/SamaelHorizAtk1");
        m_audioClips[(int)AudioClipName.SAMAEL_HORIZ_2] = Resources.Load<AudioClip>("Sound/SamaelHorizAtk2");
        m_audioClips[(int)AudioClipName.SAMAEL_HORIZ_3] = Resources.Load<AudioClip>("Sound/SamaelHorizAtk3");
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

    public AudioClip ClipToPlay(AudioClipName p_name) { return m_audioClips[(int)p_name]; }
}