using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//少しの間だけ出るエフェクトのアニメーションが終了したら破壊するためのスクリプト。
//終了後ダミーのステートに遷移するようにしておく。ステートの終了時に破壊するようにする。
public class InstantEffectExit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!animator.IsInTransition(layerIndex))
        {
            Destroy(animator.gameObject);
        }
    }
}
