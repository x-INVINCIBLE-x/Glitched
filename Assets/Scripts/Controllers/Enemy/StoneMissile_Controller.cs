using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StoneMissile_Controller : MonoBehaviour
{
    private PlayerManager playerManager;
    private Rigidbody2D rb;

    [SerializeField] private Vector3 initialTarget;
    [SerializeField] private Vector3 finalTarget;

    private float initialSpeed;
    private float finalSpeed;

    [SerializeField] private float flightDuration;
    private float lifeDuration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerManager = PlayerManager.instance;
    }

    public void Setup(Vector3 initialTarget, float flightDuration, float initialSpeed, float finalSpeed)
    {
        this.initialTarget = initialTarget;
        this.flightDuration = flightDuration;
        this.initialSpeed = initialSpeed;
        this.finalSpeed = finalSpeed;

        lifeDuration = flightDuration + 2; // flightDuration == -2
    }

    private void Update()
    {
        flightDuration -= Time.deltaTime;
        lifeDuration -= Time.deltaTime;

            finalTarget = playerManager.player.position;

        if (flightDuration > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialTarget, initialSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, finalTarget, finalSpeed * Time.deltaTime);
        }

        if (lifeDuration < 0)
            Destroy(gameObject);
    }
}
