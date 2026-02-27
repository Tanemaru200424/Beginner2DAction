using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの登場アニメーションが終了したら状態変化。
public class PlayerBirthStateExit : StateMachineBehaviour
{
    private PlayerAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<PlayerAnimationEvents>(); }
        animationEvents?.BirthEnd();
    }
}
