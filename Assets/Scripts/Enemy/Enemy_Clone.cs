using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Clone : Clone
{
    private CloneBattleState battleState;
    public bool hasAttack = true;

    protected override void Start()
    {
        base.Start();
        battleState = new(this, stateMachine, "Move");
        stateMachine.Initialize(battleState);
    }
}
