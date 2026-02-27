using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージの管理。エリアの活性化、非活性化を行う。
public class StageManager : MonoBehaviour
{
    [SerializeField] private AimPlayerManager aimPlayerManager = null;//エリアに渡すプレイヤー追跡管理者。
    [SerializeField] private ActionObjectContainer actionObjectContainer = null;//エリアに渡すコンテナ。
    [SerializeField] private CameraBrain cameraBrain = null; //レベルとエリアに渡すカメラ。

    [SerializeField] private List<LevelManager> levelManagers = new List<LevelManager>();
    [SerializeField] private List<AreaManager> areaManagers = new List<AreaManager>();
    [SerializeField] private LevelManager initStartLevel = null; //最初の開始レベル。
    private LevelManager nowStartLevel = null; //現在の開始レベル。
    public Vector3 PlayerGeneratePos => nowStartLevel.PlayerGeneratePos;
    public bool IsPlayerStartFlip => nowStartLevel.IsPlayerStartFlip;

    //シーン起動時にはコンテナの登録と全エリアの無効化を行う。
    void Awake()
    {
        IObjectContainer iobjectContainer = actionObjectContainer.GetComponent<IObjectContainer>();
        foreach(AreaManager areaManager in areaManagers)
        {
            areaManager.SetAimPlayerManager(aimPlayerManager);
            areaManager.SetGimmickToContainer(iobjectContainer);
            areaManager.SetCameraBrain(cameraBrain);
        }
        foreach(LevelManager levelManager in levelManagers)
        {
            levelManager.SetCameraBrain(cameraBrain);
        }
        nowStartLevel = initStartLevel;
    }

    //ステージの全てのエリア、レベルを無効化させる。ギミックも同様。
    public void AllDeactive()
    {
        foreach (AreaManager areaManager in areaManagers)
        {
            areaManager.AreaCameraSwitch(false);
            areaManager.GimmickActiveSwitch(false);
            areaManager.GimmickRegistSwitch(false);
        }
        foreach(LevelManager levelManager in levelManagers)
        {
            levelManager.GroundSwitch(false);
            levelManager.StartCameraSwitch(false);
        }
    }

    //ステージ開始時のレベル起動
    public void ActiveStartGround() { nowStartLevel.GroundSwitch(true); }
    public void StartCameraSwitch(bool isactive) { nowStartLevel.StartCameraSwitch(isactive); }

    //ステージ開始エリアの起動
    public void ActiveStartAreaCamera() { nowStartLevel.StartArea.AreaCameraSwitch(true); }
    public void StartAreaGimmickRegist() { nowStartLevel.StartArea.GimmickRegistSwitch(true); }
    public void StartAreaGimmickActive() { nowStartLevel.StartArea.GimmickActiveSwitch(true); }

    //レベル移動時にチェックポイント更新。
    public void UpdateStartLevel(LevelManager newLevelManager)
    {
        nowStartLevel = newLevelManager;
    }
}
