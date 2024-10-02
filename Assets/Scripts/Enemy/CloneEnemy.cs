using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEnemy : Enemy
{
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + 5 * facingDir, transform.position.y));
    }
}
