using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    bool m_isGrounded;

    protected float m_inputX;
    public PlayerAirState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName) : base(p_player, p_stateMachine, p_playerData, p_animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        m_player.SetGravity(m_playerData.m_gravityDown);
        m_isGrounded = m_player.CheckIfGrounded();
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
            return;
        }
        else if(InputManager.Instance.Skill1ButtonPressed){
            m_stateMachine.ChangeState(m_player.FireballState);
            return;
        }
        else if(InputManager.Instance.Skill2ButtonPressed){
            m_stateMachine.ChangeState(m_player.GroundbreakerState);
            return;
        }

        float velocityNormalized = m_player.CurrentVelocity.y;
        if(velocityNormalized != 0){
            velocityNormalized =  velocityNormalized / Mathf.Abs(velocityNormalized);
        }
        
        m_player.Animator.SetFloat("yVelocity", velocityNormalized);

        // CHECK IF FALLING
        if(m_player.CurrentVelocity.y < 0.01f){
            m_isGrounded = m_player.CheckIfGrounded();
            m_player.SetGravity(m_playerData.m_gravityDown);
        }
        
        // GET MOVEMENT INPUT
        m_inputX = InputManager.Instance.HorizontalAxis;
        m_player.CheckIfShouldFlip(m_inputX);

        // CHECK IF GROUNDED
        if(m_isGrounded && m_player.CurrentVelocity.y < 0.01f){
            if(m_inputX != 0){
                m_stateMachine.ChangeState(m_player.MoveState);
            }else{
                m_player.IdleState.TriggerLandAnimation();
                m_stateMachine.ChangeState(m_player.IdleState);
            }
        }
        // MOVEMENT
        m_player.SetVelocityX(m_playerData.movementVelocity * m_inputX);
        m_inputX = 0;
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
