using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private bool isDamageGlitched = false;

    public override void DoDamage(CharacterStats targetStats)
    {
        if (isDamageGlitched)
        {
            Debug.Log("Damage is currently glitched");
            return;
        }

        base.DoDamage(targetStats);
    }

    public void SetGlitchedDamage(bool value) => isDamageGlitched = value;
}
