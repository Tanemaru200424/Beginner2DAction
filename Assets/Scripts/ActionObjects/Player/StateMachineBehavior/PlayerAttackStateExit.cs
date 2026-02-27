using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの攻撃アニメーションが終わったら攻撃全部消して状態変化。
public class PlayerAttackStateExit : StateMachineBehaviour
{
    private PlayerAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animationEvents == null) { animationEvents = animator.GetComponent<PlayerAnimationEvents>(); }
        animationEvents?.AttackClear();
        animationEvents?.AttackEnd();
    }
}
