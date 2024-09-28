using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianBattleState : EnemyState
{
    private Transform player;
    private Enemy_Barbarian enemy;
    private int moveDir;


    public BarbarianBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Barbarian _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        stateTimer = enemy.battleTime;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);        
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (enemy.CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
                else
                {
                    stateMachine.ChangeState(enemy.holdState);
                    return;
                }
            }
        }
        else 
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.agroDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }


        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.attackDistance)
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
