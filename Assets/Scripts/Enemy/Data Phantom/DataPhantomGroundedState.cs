using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPhantomGroundedState : EnemyState
{
    protected Enemy_DataPhantom enemy;
    protected Transform player;

    public DataPhantomGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DataPhantom enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = enemy.player.transform;
        //player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //enemy.IsPlayerDetected().distance < enemy.agroDistance ||
        if (Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.agroDistance)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
