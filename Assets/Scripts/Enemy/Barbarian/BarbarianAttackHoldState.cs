using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianAttackHoldState : EnemyState
{
    private Enemy_Barbarian enemy;
    public BarbarianAttackHoldState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Barbarian enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerDetected().distance > enemy.attackDistance)
            {
                stateMachine.ChangeState(enemy.battleState);
            }
            else
            {
                if (enemy.CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
            
        }
        else
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
