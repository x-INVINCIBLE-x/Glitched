using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private float speed;
    private float duration;
    private int movingDir;

    private bool hasBlasted = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            FinishFireBall();

        if (!hasBlasted)
        rb.velocity = new Vector3(speed * movingDir,0,0);
    }

    public void Setup(float _speed, int _movingDir, float _duration)
    {
        speed = _speed;
        movingDir = _movingDir;
        duration = _duration;

        if (movingDir < 0)
            transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FinishFireBall();
    }

    private void FinishFireBall()
    {
        hasBlasted = true;
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Blast");
        Destroy(gameObject, 1);
    }
}
