using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//シーン上の全てのプレイヤー追跡仮想カメラにターゲットを追加解除
//一時停止でカメラの挙動を止める。
public class CameraBrain : MonoBehaviour
{
    private Transform playerTrans = null; //プレイヤー生成時に設定してもらう。
    private CinemachineVirtualCamera nowPlayerCamera = null; //今プレイヤーに追従しているカメラ。原則1つ。
    private CinemachineVirtualCamera nowStartCamera = null; //今のゲーム開始演出でプレイヤーに追従しているカメラ。原則1つ。
    private CinemachineBrain cinemachineBrain = null;

    void Awake()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    //プレイヤーの生成、死亡に応じてプレイヤーカメラの追従対象を登録、解除。
    public void SetPlayerTrans(Transform trans) { playerTrans = trans; }
    public void CancelPlayerTrans() { playerTrans = null; }

    //カメラ追従等の自動計算を止める。
    public void AutoSwitch(bool isAuto)
    {
        if (isAuto) { cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate; }
        else { cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.ManualUpdate; }
    }

    //カメラをなめらかに動かすか、直ぐに位置を変えるようにするか。
    public void LinearSwitch(bool isLinear)
    {
        if (isLinear) { cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Linear; }
        else { cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut; }
    }

    //ステージ進行時プレイヤーに追従するカメラの設定。プレイヤーカメラの優先度は必ず1が限度。イベント等は1より大きく。
    public void SetPlayerCamera(CinemachineVirtualCamera playerCamera)
    {
        if(playerTrans != null && playerCamera != null)
        {
            nowPlayerCamera = playerCamera;
            nowPlayerCamera.enabled = true;
            nowPlayerCamera.Priority = 1;
            nowPlayerCamera.Follow = playerTrans;
        }
    }
    public void CancelPlayerCamera(CinemachineVirtualCamera playerCamera)
    {
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
            playerCamera.Priority = 0;
            playerCamera.Follow = null;
        }
        if(nowPlayerCamera == playerCamera) { nowPlayerCamera.enabled = false; }
    }

    //ステージ開始時の起動レベルのカメラ設定。
    public void SetStartCamera(CinemachineVirtualCamera startCamera, Transform startTrans)
    {
        if (startTrans != null && startCamera != null)
        {
            nowStartCamera = startCamera;
            nowStartCamera.enabled = true;
            nowStartCamera.Priority = 2;
            nowStartCamera.Follow = startTrans;
        }
    }
    public void CancelStartCamera(CinemachineVirtualCamera startCamera)
    {
        if (startCamera != null)
        {
            startCamera.enabled = false;
            startCamera.Priority = 0;
            startCamera.Follow = null;
        }
        if(nowStartCamera == startCamera) { nowStartCamera.enabled = false; }
    }

    //カメラがブレンド中
    public bool IsCameraBlend() { return cinemachineBrain.ActiveBlend != null; }

    //メインカメラの位置を返す
    public Vector3 MainCameraPos() { return this.transform.position; }
}
