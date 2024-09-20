using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireBreathState : PlayerState
{
    public PlayerFireBreathState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isBusy = true;
        stateTimer = SkillManager.instance.fireBreath.GetDuration() + 0.5f;
        SkillManager.instance.fireBreath.UseSkill();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.isBusy = false;
    }
}
