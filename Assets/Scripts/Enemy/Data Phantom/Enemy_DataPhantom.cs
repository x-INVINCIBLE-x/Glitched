using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DataPhantom : Enemy
{
    public DataPhantomIdleState idleState {  get; private set; }
    public DataPhantomMoveState moveState { get; private set; }
    public DataPhantomDashState dashState { get; private set; }
    public DataPhantomBattleState battleState { get; private set; }
    public DataPhantomAttackState attackState { get; private set; }
    public DataPhantomHitState hitState { get; private set; }
    public DataPhantomDeadState deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new DataPhantomIdleState(this, stateMachine, "Idle", this);
        moveState = new DataPhantomMoveState(this, stateMachine, "Move", this);
        dashState = new DataPhantomDashState(this, stateMachine, "Move", this);
        battleState = new DataPhantomBattleState(this, stateMachine, "Move", this);
        attackState = new DataPhantomAttackState(this, stateMachine, "Attack", this);
        hitState = new DataPhantomHitState(this, stateMachine, "Hit", this);
        deadState = new DataPhantomDeadState(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        Stats.DeadEvent += Die;
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

        foreach (GameObject collider in glitchColliders)
            Destroy(collider);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
