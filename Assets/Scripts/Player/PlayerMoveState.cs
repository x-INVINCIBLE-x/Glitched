using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.crouchDashCooldown;
    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0 || (player.IsWallDetected && xInput == player.facingDir))
            player.stateMachine.ChangeState(player.idleState);

        if(yInput == -1 && stateTimer <= 0)
            stateMachine.ChangeState(player.crouchDashState);

        player.SetVelocity(player.moveSpeed * xInput, rb.velocity.y);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
