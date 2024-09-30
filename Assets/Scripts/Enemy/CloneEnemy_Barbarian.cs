using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneEnemy_Barbarian : Enemy
{
    int moveDir = 1;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (player.transform.position.x > transform.position.x)
            moveDir = 1;
        else if (player.transform.position.x < transform.position.x)
            moveDir = -1;

        if (Vector2.Distance(player.transform.position, transform.position) > attackDistance && !IsPlayerCloseOnX()) 
        {
            anim.SetBool("Move", true);
            anim.SetBool("Attack", false);
            SetVelocity(moveSpeed * moveDir, rb.velocity.y);
        }
        else
        {
            SetZeroVelocity();
            anim.SetBool("Move", false);
            anim.SetBool("Attack", true);
        }
    }

}
