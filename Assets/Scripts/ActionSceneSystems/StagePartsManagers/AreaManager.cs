using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//ステージのエリア毎に付ける。ギミックの有効化無効化を行う。
//ステージ管理者に登録。
public class AreaManager : MonoBehaviour
{
    //エリアごとに参照が違うものはインスペクタで登録
    [SerializeField] private List<GameObject> gimmickObjects = new List<GameObject>();//自身に属するギミックオブジェクト
    [SerializeField] private CinemachineVirtualCamera cvCamera = null; //エリアを移すカメラ

    //エリアで共通のものはステージ管理者がまとめて登録
    private CameraBrain cameraBrain = null;
    private AimPlayerManager aimPlayerManager = null;

    //ステージ管理者に最初に呼んでもらう。
    //所属するギミックにコンテナへの登録解除イベント注入。
    public void SetGimmickToContainer(IObjectContainer iobjectContainer)
    {
        foreach (GameObject refObject in gimmickObjects)
        {
            IContainedObject icontainedObject = refObject.GetComponent<IContainedObject>();
            icontainedObject.OnRegist += () => iobjectContainer.RegistObject(refObject);
            icontainedObject.OnRemove += () => iobjectContainer.RemoveObject(refObject);

            IGenerator igenerator = refObject.GetComponent<IGenerator>();
            igenerator?.SetObjectContainer(iobjectContainer);
        }
    }
    //ステージ管理者に最初に呼んでもらう。
    //ギミック起動時、追跡機能があるギミックにプレイヤーの位置情報を登録。
    public void SetAimPlayerManager(AimPlayerManager aimPlayerManager) { this.aimPlayerManager = aimPlayerManager; }
    //ステージ管理者に呼んでもらう。エリア起動時のプレイヤー追従カメラを登録。
    public void SetCameraBrain(CameraBrain cameraBrain) { this.cameraBrain = cameraBrain; }

    //エリアを移すカメラを起動、停止する。
    public void AreaCameraSwitch(bool isactive)
    {
        if (isactive) { cameraBrain.SetPlayerCamera(cvCamera); }
        else { cameraBrain.CancelPlayerCamera(cvCamera); }
    }
    //所属するギミックをまとめて起動、停止する。
    public void GimmickActiveSwitch(bool isactive)
    {
        foreach (GameObject refObject in gimmickObjects)
        {
            IAimPlayer iaimPlayer = refObject.GetComponent<IAimPlayer>();
            if (iaimPlayer != null)
            {
                if (isactive) { aimPlayerManager.InitSetPlayerTrans(iaimPlayer); }
                else { iaimPlayer.CancelPlayerTrans(); }
            }
            IAreaObject iareaObject = refObject.GetComponent<IAreaObject>();
            iareaObject.ActiveSwitch(isactive);
        }
    }
    //エリア内のギミックオブジェクトの有効化、無効化。
    //ギミック側のスクリプトによってコンテナに登録、解除が実行される。
    public void GimmickRegistSwitch(bool isactive)
    {
        foreach (GameObject refObject in gimmickObjects)
        {
            refObject.SetActive(isactive);
        }
    }
}
