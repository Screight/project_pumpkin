using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName) : base(p_player, p_stateMachine, p_playerData, p_animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(InputManager.Instance.DashButtonPressed){
            m_stateMachine.ChangeState(m_player.DashState);
            return ;
        }
        else if(m_input.magnitude == 0){
            m_stateMachine.ChangeState(m_player.IdleState);
            return;
        }
        m_player.SetVelocityX(m_playerData.movementVelocity * m_input.normalized.x);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
