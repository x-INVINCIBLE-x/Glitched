using System.Collections;
using UnityEngine;


public class SentinelDeadState : EnemyState
{
    private Enemy_Sentinel enemy;

    public SentinelDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Sentinel _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
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
