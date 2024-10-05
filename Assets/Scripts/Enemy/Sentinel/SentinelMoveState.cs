using System.Collections;
using UnityEngine;


public class SentinelMoveState : SentinelGroundedState
{
    public SentinelMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Sentinel _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
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


        //if (enemy.IsWallDetected || !enemy.IsGroundDetected)
        //{
        //    enemy.Flip();
        //    stateMachine.ChangeState(enemy.idleState);
        //}

        //if (enemy.transform.position.x < )

        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.attackDistance)
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

    }
}
