using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Laser_Controller : MonoBehaviour
{
    private Rigidbody2D rb;

    private float speed;
    private float duration;

    private Vector3 direction;
    private float distance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(float speed, float duration, Vector3 direction)
    {
        this.speed = speed;
        this.duration = duration;
        this.direction = direction;

        distance = speed * duration;
        transform.right = direction;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        rb.velocity = (direction.normalized * speed);

        if(duration < 0)
        {
            Destroy(gameObject);
        }
    }
}
