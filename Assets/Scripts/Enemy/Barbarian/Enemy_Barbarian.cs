using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy_Barbarian : Enemy
{
    #region States

    public BarbarianIdleState idleState { get; private set; }
    public BarbarianMoveState moveState { get; private set; }
    public BarbarianDashState dashState { get; private set; }
    public BarbarianBattleState battleState { get; private set; }
    public BarbarianAttackState attackState { get; private set; }
    public BarbarianStunnedState stunnedState { get; private set; }
    public BarbarianDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        idleState = new BarbarianIdleState(this, stateMachine, "Idle", this);
        moveState = new BarbarianMoveState(this, stateMachine, "Move", this);
        dashState = new BarbarianDashState(this, stateMachine, "Dash", this);
        battleState = new BarbarianBattleState(this, stateMachine, "Move", this);
        attackState = new BarbarianAttackState(this, stateMachine, "Attack", this);
        stunnedState = new BarbarianStunnedState(this, stateMachine, "Stunned", this);
        deadState = new BarbarianDeadState(this, stateMachine, "Death", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        trailRenderer.enabled = false;
    }


    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool CanAttack()
    {
        if (Time.time >= lastTimeAttacked + attackCooldown)
        {
            attackCooldown = Random.Range(minAttackCooldown, maxAttackCooldown);
            lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    protected override IEnumerator GlitchPhase(float duration)
    {
        float distance = Mathf.Abs(transform.position.x - player.transform.position.x);

        if (distance < 5f || !IsPlayerDetected())
            yield break;

        float defaultDashDuration = dashDuration;
        duration = ForceDash(distance);

        yield return new WaitForSeconds(duration);

        if (Random.Range(0, 1) < 0.6f)
        {
            distance = Mathf.Abs(transform.position.x - player.transform.position.x);

            if (distance < 1f || !IsPlayerDetected())
            {
                ResetEnemy(defaultDashDuration);
                yield break;
            }

            stateMachine.ChangeState(idleState);
            yield return new WaitForSeconds(0.5f);
            duration = ForceDash(distance);

            yield return new WaitForSeconds(duration);
        }

        ResetEnemy(defaultDashDuration);
    }

    private void ResetEnemy(float defaultDashDuration)
    {
        stateMachine.ChangeState(attackState);

        dashDuration = defaultDashDuration;
    }

    private float ForceDash(float distance)
    {
        float timeToReach = distance / dashSpeed;
        timeToReach = Random.Range(0, 1f) > 0.5f ? timeToReach : Random.Range(0, 1f) > 0.5f ? timeToReach + 0.4f : timeToReach - 0.4f;
        dashDuration = timeToReach;

        StartCoroutine(GlitchTrail(timeToReach));

        stateMachine.ChangeState(dashState);
        return timeToReach;
    }

    IEnumerator GlitchTrail(float duration)
    {
        float currDuration = 0;
        List<GameObject> glitchColliders = new();

        trailRenderer.enabled = true;
        while (currDuration < duration)
        {
            glitchColliders.Add(Instantiate(glitchCollider, transform.position, Quaternion.identity));
            yield return new WaitForSeconds(0.15f); // has to be adjusted according to dashSpeed
            currDuration += 0.1f;
        }
        yield return new WaitForSeconds(2f);
        trailRenderer.enabled = false;

        foreach(GameObject collider in glitchColliders)
            Destroy(collider);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
