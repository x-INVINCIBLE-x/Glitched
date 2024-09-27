using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Enemy_GlitchController))]
//[RequireComponent(typeof(EntityFX))]
//[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity, IGlitchable
{
    private Player player;
    private Enemy_GlitchController glitchController;
    [HideInInspector] public Collider2D cd;
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned info")]
    public float stunDuration = 1;
    public Vector2 stunDirection = new Vector2(10,12);
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 2;
    public float battleTime = 7;
    private float defaultMoveSpeed;
    private float speedMultiplier = 1f;

    [Header("Attack info")]
    public float agroDistance = 2;
    public float attackDistance = 2;
    public float attackCooldown;
    public float minAttackCooldown = 1;
    public float maxAttackCooldown= 2;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName {  get; private set; }
    //public EntityFX fx { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        cd = GetComponent<Collider2D>();
        glitchController = GetComponent<Enemy_GlitchController>();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();

        //fx = GetComponent<EntityFX>();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SetVelocity(float xVelocity, float yVelocity)
    {
        xVelocity = xVelocity * speedMultiplier;
        base.SetVelocity(xVelocity, yVelocity);
    }

    public void SetSpeedMultiplier(float value) => speedMultiplier = value;
    public void ResetSpeedMuliplier() => speedMultiplier = 1.0f;

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);
     
        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual void Die()
    {

    }

    public void ApplyGlitch(GlitchData glitch)
    {
        switch (glitch.type)
        {
            case Glitch.MovementSpeed:
                StartCoroutine(GlitchMovement(glitch.Duration));  // Start movement glitch coroutine
                break;
            case Glitch.AttackDamage:
                StartCoroutine(GlitchAttack(glitch.Duration));  // Start attack glitch coroutine
                break;
            default:
                Debug.Log(glitch.type + " not yet implemented");
                break;
                // Add more cases for other glitches
        }
    }

    public void RemoveGlitch(GlitchData glitch)
    {
        // Handle logic to remove glitch effects (e.g., reset movement, attack behavior)
    }

    private IEnumerator GlitchMovement(float duration)
    {
        SetSpeedMultiplier(2);
        yield return new WaitForSeconds(duration);
        ResetSpeedMuliplier();
    }

    private IEnumerator GlitchAttack(float duration)
    {
        // Handle attack glitch logic
        yield return new WaitForSeconds(duration);
        // Reset attack to normal
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AnimationSpecialAttackTrigger()
    {

    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 15, whatIsPlayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));

        Gizmos.DrawLine(wallCheck.position + new Vector3(0, 0.5f, 0), new Vector3(15 * facingDir, wallCheck.position.y + 0.5f, 0));
    }
}
