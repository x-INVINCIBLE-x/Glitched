using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    private float moveSpeed;
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if(!player.IsGroundDetected)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(0))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (Input.GetKey(KeyCode.RightControl) || Input.GetMouseButton(1))
        {
            stateMachine.ChangeState(player.blockState);
        }

        //moveSpeed = 
        //player.animator.SetFloat("xVelocity", moveSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
