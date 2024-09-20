using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireSwordState : PlayerState
{
    public PlayerFireSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isBusy = true;

        SkillManager.instance.fireSword.UseSkill();

    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        player.isBusy = false;
        base.Exit();
    }
}
