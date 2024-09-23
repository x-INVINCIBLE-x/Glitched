using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCount;

    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0;
        player.SetZeroVelocity();

        if(Time.time > lastTimeAttacked + comboWindow)
        {
            comboCount = 0;
        }

        float attackDir = player.facingDir;

        if(xInput != 0)
            attackDir = xInput;

        player.anim.SetInteger("ComboCounter", comboCount);
        player.SetVelcocityAfterDelay(player.attackForce[comboCount].x * attackDir, player.attackForce[comboCount].y, 0.075f);

        comboCount++;
        if (comboCount > 2)
            comboCount = 0;
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
        
        lastTimeAttacked = Time.time;
    }

}
