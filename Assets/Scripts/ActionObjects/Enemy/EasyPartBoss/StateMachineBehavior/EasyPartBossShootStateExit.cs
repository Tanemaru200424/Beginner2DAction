using UnityEngine;

//空中射撃は数回ループした後に終了する。
public class EasyPartBossShootStateExit : StateMachineBehaviour
{
    [SerializeField] private int loopCount = 3;
    private EasyPartBossAnimationEvents epbAnimationEvents = null;

    private int currentLoop = 0;

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

        if (loop >= currentLoop)
        {
            currentLoop = loop;

            if (currentLoop >= loopCount)
            {
                // 次の状態へ進める
                if (epbAnimationEvents == null) { epbAnimationEvents = animator.GetComponent<EasyPartBossAnimationEvents>(); }
                if (epbAnimationEvents != null)
                {
                    epbAnimationEvents.ShootEnd();
                }
            }
        }
    }
}