using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowLevelEnemyDeathStateExit : StateMachineBehaviour
{
    private LowLevelEnemyAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (!animator.IsInTransition(layerIndex))
        {
            if (animationEvents == null) { animationEvents = animator.GetComponent<LowLevelEnemyAnimationEvents>(); }
            animationEvents.DeathEnd();
        }
    }
}