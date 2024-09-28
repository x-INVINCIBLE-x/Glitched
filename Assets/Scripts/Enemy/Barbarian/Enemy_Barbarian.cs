using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy_Barbarian : Enemy
{
    #region States

    public BarbarianIdleState idleState { get; private set; }
    public BarbarianMoveState moveState { get; private set; }
    public BarbarianBattleState battleState { get; private set; }
    public BarbarianAttackState attackState { get; private set; }
    public BarbarianStunnedState stunnedState { get; private set; }
    public BarbarianDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        idleState = new BarbarianIdleState(this, stateMachine, "Idle", this);
        moveState = new BarbarianMoveState(this, stateMachine, "Move", this);
        battleState = new BarbarianBattleState(this, stateMachine, "Move", this);
        attackState = new BarbarianAttackState(this, stateMachine, "Attack", this);
        stunnedState = new BarbarianStunnedState(this, stateMachine, "Stunned", this);
        deadState = new BarbarianDeadState(this, stateMachine, "Death", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }


    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool CanAttack()
    {
        if (Time.time >= lastTimeAttacked + attackCooldown)
        {
            attackCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
            lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

    }
}
