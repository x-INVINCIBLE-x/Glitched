using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Enemy_GlitchController : MonoBehaviour
{
    #region TODO
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
    #endregion

    private Enemy enemy; 
    [SerializeField] private List<Glitch> activeGlitches;
    private Dictionary<Glitch, Coroutine> activeGlitchCoroutines;  

    [SerializeField] private List<GlitchDetails> glitchDetails;

    [System.Serializable]
    public class GlitchDetails
    {
        public Glitch glitch;
        public float minCooldown;
        public float maxCooldown;

        public float duration;
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        activeGlitchCoroutines = new Dictionary<Glitch, Coroutine>();  

        InitializeGlitches();
    }

    private void InitializeGlitches()
    {
        foreach (GlitchDetails glitchDetail in glitchDetails)
        {
            if (!activeGlitchCoroutines.ContainsKey(glitchDetail.glitch))
            {
                Coroutine glitchCoroutine = StartCoroutine(ManageGlitchLifecycle(glitchDetail));
                activeGlitchCoroutines[glitchDetail.glitch] = glitchCoroutine;
            }
        }
    }

    private IEnumerator ManageGlitchLifecycle(GlitchDetails glitchDetail)
    {
        while (true)
        {
            float randomCooldown = Random.Range(glitchDetail.minCooldown, glitchDetail.maxCooldown);
            yield return new WaitForSeconds(randomCooldown);

            GlitchData glitch = new GlitchData(glitchDetail.glitch, glitchDetail.duration);
            StartCoroutine(ApplyGlitch(glitch));

            yield return new WaitForSeconds(glitchDetail.duration);
        }
    }

    private IEnumerator ApplyGlitch(GlitchData glitch)
    {
        enemy.ApplyGlitch(glitch);

        activeGlitches.Add(glitch.type); // Temporary for Inspector view

        yield return new WaitForSeconds(glitch.Duration);  

        enemy.RemoveGlitch(glitch);

        activeGlitches.Remove(glitch.type); // Temporary for Inspector view

        activeGlitchCoroutines.Remove(glitch.type);
    }
}
