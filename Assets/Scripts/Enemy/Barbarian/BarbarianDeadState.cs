using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianDeadState : EnemyState
{
    private Enemy_Barbarian enemy;

    public BarbarianDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Barbarian _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.canBeGlitched = false;
        //enemy.anim.SetBool(enemy.lastAnimBoolName, true); // Fake Death , comic jump
        //enemy.anim.speed = 0;
        enemy.cd.enabled = false;
        enemy.rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        enemy.Destroy(5f);
        //if (stateTimer > 0)
        //    rb.velocity = new Vector2(0, 10);
    }
}
