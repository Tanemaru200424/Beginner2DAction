using UnityEngine;
using System;
using System.Collections;

public class BossBattleEvents : MonoBehaviour
{
    private BossGenerator bossGenerator = null;
    [SerializeField] private BossGate bossGate = null;
    [SerializeField] private FadeImageController fadeImageController = null;
    [SerializeField] private StageEventManager stageEventManager = null;
    [SerializeField] private PlayerGenerator playerGenerator = null;
    [SerializeField] private ActionUIController actionUIController = null;
    [SerializeField] private StageManager stageManager = null;
    [SerializeField] private ObjectCleaningManager objectCleaningManager = null;
    [SerializeField] private CameraBrain cameraBrain = null;
    [SerializeField] private AreaManager offArea = null;
    [SerializeField] private LevelManager offLevel = null;
    [SerializeField] private LevelManager onLevel = null;

    void Awake()
    {
        bossGenerator = GetComponent<BossGenerator>();
        bossGenerator.OnBossDeath += () => { BattleEndTrigger(); };
    }

    public void BattleStartTrigger() { StartCoroutine(BossBattleStart()); }
    private IEnumerator BossBattleStart()
    {
        if (stageEventManager.isEvent) { yield break; } //他のイベントが起きてるなら何もしない。
        stageEventManager.EventStart();

        //プレイヤーを無敵にして入力を無効にする。
        playerGenerator.PlayerIncredibleSwitch(true);
        playerGenerator.PlayerInputActiveSwitch(false);

        bossGate.CloseStart();
        while (!bossGate.IsClose()) { yield return null; }

        bossGenerator.GenerateBoss();
        bossGenerator.BossBirthStart();
        actionUIController.BossUISwitch(true);

        while (bossGenerator.isEntry) { yield return null; }

        //プレイヤーを無敵を無効にして入力を有効にする。
        playerGenerator.PlayerIncredibleSwitch(false);
        playerGenerator.PlayerInputActiveSwitch(true);

        stageEventManager.EventEnd();
    }

    public void BattleEndTrigger() { StartCoroutine(BossBattleEnd()); }
    private IEnumerator BossBattleEnd()
    {
        if (stageEventManager.isEvent) { yield break; } //他のイベントが起きてるなら何もしない。
        stageEventManager.EventStart();

        //プレイヤーを無敵にして入力を無効にする。
        playerGenerator.PlayerIncredibleSwitch(true);
        playerGenerator.PlayerInputActiveSwitch(false);

        stageManager.UpdateStartLevel(onLevel);

        while (bossGenerator.isRetire) { yield return null; }

        actionUIController.BossUISwitch(false);

        //攻撃オブジェクトと敵をすべて削除。
        objectCleaningManager.Cleaning();

        //カメラがプレイヤーの後をついていかないようにする。
        cameraBrain.LinearSwitch(false);

        //停止予定エリアのギミックを停止。
        offArea.GimmickActiveSwitch(false);

        //起動レベルの準備。有効化はするが起動はしない。地形生成。
        onLevel.StartArea.GimmickRegistSwitch(true);
        onLevel.StartArea.GimmickActiveSwitch(false);

        yield return new WaitForSeconds(1);

        //フェードイン待機
        yield return StartCoroutine(fadeImageController.FadeIn());

        //カメラの切り替え、地形の無効はフェードインで見えなくしてから行う。
        offLevel.GroundSwitch(false);
        offArea.AreaCameraSwitch(false);
        onLevel.GroundSwitch(true);
        onLevel.StartCameraSwitch(true);

        //もしプレイヤーがボス演出中に死んだなら再生成。
        //通常状態にして位置と向きを設定。
        playerGenerator.ReGeneratePlayer();
        playerGenerator.SetPlayerNomal();
        playerGenerator.SetPlayerPos(onLevel.PlayerGeneratePos);
        playerGenerator.SetPlayerFlip(onLevel.IsPlayerStartFlip);

        //フェードアウト待機
        yield return StartCoroutine(fadeImageController.FadeOut());

        //停止予定エリアのギミックを無効化。
        offArea.GimmickRegistSwitch(false);

        //開始エリアの起動
        onLevel.StartArea.GimmickActiveSwitch(true);

        //カメラを開始位置固定からプレイヤーを追従するように設定。
        cameraBrain.LinearSwitch(true);
        onLevel.StartArea.AreaCameraSwitch(true);
        onLevel.StartCameraSwitch(false);

        //プレイヤーを無敵を無効にして入力を有効にする。
        playerGenerator.PlayerIncredibleSwitch(false);
        playerGenerator.PlayerInputActiveSwitch(true);

        stageEventManager.EventEnd();
    }
}
