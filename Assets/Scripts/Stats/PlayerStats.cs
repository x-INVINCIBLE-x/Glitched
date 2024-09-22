using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private GlitchManager glitchManager;

    private void Start()
    {
        glitchManager = GlitchManager.Instance;
    }

    public override void DoDamage(CharacterStats targetStats)
    {
        if (glitchManager.GlitchedAttack)
        {
            DoGlitchedHeal(targetStats);
        }
        else
            base.DoDamage(targetStats);
    }
}
