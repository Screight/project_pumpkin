using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    static AnimationManager m_instance;

    static public AnimationManager Instance
    {
        get { return m_instance; }
        private set { }
    }

    private void Awake()
    {
        if (m_instance == null) { m_instance = this; }
        else { Destroy(gameObject); }
    }

    int[] m_animationHash = new int[(int)ANIMATION.LAST_NO_USE];
    string[] m_animationClipName = new string[(int)ANIMATION.LAST_NO_USE];

    private void OnEnable()
    {
        m_animationClipName[(int)ANIMATION.PLAYER_IDLE]                 = "idle";
        m_animationClipName[(int)ANIMATION.PLAYER_RUN]                  = "run";
        m_animationClipName[(int)ANIMATION.PLAYER_DASH]                 = "dash";
        m_animationClipName[(int)ANIMATION.PLAYER_JUMP]                 = "jump";
        m_animationClipName[(int)ANIMATION.PLAYER_FALL]                 = "fall";
        m_animationClipName[(int)ANIMATION.PLAYER_BOOST]                = "boost";
        m_animationClipName[(int)ANIMATION.PLAYER_LAND]                 = "land";
        m_animationClipName[(int)ANIMATION.PLAYER_HIT]                  = "hit";
        m_animationClipName[(int)ANIMATION.PLAYER_GROUNDBREAKER]        = "groundbreaker";
        m_animationClipName[(int)ANIMATION.PLAYER_GROUNDBREAKER_LOOP]   = "groundbreakerLoop";
        m_animationClipName[(int)ANIMATION.PLAYER_DEATH]                = "die";

        m_animationClipName[(int)ANIMATION.GHOUL_ATTACK]    = "Attack";
        m_animationClipName[(int)ANIMATION.GHOUL_DIE]       = "Die";
        m_animationClipName[(int)ANIMATION.GHOUL_HIT]       = "hit";
        m_animationClipName[(int)ANIMATION.GHOUL_IDLE]      = "Idle";
        m_animationClipName[(int)ANIMATION.GHOUL_MOVE]      = "Move";
        m_animationClipName[(int)ANIMATION.GHOUL_CHARGE]    = "Charge";
    }

    private void Start()
    {
        m_animationHash[(int)ANIMATION.PLAYER_IDLE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_IDLE]);
        m_animationHash[(int)ANIMATION.PLAYER_RUN] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_RUN]);
        m_animationHash[(int)ANIMATION.PLAYER_DASH] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_DASH]);
        m_animationHash[(int)ANIMATION.PLAYER_JUMP] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_JUMP]);
        m_animationHash[(int)ANIMATION.PLAYER_FALL] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_FALL]);
        m_animationHash[(int)ANIMATION.PLAYER_BOOST] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_BOOST]);
        m_animationHash[(int)ANIMATION.PLAYER_LAND] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_LAND]);
        m_animationHash[(int)ANIMATION.PLAYER_HIT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_HIT]);
        m_animationHash[(int)ANIMATION.PLAYER_GROUNDBREAKER] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_GROUNDBREAKER]);
        m_animationHash[(int)ANIMATION.PLAYER_GROUNDBREAKER_LOOP] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_GROUNDBREAKER_LOOP]);
        m_animationHash[(int)ANIMATION.PLAYER_DEATH] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PLAYER_DEATH]);

        m_animationHash[(int)ANIMATION.GHOUL_ATTACK] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_ATTACK]);
        m_animationHash[(int)ANIMATION.GHOUL_DIE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_DIE]);
        m_animationHash[(int)ANIMATION.GHOUL_HIT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_HIT]);
        m_animationHash[(int)ANIMATION.GHOUL_IDLE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_IDLE]);
        m_animationHash[(int)ANIMATION.GHOUL_MOVE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_MOVE]);
        m_animationHash[(int)ANIMATION.GHOUL_CHARGE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_CHARGE]);
    }

    public void PlayAnimation(AnimatedCharacter p_animatedCharacter, ANIMATION p_animation)
    {
        if (p_animatedCharacter.AnimationState == p_animation) { return; }
        p_animatedCharacter.Animator.Play(m_animationHash[(int)p_animation]);
        p_animatedCharacter.AnimationState = p_animation;
    }

    public void PlayAnimation(Animator p_animator, ANIMATION p_animation)
    {
        p_animator.Play(m_animationHash[(int)p_animation]);
    }

    public float GetClipDuration(AnimatedCharacter p_animatedCharacter, ANIMATION p_animation)
    {
        foreach (AnimationClip animationClip in p_animatedCharacter.Animator.runtimeAnimatorController.animationClips)
        {
            if (animationClip.name == m_animationClipName[(int)p_animation])
            {
                return animationClip.length;
            }
        }
        return -1;
    }
}