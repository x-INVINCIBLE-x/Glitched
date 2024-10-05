using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPhantomBattleState : DataPhantomGroundedState
{
    private int moveDir;

    public DataPhantomBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DataPhantom enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetInBattle(true);
        player = PlayerManager.instance.player.transform;

        stateTimer = enemy.battleTime;

        //enemy.SetZeroVelocity();

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(rb.velocity.x, rb.velocity.y);

        SetAnimation();
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (enemy.CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                    return; //
                }

                //return;
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.agroDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
            //else
            //    enemy.Flip();
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.attackDistance && !enemy.IsPlayerCloseOnX())
        {
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        }
        else
            enemy.SetVelocity(0, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool("Idle", false);
        enemy.SetInBattle(false);
    }

    private void SetAnimation()
    {
        if (enemy.rb.velocity.x == 0)
        {
            enemy.anim.SetBool(animBoolName, false);
            enemy.anim.SetBool("Idle", true);
        }
        else
        {
            enemy.anim.SetBool(animBoolName, true);
            enemy.anim.SetBool("Idle", false);
        }
    }
}
