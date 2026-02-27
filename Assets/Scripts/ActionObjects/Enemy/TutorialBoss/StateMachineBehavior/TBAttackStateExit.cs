using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//突進、射撃の両方のステートから出る時に起こす。攻撃状態を終了させ、通常状態にする。
public class TBAttackStateExit : StateMachineBehaviour
{
    private TBAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<TBAnimationEvents>(); }
        animationEvents?.AttackEnd();
    }
}
