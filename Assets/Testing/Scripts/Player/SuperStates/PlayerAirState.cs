using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    bool m_isGrounded;

    protected Vector2 m_input;
    public PlayerAirState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName) : base(p_player, p_stateMachine, p_playerData, p_animBoolName)
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
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(InputManager.Instance.HorizontalAxis == 0){
            m_input = Vector2.zero;
        }
        else{
            m_input.x = InputManager.Instance.HorizontalAxis;
            m_player.CheckIfShouldFlip(m_input.x);
            m_input.Normalize();
        }

        float velocityNormalized = m_player.CurrentVelocity.y;
        if(velocityNormalized != 0){
            velocityNormalized =  velocityNormalized / Mathf.Abs(velocityNormalized);
        }
            m_player.Animator.SetFloat("yVelocity", velocityNormalized);

        if(m_player.CurrentVelocity.y < 0.01f){
            m_isGrounded = m_player.CheckIfGrounded();
            m_player.SetGravity(m_playerData.m_gravityDown);
        }

        if(m_isGrounded && m_player.CurrentVelocity.y < 0.01f){
            if(m_input.x != 0){
                m_stateMachine.ChangeState(m_player.MoveState);
            }else{
                m_player.IdleState.TriggerLandAnimation();
                m_stateMachine.ChangeState(m_player.IdleState);
            }
        }

        m_player.SetVelocityX(m_playerData.movementVelocity * m_input.x);
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
