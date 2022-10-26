using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSkeletonChase : State
{
    public StateSkeletonChase(StateMachine p_stateMachine, SkeletonController p_controller, string p_name, ANIMATION p_animation, SKELETON_STATE p_state) : base(p_stateMachine, p_controller, p_name, p_animation, p_state)
    {
    }

    public override void Enter(bool p_changeToDefaultAnim = true)
    {
        base.Enter(p_changeToDefaultAnim);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Vector2 pawnPosition = m_controller.transform.position;
        Vector2 targetPosition = Player.Instance.transform.position;

        if (m_controller.IsPlayerInAttackRange)
        {
            m_controller.ChangeState(m_controller.AttackState);
            return;
        }
        else if (!m_controller.IsPlayerInVisionRange)
        {
            m_controller.ChangeState(m_controller.PatrolState);
            return;
        }

        float direction = (targetPosition.x - pawnPosition.x) / Mathf.Abs(targetPosition.x - pawnPosition.x);
        m_controller.Rigidbody2D.velocity = new Vector2(direction * m_controller.Speed, m_controller.Rigidbody2D.velocity.y);

        if (direction != m_controller.FacingDirection())
        {
            m_controller.FlipX();
            m_controller.VelocityX *= -1;
        }

    }

}
