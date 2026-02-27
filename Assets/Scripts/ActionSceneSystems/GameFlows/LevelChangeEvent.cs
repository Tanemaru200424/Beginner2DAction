using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージのレベル切り替えとスタート地点の書き換え。
public class LevelChangeEvent : MonoBehaviour
{
    [SerializeField] private StageEventManager stageEventManager = null;
    [SerializeField] private ObjectCleaningManager objectCleaningManager = null;
    [SerializeField] private StageManager stageManager = null;
    [SerializeField] private PlayerGenerator playerGenerator = null;
    [SerializeField] private PauseManager pauseManager = null;
    [SerializeField] private FadeImageController fadeImageController = null;
    [SerializeField] private AreaManager offArea = null;
    [SerializeField] private LevelManager offLevel = null;
    [SerializeField] private LevelManager onLevel = null;
    [SerializeField] private CameraBrain cameraBrain = null;

    public void LevelChangeTrigger()
    {
        StartCoroutine(LevelChange());
    }

    private IEnumerator LevelChange()
    {
        if (stageEventManager.isEvent) { yield break; } //他のイベントが起きてるなら何もしない。
        stageEventManager.EventStart();

        //ステージ開始時の起動レベル変更。
        stageManager.UpdateStartLevel(onLevel);

        //プレイヤーを無敵にして入力を無効にする。
        playerGenerator.PlayerIncredibleSwitch(true);
        playerGenerator.PlayerInputActiveSwitch(false);

        //攻撃オブジェクトと敵をすべて削除。
        objectCleaningManager.Cleaning();

        //カメラがプレイヤーの後をついていかないようにする。
        cameraBrain.LinearSwitch(false);

        //停止予定エリアのギミックを停止。
        offArea.GimmickActiveSwitch(false);

        //起動レベルの準備。有効化はするが起動はしない。
        onLevel.StartArea.GimmickRegistSwitch(true);
        onLevel.StartArea.GimmickActiveSwitch(false);

        //イベントによる一時停止。
        pauseManager.PauseByEvent();

        //フェードイン待機
        yield return StartCoroutine(fadeImageController.FadeIn());

        //カメラの切り替え、地形の無効はフェードインで見えなくしてから行う。
        offLevel.GroundSwitch(false);
        offArea.AreaCameraSwitch(false);
        onLevel.GroundSwitch(true);
        onLevel.StartCameraSwitch(true);

        //もしプレイヤーがボス演出中に死んだなら再生成。
        //位置と向きを設定。
        //プレイヤーを通常状態に。
        playerGenerator.ReGeneratePlayer();
        playerGenerator.SetPlayerPos(onLevel.PlayerGeneratePos);
        playerGenerator.SetPlayerFlip(onLevel.IsPlayerStartFlip);
        playerGenerator.SetPlayerNomal();

        //停止予定エリアのギミックを無効化。
        offArea.GimmickRegistSwitch(false);

        //イベントによる一時停止解除。
        pauseManager.ResumeByEvent();

        //フェードアウト待機
        yield return StartCoroutine(fadeImageController.FadeOut());

        //開始エリアの起動
        onLevel.StartArea.GimmickActiveSwitch(true); 

        //カメラを開始位置固定からプレイヤーを追従するように設定。
        cameraBrain.LinearSwitch(true);
        onLevel.StartCameraSwitch(false);
        onLevel.StartArea.AreaCameraSwitch(true);

        //プレイヤーを無敵を無効にして入力を有効にする。
        playerGenerator.PlayerIncredibleSwitch(false);
        playerGenerator.PlayerInputActiveSwitch(true);

        stageEventManager.EventEnd();
    }
}
