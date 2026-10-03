using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAttackStateExit : StateMachineBehaviour
{
    private SEShooterAnimationEvents shooterAnimationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (shooterAnimationEvents == null) { shooterAnimationEvents = animator.GetComponent<SEShooterAnimationEvents>(); }
        shooterAnimationEvents?.AttackEnd();
    }
}
