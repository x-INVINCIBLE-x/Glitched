using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState;

    public void Initialize(PlayerState _state)
    {
        ChangeState(_state);
    }

    public void ChangeState(PlayerState _state)
    {
        currentState?.Exit();
        currentState = _state;
        currentState?.Enter();
    }

}
