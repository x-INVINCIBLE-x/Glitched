using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireThrowerState : PlayerState
{
    public PlayerFireThrowerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isBusy = true;
        SkillManager.instance.fireThrower.UseSkill();
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.isBusy = false;
    }
}
