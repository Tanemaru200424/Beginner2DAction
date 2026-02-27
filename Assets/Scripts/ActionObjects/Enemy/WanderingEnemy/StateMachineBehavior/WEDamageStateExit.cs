using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEDamageStateExit : StateMachineBehaviour
{
    private WEAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<WEAnimationEvents>(); }
        animationEvents?.DamageEnd();
    }
}
