using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//射撃待機ステートから出たときに呼ぶ。待機状態から射撃状態にする。
public class TutorialBossShootChargeStateExit : StateMachineBehaviour
{
    private TutorialBossAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<TutorialBossAnimationEvents>(); }
        animationEvents?.ShootChargeEnd();
    }
}
