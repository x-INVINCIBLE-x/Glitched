using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlitchable
{
    public void ApplyGlitch(GlitchData glitch);
    public void RemoveGlitch(GlitchData glitch);
}
