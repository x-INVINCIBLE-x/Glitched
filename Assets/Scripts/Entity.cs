using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int facingDir { get; private set; } = 1;
    public bool facingRight = true;

    [Header("Collision Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [Space]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    public Rigidbody2D rb {  get; private set; }
    public Animator animator { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public void FlipController(float _x)
    {
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

    public void SetVelcocity(float xVelocity, float yVelocity)
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
        SetVelcocity(xVelocity, yVelocity);
    }

    public void SetZeroVelocity()
    {
        rb.velocity = Vector3.zero;
    }


    public bool isGroundDetected => Physics2D.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
    public bool isWallDetected => Physics2D.Raycast(wallCheck.position, Vector3.right * facingDir, wallCheckDistance, groundLayer);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position, new Vector3((wallCheck.position.x + (wallCheckDistance * facingDir)), wallCheck.position.y));
    }
}
