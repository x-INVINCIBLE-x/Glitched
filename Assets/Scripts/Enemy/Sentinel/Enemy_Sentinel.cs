using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sentinel : Enemy
{

    [Header("Archer spisifc info")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float arrowDamage;

    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float safeDistance; // how close palyer should be to trigger jump on battle state
    [HideInInspector] public float lastTimeJumped;

    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;


    #region States

    public SentinelIdleState idleState { get; private set; }
    public SentinelMoveState moveState { get; private set; }
    public SentinelBattleState battleState { get; private set; }
    public SentinelAttackState attackState { get; private set; }
    public SentinelDeadState deadState { get; private set; }
    public SentinelStunnedState stunnedState { get; private set; }

    public SentinelJumpState jumpState { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        idleState = new SentinelIdleState(this, stateMachine, "Idle", this);
        moveState = new SentinelMoveState(this, stateMachine, "Move", this);
        battleState = new SentinelBattleState(this, stateMachine, "Charge", this);
        attackState = new SentinelAttackState(this, stateMachine, "Attack", this);
        deadState = new SentinelDeadState(this, stateMachine, "Dead", this);
        stunnedState = new SentinelStunnedState(this, stateMachine, "Hit", this);
        jumpState = new SentinelJumpState(this, stateMachine, "Move", this);

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        Stats.DeadEvent += Die;
    }

    protected override void Update()
    {
        base.Update();
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

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newBullet = Instantiate(bulletPrefab, attackCheck.position, Quaternion.identity);
        newBullet.GetComponent<Bullet_Controller>().SetupBullet(bulletSpeed * facingDir, Stats);
    }


    protected override IEnumerator GlitchTeleportation(float duration)
    {
        return base.GlitchTeleportation(duration);
    }

    public bool GroundBehind() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero,0, groundLayer);
    public bool WallBehind() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDistance + 2, groundLayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(groundBehindCheck.position,groundBehindCheckSize);
    }
}
