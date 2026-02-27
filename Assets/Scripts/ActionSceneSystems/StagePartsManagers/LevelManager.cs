using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//エリアを複数もつ単位。
//地形オンオフを含む。
//レベルを乗り越える毎にプレイヤーの生成位置、初期エリア、プレイヤー生成時固定カメラが変わる。（チェックポイント更新）
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject groundObject = null; //地形オブジェクト
    [SerializeField] private GameObject backGroundObject = null; //背景オブジェクト
    [SerializeField] private Transform playerGenerateTrans = null; //プレイヤー生成位置
    [SerializeField] private AreaManager startArea = null; //開始エリア
    [SerializeField] private CinemachineVirtualCamera startCamera = null; //プレイヤー生成中のカメラ

    //レベルで共通のものはステージ管理者がまとめて登録
    private CameraBrain cameraBrain = null;

    public Vector3 PlayerGeneratePos => playerGenerateTrans.position;
    public bool IsPlayerStartFlip => playerGenerateTrans.localScale.x < 0;
    public AreaManager StartArea => startArea;

    //ステージ管理者に呼んでもらう。開始演出時プレイヤー追従カメラを登録。
    public void SetCameraBrain(CameraBrain cameraBrain) { this.cameraBrain = cameraBrain; }

    //地形のオンオフ。
    public void GroundSwitch(bool isactive)
    {
        groundObject.SetActive(isactive); 
        backGroundObject.SetActive(isactive);
    }

    //演出カメラのオンオフ
    public void StartCameraSwitch(bool isactive)
    {
        if (isactive) { cameraBrain.SetStartCamera(startCamera, playerGenerateTrans); }
        else { cameraBrain.CancelStartCamera(startCamera); }
    }
}
