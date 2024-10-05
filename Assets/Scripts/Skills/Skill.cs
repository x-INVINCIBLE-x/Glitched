using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Player player;

    [SerializeField] protected float cooldown;
    private float cooldownTimer;

    private void Awake()
    {
        //player = GameObject.Find("Player").GetComponent<Player>();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public bool CanUseSkill()
    {
        if(cooldownTimer <= 0)
        {
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {

    }
}
