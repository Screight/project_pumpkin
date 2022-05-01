using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum TRANSITION_ANIMATION { FADE_IN, FADE_OUT, EMPTY_SCREEN, LAST_NO_USE }

public class Transicion : MonoBehaviour
{
    string m_fadeIn_AnimationName = "fade_in";
    string m_fadeOutAnimationName = "fade_out";
    string m_emptyScreenAnimationName = "empty_screen";
    int[] m_animationHash = new int[(int)TRANSITION_ANIMATION.LAST_NO_USE];

    Timer m_scriptPlayerTimer;
    bool m_isBeingScripted = false;

    Animator m_animator;
    int m_currentState;

    UnityEvent m_endOfTransitionEvent;
    UnityEvent m_endOfFadeInEvent;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_scriptPlayerTimer = gameObject.AddComponent<Timer>();
        m_endOfFadeInEvent = new UnityEvent();
        m_endOfTransitionEvent = new UnityEvent();
    }

    private void Start()
    {
        m_animationHash[(int)TRANSITION_ANIMATION.FADE_IN] = Animator.StringToHash(m_fadeIn_AnimationName);
        m_animationHash[(int)TRANSITION_ANIMATION.FADE_OUT] = Animator.StringToHash(m_fadeOutAnimationName);
        m_animationHash[(int)TRANSITION_ANIMATION.EMPTY_SCREEN] = Animator.StringToHash(m_emptyScreenAnimationName);
    }

    private void Update()
    {
        /*if (m_isBeingScripted && m_scriptPlayerTimer.IsFinished)
        {
            Player.Instance.StopScripting();
            m_isBeingScripted = false;
        }*/
    }

    void ChangeAnimationState(TRANSITION_ANIMATION p_newState)
    {
        if (m_currentState == m_animationHash[(int)p_newState]) { return; }   // stop the same animation from interrupting itself
        m_animator.Play(m_animationHash[(int)p_newState]);                // play the animation
        m_currentState = m_animationHash[(int)p_newState];                // reassigning the new state
    }

    public void FadeIn()
    {
        CameraManager.Instance.SetCameraToStatic();
        ChangeAnimationState(TRANSITION_ANIMATION.FADE_IN);
    }

    private void ClampCamera()
    {
        m_endOfFadeInEvent.Invoke();
    }

    private void EndTransicion()
    {
        m_currentState = m_animationHash[(int)TRANSITION_ANIMATION.EMPTY_SCREEN];
        RoomManager.Instance.StartPlayerScripting();
        CameraManager.Instance.SetCameraToNormal();
        m_endOfTransitionEvent.Invoke();
    }

    public void AddListenerToEndOfTransition(UnityAction p_function){
        m_endOfTransitionEvent.AddListener(p_function);
    }

    public void RemoveListenerToEndOfTransition(UnityAction p_function){
        m_endOfTransitionEvent.RemoveListener(p_function);
    }

    public void AddListenerToEndOfFadeIn(UnityAction p_function){
        m_endOfFadeInEvent.AddListener(p_function);
    }

    public void RemoveListenerToEndOfFadeIn(UnityAction p_function){
        m_endOfFadeInEvent.RemoveListener(p_function);
    }

}