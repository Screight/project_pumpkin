using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected SkeletonController m_controller;
    protected StateMachine m_stateMachine;
    protected float m_startTime;

    SKELETON_STATE m_state;

    protected ANIMATION m_animation;
    protected string m_name;

    protected bool m_isActive;

    public State(StateMachine p_stateMachine, SkeletonController p_controller, string p_name, ANIMATION p_animation, SKELETON_STATE state)
    {
        m_stateMachine = p_stateMachine;
        m_controller = p_controller;
        m_name = p_name;
        m_animation = p_animation;
        m_state = state;
    }

    public virtual void Enter(bool p_changeToDefaultAnim = true)
    {
        Debug.Log("Entered " + m_name + " state.");
        DoChecks();
        // start animation
        m_startTime = Time.time;
        m_isActive = true;

        if (p_changeToDefaultAnim)
        {
            AnimationManager.Instance.PlayAnimation(m_controller.Animator, m_animation);
        }

    }
    public virtual void Exit()
    {
        m_isActive = false;
    }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks() { }

    public override string ToString()
    {
        return m_name;
    }

    public SKELETON_STATE StateEnum
    {
        get { return m_state; }
    }

    public ANIMATION Animation { get { return m_animation; } }

}