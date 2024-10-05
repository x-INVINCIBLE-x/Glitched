using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowExplodeState : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowExplodeState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Explode();
        stateTimer = enemy.explosionTime;
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            enemy.hasExploded = true;
            enemy.Die();
            return;
        }

        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) > 1f)
        {
            enemy.CancelExplode();
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
