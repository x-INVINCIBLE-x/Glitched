using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPhantomDashState : EnemyState
{
    Enemy_DataPhantom enemy;
    public DataPhantomDashState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DataPhantom enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
