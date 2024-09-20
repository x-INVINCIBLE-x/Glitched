using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchBaseState : PlayerState
{
    public PlayerCrouchBaseState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.crouchCollider.enabled = true;
        player.defaultCollider.enabled = false;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        player.defaultCollider.enabled = true;
        player.crouchCollider.enabled = false;
    }
}
