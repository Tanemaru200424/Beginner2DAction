using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDeathStateEnd : StateMachineBehaviour
{
    private SEBaseAnimationEvents baseAnimationEvents = null;

    // ステートが再生されている間、毎フレーム呼ばれる
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // アニメーションが99%以上再生されたら、後処理を実行
        if (stateInfo.normalizedTime >= 0.99f && !animator.IsInTransition(layerIndex))
        {
            if (baseAnimationEvents == null) { baseAnimationEvents = animator.GetComponent<SEBaseAnimationEvents>(); }
            baseAnimationEvents?.DeathEnd();
        }
    }
}