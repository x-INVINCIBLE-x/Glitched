using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerCrouchBaseState
{
    public PlayerCrouchState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if(yInput == 0)
            stateMachine.ChangeState(player.idleState);

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl))
            player.anim.SetTrigger("CrouchAttack");

        player.FlipController(xInput);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
