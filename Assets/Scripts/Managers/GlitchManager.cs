using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerGlitches
{
    Gravity = 1,
    Jump,
    Speed,
    Attack,
    Direction
}

public class GlitchManager : MonoBehaviour
{
    public static GlitchManager Instance;

    [field: Header("Glitcehed Info")]
    [field: SerializeField] public bool GlitchedGravity { get; private set; }
    [field: SerializeField] public bool GlitchedJump { get; private set; }
    [field: SerializeField] public bool GlitchedSpeed { get; private set; }
    [field: SerializeField] public bool GlitchedAttack { get; private set; }
    [field: SerializeField] public bool GlitchedDirection { get; private set; }

    public event Action onGlitchUpdate;

    public List<PlayerGlitches> activeGlitches;
    public int maxGlitches = 1;
    public int currGlitches = 0;
    [SerializeField] private float glitchChangeCooldown = 2f;
    private float lastTimeGlitchChanged = -10f;

    [Header("Speed Glitch Details")]
    [field: SerializeField][Range(0f, 1f)] public float minSpeedVariation;
    [field: SerializeField][Range(0f, 1f)] public float maxSpeedVariation;
    [SerializeField] private float speedGlitchCoolDown = 0.5f;
    [SerializeField] private float lastTimeSpeedGlitched = - 10f;

    [Header("Jump Glitch Details")]
    [field: SerializeField][Range(0f, 1f)] public float minJumpVariation;
    [field: SerializeField][Range(0f, 1f)] public float maxJumpVariation;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UpdateGlitches(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            UpdateGlitches(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            UpdateGlitches(3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            UpdateGlitches(4);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            UpdateGlitches(5);
    }

    private void UpdateGlitches(int toGlitch)
    {
        if (Time.time < lastTimeGlitchChanged + glitchChangeCooldown)
            return;

        if (CheckActiveGlitches((PlayerGlitches)toGlitch))
            return;

        if (maxGlitches == currGlitches)
            RemoveFirstGlitch();

        ActivateGlitch(toGlitch);

        lastTimeGlitchChanged = Time.time;

        onGlitchUpdate?.Invoke();
        activeGlitches.Add((PlayerGlitches)toGlitch);
        currGlitches++;
    }

    private void ActivateGlitch(int toGlitch)
    {
        switch (toGlitch)
        {
            case 1:
                GlitchedGravity = true;
                break;
            case 2:
                GlitchedJump = true;
                break;
            case 3:
                GlitchedSpeed = true;
                break;
            case 4:
                GlitchedAttack = true;
                break;
            case 5:
                GlitchedDirection = true;
                break;
        }

        onGlitchUpdate?.Invoke();
    }

    public void AddGlitch(PlayerGlitches glitch) => ActivateGlitch((int)glitch);
    public void RemoveGlitch(PlayerGlitches glitch) => GlitchToRemove(glitch);

    public void AddGlitchfor(PlayerGlitches glitch, float duration)
    {
        StartCoroutine(AddGlitchTimer(glitch, duration));
    }

    private IEnumerator AddGlitchTimer(PlayerGlitches glitch, float duration)
    {
        AddGlitch(glitch);
        yield return new WaitForSeconds(duration);
        RemoveGlitch(glitch);
    }

    private void RemoveFirstGlitch()
    {
        PlayerGlitches glitch = activeGlitches[0];
        activeGlitches.RemoveAt(0);
        currGlitches--;
        GlitchToRemove(glitch);
    }

    private void GlitchToRemove(PlayerGlitches glitch)
    {
        if (((int)glitch) == 1)
            GlitchedGravity = false;
        else if (((int)glitch) == 2)
            GlitchedJump = false;
        else if ((int)glitch == 3)
            GlitchedSpeed = false;
        else if (((int)glitch) == 4)
            GlitchedAttack = false;
        else if (((int)glitch) == 5)
            GlitchedDirection = false;

        onGlitchUpdate?.Invoke();
    }

    private bool CheckActiveGlitches(PlayerGlitches glitch)
    {
        foreach (PlayerGlitches activeGlitch in activeGlitches)
        {
            if (activeGlitch == glitch) return true;
        }

        return false;
    }

    public bool CanGlitch(PlayerGlitches glitch)
    {
        if (glitch == PlayerGlitches.Speed)
        {
            if (Time.time > lastTimeSpeedGlitched + speedGlitchCoolDown)
            {
                lastTimeSpeedGlitched = Time.time;
                return true;
            }
        }

        return false;
    }
}
