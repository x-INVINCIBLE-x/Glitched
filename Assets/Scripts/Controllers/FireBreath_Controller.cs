using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath_Controller : MonoBehaviour
{
    private Animator animator;

    private float duration;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Setup(float _duration)
    {
        duration = _duration;

        StartCoroutine(FinishFireBreath());
    }

    private IEnumerator FinishFireBreath()
    {
        yield return new WaitForSeconds(duration);

        animator.SetTrigger("Finish");

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
