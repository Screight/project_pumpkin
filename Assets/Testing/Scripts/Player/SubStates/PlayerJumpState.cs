using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    protected float m_inputX;
    public PlayerJumpState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName) : base(p_player, p_stateMachine, p_playerData, p_animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        m_player.SetVelocityY(m_playerData.m_maxJumpSpeed);
        m_player.SetGravity(m_playerData.m_gravityUp);
        m_player.Animator.SetFloat("yVelocity", 1);
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
            return;
        }
        else if(InputManager.Instance.Skill1ButtonPressed){
            m_stateMachine.ChangeState(m_player.FireballState);
        }
        else if(InputManager.Instance.Skill2ButtonPressed){
            m_stateMachine.ChangeState(m_player.GroundbreakerState);
        }
        else if (!InputManager.Instance.JumpButtonHold)
        {
            if(m_player.CurrentVelocity.y > m_playerData.m_minJumpSpeed){
                m_player.SetVelocityY(m_playerData.m_minJumpSpeed);
            }
            m_isAbilityDone = true;
        }else if(m_player.CurrentVelocity.y < 0.01){
            m_isAbilityDone = true;
        }

        // MOVEMENT
        m_inputX = InputManager.Instance.HorizontalAxis;
        m_player.CheckIfShouldFlip(m_inputX);
        m_player.SetVelocityX(m_playerData.movementVelocity * m_inputX);
        m_inputX = 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
