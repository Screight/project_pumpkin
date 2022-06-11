using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerNewController m_player;
    protected PlayerStateMachine m_stateMachine;
    protected PlayerData m_playerData;
    protected float m_startTime;

    private string m_animBoolName;

    public PlayerState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName){
        m_player = p_player;
        m_stateMachine = p_stateMachine;
        m_playerData = p_playerData;
        m_animBoolName = p_animBoolName;
    }

    public virtual void Enter(){
        DoChecks();
        // start animation
        m_startTime = Time.time;
        m_player.Animator.SetBool(m_animBoolName,true);
        Debug.Log(m_animBoolName);
    }
    public virtual void Exit(){
        // stop animation
        m_player.Animator.SetBool(m_animBoolName,false);
    }
    public virtual void LogicUpdate(){}
    public virtual void PhysicsUpdate(){
        DoChecks();
    }
    public virtual void DoChecks(){}

}
