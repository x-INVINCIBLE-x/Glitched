using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneEnemy_Barbarian : CloneEnemy
{
    private CloneBattleState battleState;

    protected override void Start()
    {
        base.Start();
        battleState = new(this, stateMachine, "Move");
        stateMachine.Initialize(battleState);
    }
}
