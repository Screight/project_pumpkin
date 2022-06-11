using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    PlayerState m_currentState;

    public void Initialize(PlayerState p_startingState){
        m_currentState = p_startingState;
        m_currentState.Enter();
    }

    public void ChangeState(PlayerState p_newState){
        m_currentState.Exit();
        m_currentState = p_newState;
        m_currentState.Enter();
    }

    public PlayerState CurrentState{
        get { return m_currentState; }
        set { m_currentState = value; }
    }

}
