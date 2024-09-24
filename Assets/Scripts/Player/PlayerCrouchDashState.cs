using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchDashState : PlayerCrouchBaseState
{
    private float slideSpeed;
    public PlayerCrouchDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        slideSpeed = player.crouchDashSpeed;
        stateTimer = player.crouchDashDuration;
        player.crouchDashCooldown = 0.4f;
    }

    public override void Update()
    {
        slideSpeed -= Time.deltaTime * 3f;
        player.SetVelocity(slideSpeed * player.facingDir, rb.velocity.y);

        if((rb.velocity.x > -1.1f && rb.velocity.x < 1.1f))
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
