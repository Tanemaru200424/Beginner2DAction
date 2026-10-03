using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//退場時はアニメーション遷移が無いので再生時間で観測。
public class PlayerDeathStateExit : StateMachineBehaviour
{
    private PlayerAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))
        {
            if (animationEvents == null) { animationEvents = animator.GetComponent<PlayerAnimationEvents>(); }
            animationEvents.DeathEnd();
        }
    }
}
