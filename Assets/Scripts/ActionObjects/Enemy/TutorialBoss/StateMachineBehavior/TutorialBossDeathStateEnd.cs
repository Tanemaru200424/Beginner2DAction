using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//死亡ステート終了時に呼ぶ。オブジェクトの破棄、死亡イベント終了。
public class TutorialBossDeathStateEnd : StateMachineBehaviour
{
    private TutorialBossAnimationEvents animationEvents = null;

    // ステートが再生されている間、毎フレーム呼ばれる
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // アニメーションが99%以上再生されたら、後処理を実行
        if (stateInfo.normalizedTime >= 0.99f && !animator.IsInTransition(layerIndex))
        {
            if (animationEvents == null) { animationEvents = animator.GetComponent<TutorialBossAnimationEvents>(); }
            animationEvents.DeathEnd();
        }
    }
}
