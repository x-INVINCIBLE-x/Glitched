using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsTrigger : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void CallTrigger() => player.AnimationTrigger();
    private void ActivateFireSwordAnimator() => player.SetFireSwordController();

    private void AttackTrigger()
    {
        //AudioManager.instance.PlaySFX(2, null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out EnemyStats _target))
                    player.stats.DoDamage(_target); 
        }
    }
}
