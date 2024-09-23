using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;
        player.SetVelcocity(rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y <= 0)
        {
            player.stateMachine.ChangeState(player.airState);
        }

        if (player.isWallDetected && xInput == player.facingDir)
            return;

        player.SetVelcocity(player.moveSpeed * xInput * 0.8f, rb.velocity.y);

    }

    public override void Exit()
    {
        base.Exit();
    }

}
