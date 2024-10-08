using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneBattleState : EnemyState
{
    Enemy_Clone enemy;
    int moveDir = 1;
    public CloneBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase as Enemy_Clone;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.player.transform.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (enemy.player.transform.position.x < enemy.transform.position.x)
            moveDir = -1;

        float distance = Vector2.Distance(enemy.player.transform.position, enemy.transform.position);

        if(!enemy.IsPlayerDetected() && distance < enemy.attackDistance)
        {
            enemy.Flip();
        }

        if ((enemy.IsPlayerCloseOnX() && distance > enemy.attackDistance) || (IsEnemyClose() && enemy.IsPlayerCloseOnX()))
        {
            enemy.SetZeroVelocity();
            enemy.anim.SetBool("Move", false);
            if (enemy.hasAttack)
                enemy.anim.SetBool("Attack", false);
            enemy.anim.SetBool("Idle", true);
            return;
        }

        if (distance > enemy.attackDistance && !enemy.IsPlayerCloseOnX()) 
        {
            enemy.anim.SetBool("Idle", false);
            enemy.anim.SetBool("Move", true);
            if (enemy.hasAttack)
                enemy.anim.SetBool("Attack", false);
            enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.velocity.y);
        }
        else
        {
            enemy.SetZeroVelocity();
            enemy.anim.SetBool("Idle", false);
            enemy.anim.SetBool("Move", false);
            if (enemy.hasAttack)
                enemy.anim.SetBool("Attack", true);
        }
    }

    private bool IsEnemyClose() => Physics2D.Raycast(enemy.enemyCheck.position, Vector3.right, 0.3f, enemy.whatIsEnemy);
    
}
