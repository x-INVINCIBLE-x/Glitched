using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (player.isWallDetected && xInput == player.facingDir)
            return;

        if (yInput < 0)
        {
            stateMachine.ChangeState(player.crouchState);
        }

        if (xInput != 0)
            player.stateMachine.ChangeState(player.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
