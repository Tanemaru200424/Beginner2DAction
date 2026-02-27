using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//少しの間だけ出るエフェクトのアニメーションが終了したら破壊するためのスクリプト。
public class InstantEffectEnd : StateMachineBehaviour
{
    // ステートが再生されている間、毎フレーム呼ばれる
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // アニメーションが99%以上再生されたら、後処理を実行
        if (stateInfo.normalizedTime >= 0.99f && !animator.IsInTransition(layerIndex))
        {
            Destroy(animator.gameObject);
        }
    }
}
