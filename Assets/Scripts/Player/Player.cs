using UnityEngine;

public class Player : Entity
{
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerBlockState blockState { get; private set; }
    public PlayerHangingState hangingState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerCrouchState crouchState { get; private set; }
    public PlayerCrouchDashState crouchDashState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerFireThrowerState fireThrowerState { get; private set; }
    public PlayerFireBreathState fireBreathState { get; private set; }
    public PlayerFireSwordState fireSwordState { get; private set; }

    public PlayerStateMachine stateMachine { get; private set; }

    private SpriteRenderer sr;

    [SerializeField] private LayerMask ledgeLayer;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private float ledgeCheckDistance;

    [Header("Animator info")]
    public RuntimeAnimatorController defaultAnimator;
    public AnimatorOverrideController fireSwordAnimator;

    [Header("Move info")]
    public float moveSpeed;
    private float speedMultiplier;

    [Header("Jump info")]
    public float jumpForce;
    private float jumpMultiplier;

    [Header("Attack Info")]
    public Vector2[] attackForce;

    [Header("DashInfo")]
    public float dashSpeed;
    public float dashDuration;
    public float crouchDashSpeed;
    public float crouchDashDuration;
    public float crouchDashCooldown = 0.12f;

    [Header("FireSword")]
    public float fireSwordCooldown;
    [HideInInspector] public float fireSwordTimer;


    [Header("Colliders")]
    public Collider2D defaultCollider;
    public Collider2D crouchCollider;

    private SkillManager skillManager;
    public bool isBusy;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Air");
        airState = new PlayerAirState(this, stateMachine, "Air");
        blockState = new PlayerBlockState(this, stateMachine, "Block");
        hangingState = new PlayerHangingState(this, stateMachine, "Hang");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        crouchDashState = new PlayerCrouchDashState(this, stateMachine, "CrouchDash");

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "PrimaryAttack");
        fireThrowerState = new PlayerFireThrowerState(this, stateMachine, "FireThrow");
        fireBreathState = new PlayerFireBreathState(this, stateMachine, "FireBreath");
        fireSwordState = new PlayerFireSwordState(this, stateMachine, "FireSword");

        stateMachine.Initialize(idleState);
        skillManager = SkillManager.instance;

        GlitchManager = GlitchManager.Instance;
        GlitchManager.onGlitchUpdate += OnGlitchesUpdate;
    }

    private void OnEnable()
    {
        
    }

    protected override void Update()
    {
        stateMachine.currentState.Update();

        if (!isBusy)
            CheckAbilities();
    }

    private void CheckAbilities()
    {
        if (Input.GetKeyDown(KeyCode.F) && skillManager.fireThrower.CanUseSkill())
            stateMachine.ChangeState(fireThrowerState);

        //if (Input.GetKeyDown(KeyCode.B) && skillManager.fireBreath.CanUseSkill())
        //    stateMachine.ChangeState(fireBreathState);

        if (Input.GetKeyDown(KeyCode.U) && skillManager.fireSword.CanUseSkill())
            stateMachine.ChangeState(fireSwordState);

        if (Input.GetKeyDown(KeyCode.LeftShift) && skillManager.dash.CanUseSkill())
        {
            stateMachine.ChangeState(dashState);
        }
    }

    public void SetFireSwordController() => anim.runtimeAnimatorController = fireSwordAnimator;
    public void SetDefaultController()
    {
        PlayerState currentState = stateMachine.currentState;
        anim.runtimeAnimatorController = defaultAnimator;
        stateMachine.ChangeState(currentState);
    }

    public bool isLedgeDetected => Physics2D.Raycast(ledgeCheck.position, Vector3.right * facingDir, ledgeCheckDistance, ledgeLayer);
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();

    private void OnGlitchesUpdate()
    {
        if (GlitchManager.GlitchedGravity)
        {
            rb.gravityScale *= -1;
            groundCheckDistance *= -1;
            transform.rotation = Quaternion.Euler(180, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (!GlitchManager.GlitchedGravity && rb.gravityScale < 0)
        {
            rb.gravityScale = Mathf.Abs(rb.gravityScale);
            groundCheckDistance = Mathf.Abs(groundCheckDistance);
            transform.rotation = Quaternion.Euler(180, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    public override void SetVelcocity(float xVelocity, float yVelocity)
    {
        if (GlitchManager.GlitchedDirection)
        {
            xVelocity = -xVelocity;
        }

        if (GlitchManager.GlitchedSpeed)
        {
            if (GlitchManager.CanGlitch(Glitches.Speed))
                speedMultiplier = Random.Range(GlitchManager.minSpeedVariation, GlitchManager.maxSpeedVariation);
            xVelocity *= speedMultiplier;
        }

        if (GlitchManager.GlitchedJump && rb.velocity.y == 0)
        {
            jumpMultiplier = Random.Range(GlitchManager.minJumpVariation, GlitchManager.maxJumpVariation);
            yVelocity *= jumpMultiplier;
        }

        base.SetVelcocity(xVelocity, yVelocity);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.green;
        Gizmos.DrawLine(ledgeCheck.position, new Vector3((ledgeCheck.position.x + (ledgeCheckDistance * facingDir)), ledgeCheck.position.y));
    }
}
