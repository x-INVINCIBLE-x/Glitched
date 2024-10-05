using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shadow : Enemy
{
    public ShadowIdleState idleState {  get; private set; }
    public ShadowMoveState moveState { get; private set; }
    public ShadowDashState dashState { get; private set; }
    public ShadowExplodeState explodeState { get; private set; }
    public ShadowDeadState deadState { get; private set; }

    [Header("Explosion Info")]
    public bool canExplode = false;
    public bool hasExploded = false;
    public float explosionTime = 1f;
    [SerializeField] private float explosiveDamage = 0;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;
    [SerializeField] private GameObject explosionPrefab;
    private GameObject ongoingExplosion;

    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadowIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadowMoveState(this, stateMachine, "Move", this);
        dashState = new ShadowDashState(this, stateMachine, "Move", this);
        explodeState = new ShadowExplodeState(this, stateMachine, "Idle", this);
        deadState = new ShadowDeadState(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        Stats.DeadEvent += Die;
    }

    protected override IEnumerator GlitchTeleportation(float duration)
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 5f)
            isInBattle = true;
        else
            isInBattle = false;

        return base.GlitchTeleportation(duration);
    }

    protected override IEnumerator GlitchPhase(float duration)
    {
        float distance = Mathf.Abs(transform.position.x - player.transform.position.x);

        if (!IsPlayerDetected())
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
        stateMachine.ChangeState(idleState);

        dashDuration = defaultDashDuration;
    }

    private float ForceDash(float distance)
    {
        float timeToReach = distance / dashSpeed;
        timeToReach = Random.Range(0, 1f) > 0.5f ? timeToReach + 0.2f : timeToReach + 0.4f;
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

        yield return new WaitForSeconds(duration);
        trailRenderer.enabled = false;

        foreach (GameObject collider in glitchColliders)
            Destroy(collider);
    }

    public void Explode()
    {
        if (ongoingExplosion != null)
            CancelExplode();

        ongoingExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        ongoingExplosion.GetComponent<Explosive_Controller>().SetupExplosive(Stats, growSpeed, maxSize, attackCheckRadius, explosiveDamage);
    }

    public void CancelExplode()
    {
        if(ongoingExplosion != null)
            Destroy(ongoingExplosion);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
