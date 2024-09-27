using System.Collections;
using System.Collections.Generic;
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
    private List<GlitchData> glitches;  
    private Dictionary<Glitch, Coroutine> activeGlitchCoroutines;  

    private float nextGlitchTime;

    [Header("Generic Glitch Info")]
    [SerializeField] private float minNextGlitchTime = 1f;
    [SerializeField] private float maxNextGlitchTime = 4f;

    [Header("Movement Glitch Info")]
    [SerializeField] private float movGlitchDuration;
    [SerializeField] private float movGlitchCooldown;

    [Header("Attack Damage Glitch Info")]
    [SerializeField] private float attackDamageGlitchDuration;
    [SerializeField] private float attackDamageGlitchCooldown;

    public class ActiveGlitch
    {
        public GlitchData Glitch { get; private set; }
        public float Duration { get; set; }

        public ActiveGlitch(GlitchData glitch)
        {
            Glitch = glitch;
            Duration = glitch.Duration;  // Set initial duration
        }
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        glitches = new List<GlitchData>
        {
            new GlitchData(Glitch.MovementSpeed, movGlitchDuration, movGlitchCooldown),
            new GlitchData(Glitch.AttackDamage, attackDamageGlitchDuration, attackDamageGlitchCooldown)
            // Add more glitches here
        };
        activeGlitchCoroutines = new Dictionary<Glitch, Coroutine>();  

        ScheduleNextGlitch();
    }

    void Update()
    {
        if (Time.time >= nextGlitchTime)
        {
            TriggerRandomGlitch();
        }
    }

    private void ScheduleNextGlitch()
    {
        float randomCooldown = Random.Range(minNextGlitchTime, maxNextGlitchTime);
        nextGlitchTime = Time.time + randomCooldown;
    }

    private void TriggerRandomGlitch()
    {
        GlitchData glitch = glitches[Random.Range(0, glitches.Count)];
        if (!activeGlitchCoroutines.ContainsKey(glitch.type))
        {
            Coroutine glitchCoroutine = StartCoroutine(ApplyGlitch(glitch));
            activeGlitchCoroutines[glitch.type] = glitchCoroutine;
        }

        ScheduleNextGlitch();
    }

    private IEnumerator ApplyGlitch(GlitchData glitch)
    {
        enemy.ApplyGlitch(glitch);  

        yield return new WaitForSeconds(glitch.Duration);  

        enemy.RemoveGlitch(glitch);  

        activeGlitchCoroutines.Remove(glitch.type);
    }
}
