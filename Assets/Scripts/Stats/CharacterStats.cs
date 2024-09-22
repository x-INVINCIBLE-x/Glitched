using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public enum Stats
{
    Health,
    Mana,
    ManaRegain,
    PhysicalAtk,
    FireAtk,
    ElectricAtk,
    PhysicalDef
}
public enum AilmentType
{
    Fire,
    Electric,
    Acid,
    Disruption,
    Shock,
    Break
}

public class CharacterStats : MonoBehaviour
{
    [Header("Common Abilities")]
    public Stat health;
    public Stat mana;
    public Stat manaRegain;


    [Header("Attack Abilities")]
    public Stat physicalAtk;
    public Stat fireAtk;
    public Stat electricAtk;

    [Header("Defence")]
    public Stat physicalDef;

    [Header("Ailment Status")]
    public AilmentStatus fireStatus;
    public AilmentStatus electricStatus;

    public float ailmentLimitOffset = 10;

    public float currentHealth;
    public float currentMana;

    public bool isInvincible { get; private set; } = false;
    public bool isBlocking { get; private set; } = false;
    public bool isPerfectBlock { get; private set; } = false;
    public bool isConsumingStamina { get; private set; } = false;

    protected Dictionary<AilmentType, Action> ailmentActions;
    public Dictionary<Stats, Stat> statDictionary;

    public event Action UpdateHUD;

    [System.Serializable]
    public class AilmentStatus
    {
        public float Value;
        public Stat resistance;
        public Stat defence;
        public bool isMaxed = false;
        public float ailmentLimit = 100;
        public event Action<AilmentStatus> ailmentEffectEnded;
        public IEnumerator ReduceValueOverTime()
        {
            while (Value > 0)
            {
                if (Value > 0)
                {
                    Value -= resistance.Value * Time.deltaTime;
                    
                    if (Value <= 0)
                    {
                        if (isMaxed)
                            ailmentEffectEnded?.Invoke(this);

                        Value = 0;
                        isMaxed = false;
                    }

                    yield return new WaitForEndOfFrame();
                }
                yield return null;
            }
        }
    }

    protected virtual void Awake()
    {
        InitializeValues();

        ailmentActions = new Dictionary<AilmentType, Action>
        {
            { AilmentType.Fire, ApplyFireAilment },
            { AilmentType.Electric, ApplyElectricAilment },
            { AilmentType.Acid, ApplyAcidAilment },
            { AilmentType.Disruption, ApplyDisruptionAilment },
            { AilmentType.Shock, ApplyShockAilment },
            { AilmentType.Break, ApplyBreakAilment }
        };
    }

    private void Start()
    {
        InitializeStatDictionary();
    }

    private void InitializeValues()
    {
        currentHealth = health.Value;
        currentMana = mana.Value;
    }

    public void InitializeStatDictionary()
    {
        statDictionary = new Dictionary<Stats, Stat>
        {
            { Stats.Health,  health },
            { Stats.Mana,  mana },
            { Stats.ManaRegain, manaRegain },
            { Stats.PhysicalAtk, physicalAtk },
            { Stats.FireAtk, fireAtk },
            { Stats.ElectricAtk, electricAtk },
            { Stats.PhysicalDef, physicalDef }
        };
    }

    private void Update()
    {
        if (!isConsumingStamina && currentMana < mana.Value)
        {
            currentMana += manaRegain.Value * Time.deltaTime;
            UpdateHUD?.Invoke();
        }
    }

    public virtual void DoGlitchedHeal(CharacterStats targetStats)
    {
        targetStats.Heal(physicalAtk.Value);
    }

    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, health.Value);
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        if (targetStats.isPerfectBlock)
        {
            Debug.Log("Perfect Block Successful!");
            return;
        }

        targetStats.TakePhysicalDamage(physicalAtk.Value);

        DoAilmentDamage(targetStats);
        targetStats.UpdateHUD?.Invoke();
    }

    public void DoAilmentDamage(CharacterStats targetStats)
    {
        float _fireAtk = fireAtk.Value;
        float _electricAtk = electricAtk.Value;


        float damage = _fireAtk + _electricAtk;

        if (damage == 0)
            return;

        if (_fireAtk > 0)
            targetStats.TryApplyAilmentEffect(_fireAtk, ref targetStats.fireStatus, AilmentType.Fire);
        else if (_electricAtk > 0)
            targetStats.TryApplyAilmentEffect(_electricAtk, ref targetStats.electricStatus, AilmentType.Electric);
    }

    protected virtual void TryApplyAilmentEffect(float ailmentAtk, ref AilmentStatus ailmentStatus, AilmentType ailmentType)
    {
        if (ailmentStatus.isMaxed)
            return;

        float ailmentDefence = ailmentStatus.defence.Value;
        float effectAmount = ailmentAtk - ailmentDefence;
        ReduceHealthBy(effectAmount);

        ailmentStatus.Value = Mathf.Min(ailmentStatus.ailmentLimit + ailmentLimitOffset, ailmentStatus.Value + effectAmount);
        StartCoroutine(ailmentStatus.ReduceValueOverTime());

        //UI.instance.ailmentSlider[((int)ailmentType)].gameObject.SetActive(true);
        //StartCoroutine(UI.instance.ailmentSlider[((int)ailmentType)].UpdateUI());

        if (ailmentStatus.Value < ailmentStatus.ailmentLimit)
            return;

        ApplyAilment(ailmentType);
        ailmentStatus.isMaxed = true;
        ailmentStatus.ailmentEffectEnded += AilmentEffectEnded;
    }

    private void ApplyAilment(AilmentType ailmentType)
    {
        if (ailmentActions.TryGetValue(ailmentType,out var ailmentEffect))
            ailmentEffect();
    }

    protected virtual void AilmentEffectEnded(AilmentStatus ailmentStatus)
    {
        ailmentStatus.ailmentEffectEnded -= AilmentEffectEnded;
    }

    #region Ailment Specific functions

    private void ApplyFireAilment()
    {

    }

    private void ApplyElectricAilment()
    {

    }

    private void ApplyAcidAilment()
    {

    }

    private void ApplyDisruptionAilment()
    {

    }

    private void ApplyShockAilment()
    {

    }

    private void ApplyBreakAilment()
    {

    }

    #endregion

    public void TakePhysicalDamage(float damage)
    {
        float reducedDamage = Mathf.Max(0, damage - physicalDef.Value);

        ReduceHealthBy(reducedDamage);
    }

    public void ReduceHealthBy(float damage)
    {
        if (isInvincible)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);
    }

    public void SetInvincibleFor(float time) => StartCoroutine(MakeInvincibleFor(time));

    private IEnumerator MakeInvincibleFor(float time)
    {
        isInvincible = true;

        yield return new WaitForSeconds(time);

        isInvincible = false;
    }

    public void SetInvincible(bool invincible) => isInvincible = invincible;

    public void SetBlocking(bool blocking) => isBlocking = blocking;
    
    public void SetPerfectBlock(bool perfectBlock) => isPerfectBlock = perfectBlock;
    
    public void SetConsumingMana(bool status) => isConsumingStamina = status;

    public bool HasEnoughMana(float staminaAmount)
    {
        if(currentMana > staminaAmount)
        {
            currentMana -= staminaAmount;
            UpdateHUD?.Invoke();
            return true;
        }

        return false;
    }

    public float GetCurrentMana() => currentMana;
}