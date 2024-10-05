using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_Controller : MonoBehaviour
{
    private Animator anim;
    private CharacterStats myStats;
    private float growSpeed = 15;
    private float maxSize = 6;
    private float explosionRadius;

    private bool canGrow = true;
    public float damage = 0;

    private void Update()
    {
        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (maxSize - transform.localScale.x < .5f)
        {
            canGrow = false;
            anim.SetTrigger("Explode");
        }

    }


    public void SetupExplosive(CharacterStats _myStats,float _growSpeed,float _maxSize,float _radius, float _damage)
    {
        anim= GetComponent<Animator>();

        myStats= _myStats;
        growSpeed= _growSpeed;
        maxSize= _maxSize;
        explosionRadius = _radius;
        damage = _damage;
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out CharacterStats enemyStat))
            {

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                //myStats.DoDamage(hit.GetComponent<CharacterStats>());
                enemyStat.TakePhysicalDamage(damage);

                //hit.GetComponent<Player>()?.fx.ScreenShake(new Vector3(2, 2));
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}
