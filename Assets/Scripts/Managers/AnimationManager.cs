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

    private void OnEnable() {

        // PLAYER ------------------------
        m_animationClipName[(int)ANIMATION.PLAYER_IDLE] = "idle";
        m_animationClipName[(int)ANIMATION.PLAYER_RUN] = "run";
        m_animationClipName[(int)ANIMATION.PLAYER_DASH] = "dash";
        m_animationClipName[(int)ANIMATION.PLAYER_JUMP] = "jump";
        m_animationClipName[(int)ANIMATION.PLAYER_FALL] = "fall";
        m_animationClipName[(int)ANIMATION.PLAYER_BOOST] = "boost";
        m_animationClipName[(int)ANIMATION.PLAYER_LAND] = "land";
        m_animationClipName[(int)ANIMATION.PLAYER_HIT] = "hit";
        m_animationClipName[(int)ANIMATION.PLAYER_GROUNDBREAKER] = "groundbreaker";
        m_animationClipName[(int)ANIMATION.PLAYER_GROUNDBREAKER_LOOP] = "groundbreakerLoop";
        m_animationClipName[(int)ANIMATION.PLAYER_DEATH] = "die";

        // GHOUL --------------------------
        m_animationClipName[(int)ANIMATION.GHOUL_ATTACK] = "Attack";
        m_animationClipName[(int)ANIMATION.GHOUL_DIE] = "Die";
        m_animationClipName[(int)ANIMATION.GHOUL_HIT] = "hit";
        m_animationClipName[(int)ANIMATION.GHOUL_IDLE] = "Idle";
        m_animationClipName[(int)ANIMATION.GHOUL_MOVE] = "Move";
        m_animationClipName[(int)ANIMATION.GHOUL_CHARGE] = "Charge";

        // FLYING ENEMY CHARGE ------------
        m_animationClipName[(int)ANIMATION.CHARGE_BAT_MOVE] = "move";
        m_animationClipName[(int)ANIMATION.CHARGE_BAT_PREPARE_ATTACK] = "prepare_attack";
        m_animationClipName[(int)ANIMATION.CHARGE_BAT_ATTACK] = "attack";
        m_animationClipName[(int)ANIMATION.CHARGE_BAT_RECOVER_FROM_ATTACK] = "recover_from_attack";
        m_animationClipName[(int)ANIMATION.CHARGE_BAT_HIT] = "hit";
        m_animationClipName[(int)ANIMATION.CHARGE_BAT_DIE] = "die";

        // SKELETON ------------------------
        m_animationClipName[(int)ANIMATION.SKELETON_ATTACK] = "attack";
        m_animationClipName[(int)ANIMATION.SKELETON_DIE] = "Die";
        m_animationClipName[(int)ANIMATION.SKELETON_FIRE] = "Fire";
        m_animationClipName[(int)ANIMATION.SKELETON_HIT] = "hit";
        m_animationClipName[(int)ANIMATION.SKELETON_MOVE] = "Move";
        m_animationClipName[(int)ANIMATION.SKELETON_RELOAD] = "Reload";

         // PATROL BAT ------------------------
        m_animationClipName[(int)ANIMATION.PATROL_BAT_DIE] = "die";
        m_animationClipName[(int)ANIMATION.PATROL_BAT_HIT] = "hit";
        m_animationClipName[(int)ANIMATION.PATROL_BAT_MOVE] = "move";

        // SPIDER BOSS ------------------------
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_IDLE] = "idle";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ROAR] = "roar";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ROAR_LOOP] = "roarLoop";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_MOVE_LEFT] = "moveLeft";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_MOVE_RIGHT] = "moveRight";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ATTACK_LEFT] = "atkLeftCenter";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ATTACK_RIGHT] = "atkRightCenter";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_LEFT] = "trappedRecoverLeft";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_RIGHT] = "trappedRecoverRight";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_LEFT] = "normalRecoverLeft";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_RIGHT] = "normalRecoverRight";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ATTACK_BITE] = "headAtk";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ATTACK_SPIT] = "spitWeb";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_MOVE] = "move";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_LOSE_LEFT_LEG] = "lose_Left_leg";
        m_animationClipName[(int)ANIMATION.SPIDER_BOSS_LOSE_RIGHT_LEG] = "lose_Right_leg";
        
        // SPIDER -------------------------
        m_animationClipName[(int)ANIMATION.SPIDER_WALK] = "spider_Walk";
        m_animationClipName[(int)ANIMATION.SPIDER_EGG] = "spiderEgg";

        // EFFECTS -------------------------
            // ACID BALL
        m_animationClipName[(int)ANIMATION.EFFECT_ACID_LOOP] = "acid_ball_loop";
        m_animationClipName[(int)ANIMATION.EFFECT_ACID_EXPLOSION] = "acid_ball_explosion";

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

        // GHOUL ----------------------------
        m_animationHash[(int)ANIMATION.GHOUL_ATTACK] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_ATTACK]);
        m_animationHash[(int)ANIMATION.GHOUL_DIE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_DIE]);
        m_animationHash[(int)ANIMATION.GHOUL_HIT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_HIT]);
        m_animationHash[(int)ANIMATION.GHOUL_IDLE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_IDLE]);
        m_animationHash[(int)ANIMATION.GHOUL_MOVE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_MOVE]);
        m_animationHash[(int)ANIMATION.GHOUL_CHARGE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.GHOUL_CHARGE]);

        // CHARGE BAT ----------------------------
        m_animationHash[(int)ANIMATION.CHARGE_BAT_ATTACK] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.CHARGE_BAT_ATTACK]);
        m_animationHash[(int)ANIMATION.CHARGE_BAT_DIE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.CHARGE_BAT_DIE]);
        m_animationHash[(int)ANIMATION.CHARGE_BAT_HIT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.CHARGE_BAT_HIT]);
        m_animationHash[(int)ANIMATION.CHARGE_BAT_MOVE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.CHARGE_BAT_MOVE]);
        m_animationHash[(int)ANIMATION.CHARGE_BAT_PREPARE_ATTACK] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.CHARGE_BAT_PREPARE_ATTACK]);
        m_animationHash[(int)ANIMATION.CHARGE_BAT_RECOVER_FROM_ATTACK] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.CHARGE_BAT_RECOVER_FROM_ATTACK]);

        // SKELETON ----------------------------
        m_animationHash[(int)ANIMATION.SKELETON_ATTACK] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SKELETON_ATTACK]);
        m_animationHash[(int)ANIMATION.SKELETON_DIE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SKELETON_DIE]);
        m_animationHash[(int)ANIMATION.SKELETON_FIRE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SKELETON_FIRE]);
        m_animationHash[(int)ANIMATION.SKELETON_HIT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SKELETON_HIT]);
        m_animationHash[(int)ANIMATION.SKELETON_MOVE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SKELETON_MOVE]);
        m_animationHash[(int)ANIMATION.SKELETON_RELOAD] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SKELETON_RELOAD]);

        // PATROL BAT ----------------------------
        m_animationHash[(int)ANIMATION.PATROL_BAT_DIE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PATROL_BAT_DIE]);
        m_animationHash[(int)ANIMATION.PATROL_BAT_HIT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PATROL_BAT_HIT]);
        m_animationHash[(int)ANIMATION.PATROL_BAT_MOVE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.PATROL_BAT_MOVE]);

        // SPIDER BOSS ---------------------------
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_IDLE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_IDLE]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_ROAR] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ROAR]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_ROAR_LOOP] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ROAR_LOOP]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_MOVE_LEFT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_MOVE_LEFT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_MOVE_RIGHT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_MOVE_RIGHT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_ATTACK_LEFT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ATTACK_LEFT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_ATTACK_RIGHT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ATTACK_RIGHT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_LEFT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_LEFT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_RIGHT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_RECOVER_TERRAIN_RIGHT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_LEFT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_LEFT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_RIGHT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_RECOVER_NORMAL_RIGHT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_ATTACK_BITE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ATTACK_BITE]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_ATTACK_SPIT] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_ATTACK_SPIT]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_MOVE] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_MOVE]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_LOSE_LEFT_LEG] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_LOSE_LEFT_LEG]);
        m_animationHash[(int)ANIMATION.SPIDER_BOSS_LOSE_RIGHT_LEG] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_BOSS_LOSE_RIGHT_LEG]);

        // SPIDER ---------------------------
        m_animationHash[(int)ANIMATION.SPIDER_WALK] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_WALK]);
        m_animationHash[(int)ANIMATION.SPIDER_EGG] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.SPIDER_EGG]);

        // EFFECTS ---------------------------
            // ACID BALL
        m_animationHash[(int)ANIMATION.EFFECT_ACID_LOOP] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.EFFECT_ACID_LOOP]);
        m_animationHash[(int)ANIMATION.EFFECT_ACID_EXPLOSION] = Animator.StringToHash(m_animationClipName[(int)ANIMATION.EFFECT_ACID_EXPLOSION]);
    }    

    public void PlayAnimation(AnimatedCharacter p_animatedCharacter, ANIMATION p_animation, bool p_startAgainIfSameAnimation){
        if( !p_startAgainIfSameAnimation && p_animatedCharacter.AnimationState == p_animation) return ;

        if(p_animatedCharacter.AnimationState == p_animation){
            p_animatedCharacter.Animator.Play(m_animationHash[(int)p_animation], -1, 0f);
            return;
        }
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