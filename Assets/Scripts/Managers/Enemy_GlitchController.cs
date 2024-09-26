using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GlitchController : MonoBehaviour
{
    //TODO:

    //Movement Glitch
    //Attack anim speed inc/ dec
    //no damage

    //Level 2
    //Teleportation
    //Phase
    //Attack cooldown dec {Not like Glitch}
    // movement speed inc {Not like glitch}

    //Level 3 
    //Enemy dublicate
    //increased aggressiveness
    //glitch movement with teleport 

    [field: Header("Glitch Info")]
    public bool movementGlitch { get; private set; } = false;
    [Range(0f, 2f)] public float minSpeedVariation;
    [Range(0f, 2f)] public float maxSpeedVariation;
    [SerializeField] private float speedGlitchCoolDown = 0.5f; // Time for next speed glitch
    [SerializeField] private float lastTimeSpeedGlitched = -10f;
    [SerializeField] private float minSpeedGlitchDuration;
    [SerializeField] private float maxSpeedGlitchDuration;
    private float currGlitchDuration; // How long the current glitch should last

    [Header("Attack Info")]
    public bool attackSpeedGlitch;
    public bool attackDamageGlitch;
    [Range(0f, 1f)] public float minAttackSpeedVariation;
    [Range(0f, 1f)] public float maxAttackSpeedVariation;
    [SerializeField] private float attackGlitchCoolDown = 0.5f;
    [SerializeField] private float lastTimeAttackGlitched = -10f;

    public bool CanMovementGlitch(float glitchProbability)
    {
        if (lastTimeSpeedGlitched + speedGlitchCoolDown > Time.time)
            return true;

        if (currGlitchDuration + lastTimeSpeedGlitched > Time.time)
            return false;

        float chance = Random.Range(0, 1f);
        if (glitchProbability > chance)
            return false;

        lastTimeSpeedGlitched = Time.time;
        return true;
    }
}
