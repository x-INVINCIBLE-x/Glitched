using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDashState : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowDashState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.dashDuration;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.dashSpeed * enemy.facingDir, 0);

        if (enemy.canExplode && Vector2.Distance(enemy.transform.position, enemy.player.transform.position) < 1f)
        {
            stateMachine.ChangeState(enemy.explodeState);
            return;
        }

        if (stateTimer <= 0 || !enemy.IsGroundDetected || enemy.IsWallDetected)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetVelocity(0, rb.velocity.y);
    }
}
