using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//登場ステートから出た時に呼ぶ。登場イベント終了して通常状態にする。
public class TBEntryStateExit : StateMachineBehaviour
{
    private TBAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<TBAnimationEvents>(); }
        animationEvents?.BirthEnd();
    }
}
