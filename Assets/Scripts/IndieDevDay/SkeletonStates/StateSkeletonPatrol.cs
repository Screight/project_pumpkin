using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSkeletonPatrol : State
{
    Vector2 m_patrolPointLeft;
    Vector2 m_patrolPointRight;
    public StateSkeletonPatrol(StateMachine p_stateMachine, SkeletonController p_controller, string p_name, ANIMATION p_animation, SKELETON_STATE p_state, Transform p_patrolPointLeft, Transform p_patrolPointRight) : base(p_stateMachine, p_controller, p_name, p_animation, p_state)
    {
        m_patrolPointLeft = p_patrolPointLeft.position;
        m_patrolPointRight = p_patrolPointRight.position;
    }

    public override void Enter(bool p_changeToDefaultAnim = true)
    {
        base.Enter(p_changeToDefaultAnim);
        Vector2 pawnPosition = m_controller.transform.position;
        float distanceToLeftPatrolPoint = Mathf.Abs((pawnPosition.x - m_patrolPointLeft.x));
        float distanceToRightPatrolPoint = Mathf.Abs((pawnPosition.x - m_patrolPointRight.x));

        float direction;
        if(distanceToLeftPatrolPoint < distanceToRightPatrolPoint)
        {
            direction = (pawnPosition.x - m_patrolPointLeft.x)/Mathf.Abs(pawnPosition.x - m_patrolPointLeft.x);
        }
        else {
            direction = (pawnPosition.x - m_patrolPointRight.x) / Mathf.Abs(pawnPosition.x - m_patrolPointRight.x);
        }
        m_controller.Rigidbody2D.velocity = new Vector2(direction * m_controller.Speed, m_controller.Rigidbody2D.velocity.y);

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float direction = m_controller.VelocityX / Mathf.Abs(m_controller.VelocityX);

        if(direction != m_controller.FacingDirection()) { m_controller.FlipX(); }

        if (m_controller.IsPlayerInAttackRange)
        {
            m_controller.ChangeState(m_controller.AttackState);
            return;
        }
        else if (m_controller.IsPlayerInVisionRange)
        {
            m_stateMachine.ChangeState(m_controller.ChaseState);
            return;
        }

        Vector3 pawnPosition = m_controller.transform.position;

        if (m_controller.IsHittingWall)
        {
            m_controller.FlipX();
            m_controller.IsHittingWall = false;
            m_controller.Rigidbody2D.velocity *= -1;
        }else if (pawnPosition.x < m_patrolPointLeft.x)
        {
            float distanceToPatrolPoint = Mathf.Abs(m_patrolPointLeft.x - pawnPosition.x);

            if(direction == -1)
            {
                pawnPosition = new Vector3(m_patrolPointLeft.x, pawnPosition.y, pawnPosition.z);
                m_controller.Rigidbody2D.velocity *= -1;
            }
            
        }
        else if (pawnPosition.x > m_patrolPointRight.x)
        {
            float distanceToPatrolPoint = Mathf.Abs(m_patrolPointRight.x - pawnPosition.x);

            if (direction == 1)
            {
                pawnPosition = new Vector3(m_patrolPointRight.x, pawnPosition.y, pawnPosition.z);
                m_controller.Rigidbody2D.velocity *= -1;
            }
        }

    }

}
