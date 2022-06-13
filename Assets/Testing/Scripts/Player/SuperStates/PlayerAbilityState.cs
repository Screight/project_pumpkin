using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    private bool m_isGrounded;
    protected bool m_isAbiltyDone;
    public PlayerAbilityState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName) : base(p_player, p_stateMachine, p_playerData, p_animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        m_isGrounded = m_player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        m_isAbiltyDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isAbiltyDone){
            if(m_isGrounded && m_player.CurrentVelocity.y < 0){
                m_stateMachine.ChangeState(m_player.IdleState);
            }
            else{
                m_stateMachine.ChangeState(m_player.AirState);
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
