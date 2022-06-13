using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    private bool m_isGrounded;
    protected bool m_isAbilityDone;
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
        m_isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_isAbilityDone){
            m_isGrounded = m_player.CheckIfGrounded();
            if(m_isGrounded && m_player.CurrentVelocity.y < 0.01f){
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
