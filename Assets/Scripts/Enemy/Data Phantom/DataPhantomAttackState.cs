using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPhantomAttackState : EnemyState
{
    private Enemy_DataPhantom enemy;
    public DataPhantomAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DataPhantom enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetInBattle(true);
        enemy.anim.speed = enemy.attackAnimationSpeed;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetInBattle(false);
        enemy.anim.speed = 1f;
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
