using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public GlitchManager GlitchManager;
    public CharacterStats Stats { get; private set; }

    public int facingDir { get; private set; } = 1;
    public bool facingRight = true;

    [Header("Collision Check")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform groundCheck;
    public float groundCheckDistance;
    [Space]

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [Header("Attack Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    private int knockbackDir;
    private Vector2 knockbackPower;
    private bool isKnocked;
    private Vector3 knockbackOffset;
    private float knockbackDuration;

    public Rigidbody2D rb {  get; private set; }
    public Animator anim { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        Stats = GetComponent<CharacterStats>();
        GlitchManager = GlitchManager.Instance;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        
    }

    public void FlipController(float _x)
    {
        if (_x == 0)
            return;

        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetVelcocityAfterDelay(float xVelocity, float yVelocity, float time)
    {
        StartCoroutine(SetVelocityAfter(xVelocity, yVelocity, time));
    }

    IEnumerator SetVelocityAfter(float xVelocity, float yVelocity, float time)
    {
        yield return new WaitForSeconds(time);
        SetVelocity(xVelocity, yVelocity);
    }

    public virtual void DamageImpact() => StartCoroutine("HitKnockback");

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockbackDir = -1;
        else if (_damageDirection.position.x < transform.position.x)
            knockbackDir = 1;


    }

    public void SetupKnockbackPower(Vector2 _knockbackpower) => knockbackPower = _knockbackpower;
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        float xOffset = Random.Range(knockbackOffset.x, knockbackOffset.y);


        if (knockbackPower.x > 0 || knockbackPower.y > 0) // This line makes player immune to freeze effect when he takes hit
            rb.velocity = new Vector2((knockbackPower.x + xOffset) * knockbackDir, knockbackPower.y);

        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        SetupZeroKnockbackPower();
    }

    private void SetupZeroKnockbackPower()
    {
        
    }

    public void SetZeroVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public bool IsGroundDetected => Physics2D.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
    public bool IsWallDetected => Physics2D.Raycast(wallCheck.position, Vector3.right * facingDir, wallCheckDistance, groundLayer);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position, new Vector3((wallCheck.position.x + (wallCheckDistance * facingDir)), wallCheck.position.y));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
