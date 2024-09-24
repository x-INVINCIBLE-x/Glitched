using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerState
{
    public PlayerHangingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    public override void Update()
    {
        base.Update();

        if(xInput != 0 && xInput != player.facingDir)
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }


        if (!player.isLedgeDetected)
            stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.S) || yInput == -1)
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        player.SetVelocity(xInput,-1f);
    }
}
