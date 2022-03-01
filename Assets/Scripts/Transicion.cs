using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TRANSITION_ANIMATION { FADE_IN, FADE_OUT, EMPTY_SCREEN, LAST_NO_USE }

public class Transicion : MonoBehaviour
{
    [SerializeField] CameraMovement m_cameraScript;
    [SerializeField] Image m_image;
    string m_fadeIn_AnimationName = "fade_in";
    string m_fadeOutAnimationName = "fade_out";
    string m_emptyScreenAnimationName = "empty_screen";
    int[] m_animationHash = new int[(int)TRANSITION_ANIMATION.LAST_NO_USE];

    Animator m_animator;
    int m_currentState;

    private void Awake() { m_animator = GetComponent<Animator>(); }

    private void Start()
    {
        m_animationHash[(int)TRANSITION_ANIMATION.FADE_IN] = Animator.StringToHash(m_fadeIn_AnimationName);
        m_animationHash[(int)TRANSITION_ANIMATION.FADE_OUT] = Animator.StringToHash(m_fadeOutAnimationName);
        m_animationHash[(int)TRANSITION_ANIMATION.EMPTY_SCREEN] = Animator.StringToHash(m_emptyScreenAnimationName);

        ChangeAnimationState(m_animationHash[(int)TRANSITION_ANIMATION.FADE_IN]);
    }

    void ChangeAnimationState(int p_newState)
    {
        if (m_currentState == p_newState) return;   // stop the same animation from interrupting itself
        m_animator.Play(p_newState);                // play the animation
        m_currentState = p_newState;                // reassigning the new state
    }

    public void FadeIn()
    {
        CheckpointsManager.Instance.MovePlayerToLocalCheckPoint();
        m_cameraScript.SetCameraToPlayerPosition();
        ChangeAnimationState(m_animationHash[(int)TRANSITION_ANIMATION.FADE_IN]);
    }
    public void FadeOut() { ChangeAnimationState(m_animationHash[(int)TRANSITION_ANIMATION.FADE_OUT]); }
    public void LocalCheckpointTransition() { FadeOut(); }
}