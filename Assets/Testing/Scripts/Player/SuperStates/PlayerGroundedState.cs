using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected float m_inputX;
    protected bool m_hasTransitioned = false;
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
        m_hasTransitioned = false;
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
            m_hasTransitioned = true;
            return;
        }
        else if(InputManager.Instance.DashButtonPressed){
            m_stateMachine.ChangeState(m_player.DashState);
            m_hasTransitioned = true;
            return ;
        }
        else if(InputManager.Instance.Skill1ButtonPressed){
            m_stateMachine.ChangeState(m_player.FireballState);
            m_hasTransitioned = true;
            return;
        }
        else if(InputManager.Instance.HorizontalAxis == 0){
            m_inputX = 0;
        }
        else{
            m_inputX = InputManager.Instance.HorizontalAxis;
            m_player.CheckIfShouldFlip(m_inputX);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
