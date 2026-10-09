using UnityEngine;

public class EasyPartBossBirthStateExit : StateMachineBehaviour
{
    private EasyPartBossAnimationEvents animationEvents = null;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (!animator.IsInTransition(layerIndex))
        {
            if (animationEvents == null) { animationEvents = animator.GetComponent<EasyPartBossAnimationEvents>(); }
            animationEvents.BirthEnd();
        }
    }
}
