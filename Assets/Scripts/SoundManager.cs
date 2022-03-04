using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipName { ENEMY_HIT, ENEMY_KILL, FIREBALL, PILAR, ARCHER_ATTACK, PLAYER_ATTACK_1, PLAYER_ATTACK_2, PLAYER_ATTACK_3, LOW_HP, GROUNDBREAKER, ITEM_PICK_UP, DASH, ARROW, LAST_NO_USE }

public class SoundManager : MonoBehaviour
{
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_generalVolume = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_background = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_enemyHit = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_enemyDeath = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_fireball = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_dashVolume = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_pilar = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_skeletonAttack = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_playerAttackOne = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_playerAttackSecond = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_playerAttackThird = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_playerLowHealth = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_groundbreaker = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_itemPickUp = 1;
    [Range(0.0F, 1.0F)]
    [SerializeField] float m_arrow = 1;

    static SoundManager m_instance;
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioSource m_backgroundSource;
    AudioClip[] m_audioClips;
    float[] m_volumeClips;

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
        m_audioClips[(int)AudioClipName.ITEM_PICK_UP] = Resources.Load<AudioClip>("Sound/UraItemPickUpSFX");
        m_audioClips[(int)AudioClipName.DASH] = Resources.Load<AudioClip>("Sound/UraDashSFX");
        m_audioClips[(int)AudioClipName.ARROW] = Resources.Load<AudioClip>("Sound/ArrowSFX");

        m_volumeClips = new float[(int)AudioClipName.LAST_NO_USE];
        m_volumeClips[(int)AudioClipName.ENEMY_HIT] = m_enemyHit;
        m_volumeClips[(int)AudioClipName.ARCHER_ATTACK] = m_skeletonAttack;
        m_volumeClips[(int)AudioClipName.ARROW] = m_arrow;
        m_volumeClips[(int)AudioClipName.DASH] = m_dashVolume;
        m_volumeClips[(int)AudioClipName.ENEMY_KILL] = m_enemyDeath;
        m_volumeClips[(int)AudioClipName.FIREBALL] = m_fireball;
        m_volumeClips[(int)AudioClipName.GROUNDBREAKER] = m_groundbreaker;
        m_volumeClips[(int)AudioClipName.ITEM_PICK_UP] = m_itemPickUp;
        m_volumeClips[(int)AudioClipName.LOW_HP] = m_playerLowHealth;
        m_volumeClips[(int)AudioClipName.PILAR] = m_pilar;
        m_volumeClips[(int)AudioClipName.PLAYER_ATTACK_1] = m_playerAttackOne;
        m_volumeClips[(int)AudioClipName.PLAYER_ATTACK_2] = m_playerAttackSecond;
        m_volumeClips[(int)AudioClipName.PLAYER_ATTACK_3] = m_playerAttackThird;
    }

    public void PlayOnce(AudioClipName p_name)
    {
        m_audioSource.volume = m_generalVolume * m_volumeClips[(int)p_name];
        m_audioSource.PlayOneShot(m_audioClips[(int)p_name]);
    }

    public void PlayBackground(AudioClipName p_name)
    {
        m_audioSource.volume = m_background;
        m_audioSource.Play();

    }

}
