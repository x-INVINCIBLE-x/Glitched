using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath_Skill : Skill
{
    [SerializeField] private GameObject fireBreath;
    private FireBreath_Controller controller;

    [SerializeField] private float fireBreathDuration;

    public override void UseSkill()
    {
        base.UseSkill();

        if (controller == null)
            controller = fireBreath.GetComponent<FireBreath_Controller>();
        Invoke(nameof(StartSkill), 0.1f);
    }

    private void StartSkill()
    {
        fireBreath.SetActive(true);
        controller.Setup(fireBreathDuration);
    }

    public float GetDuration() => fireBreathDuration;
}
