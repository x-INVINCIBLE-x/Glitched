using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPhantomHitState : DataPhantomGroundedState
{
    public DataPhantomHitState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DataPhantom enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }
}
