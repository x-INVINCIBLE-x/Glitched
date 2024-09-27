using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Glitch
{
    MovementSpeed,
    AttackDamage,
    AttackSpeed
}

public class GlitchData
{
    public Glitch type { get; private set; }
    public float Duration { get; private set; }
    public float Cooldown { get; private set; }

    public GlitchData(Glitch type, float duration, float cooldown)
    {
        this.type = type;
        Duration = duration;
        Cooldown = cooldown;
    }
}
