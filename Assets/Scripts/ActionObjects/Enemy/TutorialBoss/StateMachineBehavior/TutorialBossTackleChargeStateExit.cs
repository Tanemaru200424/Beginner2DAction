using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//突進待機ステートから出たときに呼ぶ。待機状態から突進状態にする。
public class TutorialBossTackleChargeStateExit : StateMachineBehaviour
{
    private TutorialBossAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<TutorialBossAnimationEvents>(); }
        animationEvents?.TackleChargeEnd();
    }
}
