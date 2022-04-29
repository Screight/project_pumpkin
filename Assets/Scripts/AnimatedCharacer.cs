using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedCharacter : MonoBehaviour
{
    Animator m_animator;
    protected ANIMATION m_animationState;
    protected virtual void Awake() {
        m_animator = GetComponent<Animator>();
        m_animationState = ANIMATION.LAST_NO_USE;
    }

    public Animator Animator { get { return m_animator;}}
    public ANIMATION AnimationState { 
        get { return m_animationState;}
        set { m_animationState = value;}
    }

}
