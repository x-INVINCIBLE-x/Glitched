using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMoveState : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.canExplode && Vector2.Distance(enemy.transform.position, enemy.player.transform.position) < 1f)
        {
            stateMachine.ChangeState(enemy.explodeState);
            return;
        }

        if (enemy.IsWallDetected || !enemy.IsGroundDetected)
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }

    }

}
