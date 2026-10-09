using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//突進待機ステートから出たときに呼ぶ。待機状態から突進状態にする。
public class TutorialBossTackleChargeStateExit : StateMachineBehaviour
{
    //[SerializeField] private int loopCount = 2;
    private TutorialBossAnimationEvents animationEvents = null;

    //private int currentLoop = 0;

    /*
    //射撃開始時にループカウントリセット
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentLoop = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // normalizedTime:
        // 0.0 ～ 1.0 = 1ループ目
        // 1.0 ～ 2.0 = 2ループ目
        // 2.0 ～ 3.0 = 3ループ目
        int loop = Mathf.FloorToInt(stateInfo.normalizedTime);

        if (loop > currentLoop)
        {
            currentLoop = loop;

            if (currentLoop >= loopCount)
            {
                // 次の状態へ進める
                if (animationEvents == null) { animationEvents = animator.GetComponent<TutorialBossAnimationEvents>(); }
                animationEvents?.TackleChargeEnd();
            }
        }
    }
    */
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animationEvents == null) { animationEvents = animator.GetComponent<TutorialBossAnimationEvents>(); }
        animationEvents?.TackleChargeEnd();
    }
}
