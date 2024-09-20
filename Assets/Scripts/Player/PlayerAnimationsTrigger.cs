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
}
