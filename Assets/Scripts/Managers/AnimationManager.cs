using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    static AnimationManager m_instance;

    static public AnimationManager Instance {
        get { return m_instance;}
        private set {}
    }

    private void Awake() {
        if(m_instance == null){ m_instance = this;}
        else { Destroy(this.gameObject);}
    }

    string m_playerIdle = "idle";
    string m_playerRun  = "run";
    string m_playerDash = "dash";
    string m_playerJump = "jump";
    string m_playerFall = "fall";
    string m_playerBoost = "boost";
    string m_playerLand = "land";
    string m_playerHit = "hit";
    string m_playerGroundbreaker = "groundbreaker";
    string m_playerGroundbreakerLoop = "groundbreakerLoop";
    string m_playerDeath = "die";

    int[] m_animationHash = new int[(int)ANIMATION.LAST_NO_USE];

    private void Start() {
        m_animationHash[(int)ANIMATION.PLAYER_IDLE] = Animator.StringToHash(m_playerIdle);
        m_animationHash[(int)ANIMATION.PLAYER_RUN] = Animator.StringToHash(m_playerRun);
        m_animationHash[(int)ANIMATION.PLAYER_DASH] = Animator.StringToHash(m_playerDash);
        m_animationHash[(int)ANIMATION.PLAYER_JUMP] = Animator.StringToHash(m_playerJump);
        m_animationHash[(int)ANIMATION.PLAYER_FALL] = Animator.StringToHash(m_playerFall);
        m_animationHash[(int)ANIMATION.PLAYER_BOOST] = Animator.StringToHash(m_playerBoost);
        m_animationHash[(int)ANIMATION.PLAYER_LAND] = Animator.StringToHash(m_playerLand);
        m_animationHash[(int)ANIMATION.PLAYER_HIT] = Animator.StringToHash(m_playerHit);
        m_animationHash[(int)ANIMATION.PLAYER_GROUNDBREAKER] = Animator.StringToHash(m_playerGroundbreaker);
        m_animationHash[(int)ANIMATION.PLAYER_GROUNDBREAKER_LOOP] = Animator.StringToHash(m_playerGroundbreakerLoop);
        m_animationHash[(int)ANIMATION.PLAYER_DEATH] = Animator.StringToHash(m_playerDeath);

        m_animationHash[(int)ANIMATION.GHOUL_ATTACK] = Animator.StringToHash("Attack");
        m_animationHash[(int)ANIMATION.GHOUL_DIE] = Animator.StringToHash("Die");
        m_animationHash[(int)ANIMATION.GHOUL_HIT] = Animator.StringToHash("hit");
        m_animationHash[(int)ANIMATION.GHOUL_IDLE] = Animator.StringToHash("Idle");
        m_animationHash[(int)ANIMATION.GHOUL_MOVE] = Animator.StringToHash("Move");
        m_animationHash[(int)ANIMATION.GHOUL_CHARGE] = Animator.StringToHash("Charge");
    }    

    public void PlayAnimation(AnimatedCharacter p_animatedCharacter, ANIMATION p_animation){
        if(p_animatedCharacter.AnimationState == p_animation) return ;
        p_animatedCharacter.Animator.Play(m_animationHash[(int)p_animation]);
        p_animatedCharacter.AnimationState = p_animation;
    }

}
