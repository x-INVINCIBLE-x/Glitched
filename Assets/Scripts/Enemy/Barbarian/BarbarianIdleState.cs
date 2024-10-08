using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianIdleState : BarbarianGroundedState
{
    public BarbarianIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Barbarian enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        //AudioManager.instance.PlaySFX(14,enemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);

    }
}
