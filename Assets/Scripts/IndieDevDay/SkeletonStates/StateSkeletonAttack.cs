using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSkeletonAttack : State
{

    Timer m_attackTimer;

    public StateSkeletonAttack(StateMachine p_stateMachine, SkeletonController p_controller, string p_name, ANIMATION p_animation, SKELETON_STATE p_state, float p_timeBetweenAttacks) : base(p_stateMachine, p_controller, p_name, p_animation, p_state)
    {
        m_attackTimer = m_controller.gameObject.AddComponent<Timer>();
        m_attackTimer.Duration = p_timeBetweenAttacks;
        m_attackTimer.Run();
    }

    public override void Enter(bool p_changeToDefaultAnim = true)
    {
        base.Enter(false);
        m_controller.VelocityX = 0;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Vector2 pawnPosition = m_controller.transform.position;
        Vector2 targetPosition = Player.Instance.transform.position;

        if (m_attackTimer.IsFinished)
        {
            AnimationManager.Instance.PlayAnimation(m_controller.Animator, ANIMATION.SKELETON_ATTACK);
            m_controller.AnimationState = ANIMATION.SKELETON_ATTACK;
        }

    }

}
