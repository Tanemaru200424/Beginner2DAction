using UnityEngine;
using System;
using System.Collections;

//ステージスタート、ゲームオーバー、クリアなど
public class GameCycleEvents : MonoBehaviour
{
    [SerializeField] private StageEventManager stageEventManager = null;
    [SerializeField] private ObjectCleaningManager objectCleaningManager = null;
    [SerializeField] private StageManager stageManager = null;
    [SerializeField] private CameraBrain cameraBrain = null;
    [SerializeField] private ActionUIController actionUIController = null;
    [SerializeField] private PlayerGenerator playerGenerator = null;
    [SerializeField] private FadeImageController fadeImageController = null;

    void Awake()
    {
        playerGenerator.OnPlayerDeath += () => { StartCoroutine(GameOver()); };
    }

    void Start() { StartCoroutine(StageStart()); } //最初にステージ開始させる。

    //ステージ開始処理。
    private IEnumerator StageStart()
    {
        if (stageEventManager.isEvent) { yield break; } //他のイベントが起きてるなら何もしない。
        stageEventManager.EventStart();

        //ゲーム開始前に弾や敵の全消去、エリアとレベルの全停止、アクションUI非表示、開始エリアのギミック表示。
        objectCleaningManager.Cleaning();
        stageManager.AllDeactive();
        actionUIController.UIClear();
        stageManager.ActiveStartGround();
        stageManager.StartAreaGimmickRegist();

        //開始時のカメラ設定。
        cameraBrain.AutoSwitch(true);
        cameraBrain.LinearSwitch(false);
        stageManager.StartCameraSwitch(true);

        yield return StartCoroutine(fadeImageController.FadeOut());

        //プレイヤーを入力無効、無敵状態で生成して登場アニメーション再生。
        playerGenerator.GeneratePlayer();
        playerGenerator.PlayerIncredibleSwitch(true);
        playerGenerator.PlayerInputActiveSwitch(false);
        playerGenerator.PlayerBirthStart();

        while (playerGenerator.isEntry) { yield return null; }

        //追従カメラ起動。
        stageManager.ActiveStartAreaCamera();
        cameraBrain.LinearSwitch(true);
        stageManager.StartCameraSwitch(false);

        //UI表示
        actionUIController.PlayerUISwitch(true);

        //ギミック起動。
        stageManager.StartAreaGimmickActive();

        //無敵解除と入力有効化。
        playerGenerator.PlayerIncredibleSwitch(false);
        playerGenerator.PlayerInputActiveSwitch(true);

        stageEventManager.EventEnd();
    }

    //ゲームオーバー処理。
    private IEnumerator GameOver()
    {
        if (stageEventManager.isEvent) { yield break; } //他のイベントが起きてるなら何もしない。
        stageEventManager.EventStart();

        //カメラの追従停止。
        cameraBrain.AutoSwitch(false);

        //プレイヤーの無敵と入力無効。
        playerGenerator.PlayerIncredibleSwitch(true);
        playerGenerator.PlayerInputActiveSwitch(false);

        while (playerGenerator.isRetire) { yield return null; }
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeImageController.FadeIn());

        stageEventManager.EventEnd();
        StartCoroutine(StageStart()); //再スタート処理。
    }
}
