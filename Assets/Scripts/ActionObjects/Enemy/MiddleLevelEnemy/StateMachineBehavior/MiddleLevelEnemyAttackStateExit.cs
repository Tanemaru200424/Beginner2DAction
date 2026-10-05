using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLevelEnemyAttackStateExit : StateMachineBehaviour
{
    private MiddleLevelEnemyAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<MiddleLevelEnemyAnimationEvents>(); }
        animationEvents?.AttackEnd();
    }
}
