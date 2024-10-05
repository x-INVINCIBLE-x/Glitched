using System.Collections;
using UnityEngine;

public class SentinelIdleState : SentinelGroundedState
{
    public SentinelIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Sentinel _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;

    }

    public override void Exit()
    {
        base.Exit();

        //AudioManager.instance.PlaySFX(14, enemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.attackDistance - 1f || enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.battleState);
    }
}
