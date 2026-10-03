using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//死亡ステート終了時に呼ぶ。オブジェクトの破棄、死亡イベント終了。
public class TutorialBossDeathStateExit : StateMachineBehaviour
{
    private TutorialBossAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))
        {
            if (animationEvents == null) { animationEvents = animator.GetComponent<TutorialBossAnimationEvents>(); }
            animationEvents.DeathEnd();
        }
    }
}
