using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーのダメージアニメーションが終了したら状態変化。
public class PlayerDamageStateExit : StateMachineBehaviour
{
    private PlayerAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<PlayerAnimationEvents>(); }
        animationEvents?.DamageEnd();
    }
}
