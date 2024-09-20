using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSword_Skill : Skill
{
    [SerializeField] private float fireSwordDuration;
    private float fireSwordTimer;
    public bool isActive;

    public override void UseSkill()
    {
        base.UseSkill();

        isActive = true;
        fireSwordTimer = Time.time;
        cooldown = Mathf.Max(cooldown, fireSwordDuration);
    }

    protected override void Update()
    {
        base.Update();

        if (isActive)
        {
            if (Time.time > fireSwordTimer + fireSwordDuration)
            {
                player.SetDefaultController();
                isActive = false;
            }
        }

    }
}
