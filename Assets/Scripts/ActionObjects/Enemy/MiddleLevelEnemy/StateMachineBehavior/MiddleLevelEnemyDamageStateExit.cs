using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLevelEnemyDamageStateExit : StateMachineBehaviour
{
    private MiddleLevelEnemyAnimationEvents animationEvents = null;

    // ステートが再生されている間、毎フレーム呼ばれる
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<MiddleLevelEnemyAnimationEvents>(); }
        animationEvents?.DamageEnd();
    }
}