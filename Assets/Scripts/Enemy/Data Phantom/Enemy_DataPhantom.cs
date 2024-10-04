using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DataPhantom : Enemy
{
    public DataPhantomIdleState idleState {  get; private set; }
    public DataPhantomMoveState moveState { get; private set; }
    public DataPhantomAttackState attackState { get; private set; }
    public DataPhantomHitState hitState { get; private set; }
    public DataPhantomDeadState deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();


    }
}
