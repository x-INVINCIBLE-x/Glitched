using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    bool isGravityGlitced;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;

        isGravityGlitced = player.GlitchManager.GlitchedGravity;
        if (isGravityGlitced == false)
            player.SetVelocity(rb.velocity.x, player.jumpForce);
        else
            player.SetVelocity(rb.velocity.x, -player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if ((rb.velocity.y <= 0 && isGravityGlitced == false) || (rb.velocity.y >= 0 && isGravityGlitced == true))
        {
            player.stateMachine.ChangeState(player.airState);
        }

        if (player.IsWallDetected && xInput == player.facingDir)
            return;

        player.SetVelocity(player.moveSpeed * xInput * 0.8f, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
