using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public FireThrower_Skill fireThrower {  get; private set; }
    public FireBreath_Skill fireBreath { get; private set; }
    public FireSword_Skill fireSword { get; private set; }
    public Dash_Skill dash { get; private set; }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        fireThrower = GetComponent<FireThrower_Skill>();
        fireBreath = GetComponent<FireBreath_Skill>();
        fireSword = GetComponent<FireSword_Skill>();
        dash = GetComponent<Dash_Skill>();
    }
}