using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//よろけステートから出る時に使う。よろけから通常状態にする。
public class TBDamageStateExit : StateMachineBehaviour
{
    private TBAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<TBAnimationEvents>(); }
        animationEvents?.DamageEnd();
    }
}
