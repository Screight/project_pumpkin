using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    State m_currentState;
    SkeletonController m_controller;

    public void Initialize(State p_startingState, SkeletonController p_controller, bool p_changeToDefaultAnim = true)
    {
        m_currentState = p_startingState;
        m_controller = p_controller;
        m_currentState.Enter(p_changeToDefaultAnim);
    }

    public void ChangeState(State p_newState, bool p_changeToDefaultAnim = true)
    {
        if(m_currentState != null) { m_currentState.Exit(); }
        m_currentState = p_newState;
        if (p_changeToDefaultAnim) { m_controller.AnimationState = m_currentState.Animation; }
        m_currentState.Enter(p_changeToDefaultAnim);
    }

    public State CurrentState
    {
        get { return m_currentState; }
        set { m_currentState = value; }
    }

}