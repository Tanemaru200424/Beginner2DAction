using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//退場時はアニメーション遷移が無いので再生時間で観測。
public class PlayerDeathStateEnd : StateMachineBehaviour
{
    private PlayerAnimationEvents animationEvents = null;

    // ステートが再生されている間、毎フレーム呼ばれる
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // アニメーションが99%以上再生されたら、後処理を実行
        if (stateInfo.normalizedTime >= 0.99f && !animator.IsInTransition(layerIndex))
        {
            if (animationEvents == null) { animationEvents = animator.GetComponent<PlayerAnimationEvents>(); }
            animationEvents.DeathEnd();
        }
    }
}
