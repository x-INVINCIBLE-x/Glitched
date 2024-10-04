using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchEffect : MonoBehaviour
{
    [SerializeField] private float glitchDuration;
    [SerializeField] private Glitch[] possibleGlitches;

    public void Setup(Glitch[] possibleGlitches, float glitchDuration)
    {
        this.possibleGlitches = possibleGlitches;
        this.glitchDuration = glitchDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerStats stats))
        {
            if (possibleGlitches.Length == 0)
            {
                Debug.LogWarning("No Glitches Added In Glitch Trail Collider");
                return;
            }

            int index = Random.Range(0, possibleGlitches.Length);

            stats.AddGlitchToPlayer((PlayerGlitches)index, glitchDuration);
            stats.StartGlitchByTrail(glitchDuration);
        }
    }
}