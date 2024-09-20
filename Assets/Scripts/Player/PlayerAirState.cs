using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.isGroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if(player.isLedgeDetected && !Input.GetKey(KeyCode.DownArrow))
        {
            stateMachine.ChangeState(player.hangingState);
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
