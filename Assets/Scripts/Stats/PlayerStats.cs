using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private GlitchManager glitchManager;
    [field: SerializeField] public int tempGlitchesAmount;
    [field: SerializeField] public int currTempGlitches;
    [field: SerializeField] public bool isGlitchedByTrail { get; private set; } = false;

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

    public void AddGlitchToPlayer(PlayerGlitches glitch, float duration)
    {
        if (currTempGlitches >= tempGlitchesAmount)
            return;

        currTempGlitches++;
        glitchManager.AddGlitchfor(glitch, duration);
    }


    public void StartGlitchByTrail(float duration)
    {
        isGlitchedByTrail = true;
        StartCoroutine(EndEffectAfter(duration));
    }

    IEnumerator EndEffectAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isGlitchedByTrail = false;
    }
}
