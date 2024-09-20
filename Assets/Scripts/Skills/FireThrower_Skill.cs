using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrower_Skill : Skill
{
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private Transform shootingPoint;

    [SerializeField] private float fireballSpeed;
    [SerializeField] private float fireballDuration;

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject fireball = Instantiate(fireBallPrefab, shootingPoint.position, Quaternion.identity);
        FireBall_Controller fireballController = fireball.GetComponent<FireBall_Controller>();
        fireballController.Setup(fireballSpeed, player.facingDir, fireballDuration);
    }
}
