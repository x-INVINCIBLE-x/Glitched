using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Glitch
{
    MovementSpeed,
    AttackDamage,
    AttackSpeed,
    Teleportation,
    Phase,
    Dublicate
}

public class GlitchData
{
    public Glitch type { get; private set; }
    public float Duration { get; private set; }

    public GlitchData(Glitch type, float duration)
    {
        this.type = type;
        Duration = duration;
    }
}
