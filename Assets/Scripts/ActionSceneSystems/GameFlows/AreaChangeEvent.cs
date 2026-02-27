using UnityEngine;
using System.Collections;

public class AreaChangeEvent : MonoBehaviour
{
    [SerializeField] private StageEventManager stageEventManager = null;
    [SerializeField] private PlayerGenerator playerGenerator = null;
    [SerializeField] private CameraBrain cameraBrain = null;
    [SerializeField] private ObjectCleaningManager objectCleaningManager = null;
    [SerializeField] private PauseManager pauseManager = null;
    [SerializeField] private AreaManager offArea = null;
    [SerializeField] private AreaManager onArea = null;

    public void AreaChangeTrigger()
    {
        StartCoroutine(AreaChange());
    }

    private IEnumerator AreaChange()
    {
        if (stageEventManager.isEvent) { yield break; } //他のイベントが起きてるなら何もしない。
        stageEventManager.EventStart();

        //プレイヤーを無敵にして入力を無効にする。
        playerGenerator.PlayerIncredibleSwitch(true);
        playerGenerator.PlayerInputActiveSwitch(false);

        //停止予定のエリアのギミックとカメラを停止。
        offArea.GimmickActiveSwitch(false);
        offArea.AreaCameraSwitch(false);

        //スクロール終了で今存在する敵と攻撃を削除。
        objectCleaningManager.Cleaning();

        //起動予定のエリアのギミックを停止状態で有効化しカメラを起動
        onArea.GimmickRegistSwitch(true);
        onArea.GimmickActiveSwitch(false);
        onArea.AreaCameraSwitch(true);

        //イベントによる一時停止。
        pauseManager.PauseByEvent();

        //スクロールが終わるまで待機。
        yield return null;
        while (cameraBrain.IsCameraBlend()) { yield return null; }

        //イベントによる一時停止解除。
        pauseManager.ResumeByEvent();

        //停止予定エリアのギミックを無効化。
        offArea.GimmickRegistSwitch(false);

        //起動予定エリアのギミックを起動。
        onArea.GimmickActiveSwitch(true);

        //プレイヤーの入力有効化と無敵終了
        playerGenerator.PlayerIncredibleSwitch(false);
        playerGenerator.PlayerInputActiveSwitch(true);
        stageEventManager.EventEnd();
    }
}
