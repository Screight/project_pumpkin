using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundebreakerState : PlayerAbilityState
{
    protected float m_inputX;
    public PlayerGroundebreakerState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName) : base(p_player, p_stateMachine, p_playerData, p_animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        m_player.SetGravity(0);
        m_player.SetVelocityY(m_playerData.m_groundbreakerSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_player.CheckIfGrounded()){
            m_isAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
