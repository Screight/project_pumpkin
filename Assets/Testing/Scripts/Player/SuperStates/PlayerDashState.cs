using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    float m_dashSpeed;
    Timer m_dashTimer;
    public PlayerDashState(PlayerNewController p_player, PlayerStateMachine p_stateMachine, PlayerData p_playerData, string p_animBoolName) : base(p_player, p_stateMachine, p_playerData, p_animBoolName)
    {
        m_dashTimer = m_player.gameObject.AddComponent<Timer>();
        m_dashTimer.Duration = AnimationManager.GetClipDuration(m_player.Animator, "dash");
        m_dashSpeed = m_playerData.m_dashDistance / m_dashTimer.Duration;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        m_dashTimer.Run();
        if(m_player.FacingDirection == 1){
            m_player.SetVelocityX(m_dashSpeed);
        }
        else{
            m_player.SetVelocityX(-m_dashSpeed);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(m_dashTimer.IsFinished){
            Exit();
            if(InputManager.Instance.HorizontalAxis == 0){
                m_stateMachine.ChangeState(m_player.IdleState);
            } else{
                m_stateMachine.ChangeState(m_player.MoveState);
            }
            
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
