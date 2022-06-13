using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected Vector2 m_input;
    public PlayerGroundedState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName) : base(p_player, p_stateMachine, p_playerData, p_animBoolName)
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

        if(m_player.CurrentVelocity.y < 0f){
            if(!m_player.CheckIfGrounded()){
                m_stateMachine.ChangeState(m_player.AirState);
            }
        }
        else if(InputManager.Instance.JumpButtonPressed){
            m_stateMachine.ChangeState(m_player.JumpState);
        }
        else if(InputManager.Instance.HorizontalAxis == 0){
            m_input = Vector2.zero;
        }
        else{
            m_input.x = InputManager.Instance.HorizontalAxis;
            m_player.CheckIfShouldFlip(m_input.x);
            m_input.Normalize();
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
