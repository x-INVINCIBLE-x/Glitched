using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{

    [SerializeField] private int damage;
    [SerializeField] private string targetLayerName = "Player";


    [SerializeField] private float xVelocity;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    private CharacterStats stats;

    [Header("Glitch")]
    [SerializeField] private float glitchDuration = 0.2f;
    [SerializeField] private float lastTimeGlitched = 0;
    [SerializeField] private float glitchSpeedMultiplier;

    private void Update()
    {
        if(canMove)
            rb.velocity = new Vector2(xVelocity * speedMultiplier, rb.velocity.y);
    }

    public void SetupBullet( float _speed,CharacterStats _stats)
    {
        xVelocity = _speed;
        stats = _stats;

        StartCoroutine(StartGlitchTimer());
    }

    IEnumerator StartGlitchTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            TryGlitch();
        }
    }

    public void TryGlitch()
    {
        if (Time.time < lastTimeGlitched + glitchDuration)
            return;

        bool canGLitch = Random.Range(0,1f) < 0.5f;

        if (!canGLitch)
            return;

        Invoke(nameof(ResetGlitch), glitchDuration - 0.05f);
        lastTimeGlitched = Time.time;
        speedMultiplier = glitchSpeedMultiplier;
    }

    private void ResetGlitch()
    {
        speedMultiplier = 1.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<CharacterStats>()?.isInvincible == true)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {

            //collision.GetComponent<CharacterStats>()?.TakePhysicalDamage(damage);


            stats.DoDamage(collision.GetComponent<CharacterStats>());

            if (targetLayerName == "Enemy")
                Destroy(gameObject);

            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        ////////////////////////////////
        ///
        Destroy(gameObject);

        /////////////////////////////////////
        //GetComponentInChildren<ParticleSystem>().Stop();

        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(5, 7));
    }

    public void FlipArrow()
    {
        if (flipped)
            return;


        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
}
