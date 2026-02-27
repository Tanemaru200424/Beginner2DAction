using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アクションにおける一時停止を担う。
public class PauseManager : MonoBehaviour
{
    [SerializeField] private ActionObjectContainer actionObjectContainer = null;//ポーズ対象が納められたコンテナ
    [SerializeField] private CameraBrain cameraBrain = null;
    [SerializeField] private PauseImageController pauseImageController = null;
    private bool isPause = false;

    private enum PauseType {NONE, PLAYER, EVENT}; //今何による一時停止がされているか。
    private PauseType pauseType = PauseType.NONE;

    void Awake() { pauseType = PauseType.NONE; }

    //プレイヤーの入力による一時停止
    public void PlayerPause()
    {
        if (!isPause) { PauseByPlayer(); }
        else { ResumeByPlayer(); }
    }

    //プレイヤーによる停止と解除。カメラ追従ごと止まる。
    private void PauseByPlayer()
    {
        if (pauseType == PauseType.NONE && !isPause)
        {
            pauseType = PauseType.PLAYER;
            isPause = true;
            List<GameObject> pausedObjects = new List<GameObject>();
            pausedObjects.Add(actionObjectContainer.PlayerObject);
            pausedObjects.AddRange(actionObjectContainer.EnemyObjects);
            pausedObjects.AddRange(actionObjectContainer.AttackObjects);
            pausedObjects.AddRange(actionObjectContainer.GimmickObjects);
            foreach (GameObject obj in pausedObjects)
            {
                IPausable ipausable = obj.GetComponent<IPausable>();
                ipausable?.Paused();
            }
            cameraBrain.AutoSwitch(false);
            pauseImageController.DisplaySwitch(true);
        }
    }
    private void ResumeByPlayer()
    {
        if (pauseType == PauseType.PLAYER && isPause)
        {
            pauseType = PauseType.NONE;
            isPause = false;
            List<GameObject> resumedObjects = new List<GameObject>();
            resumedObjects.Add(actionObjectContainer.PlayerObject);
            resumedObjects.AddRange(actionObjectContainer.EnemyObjects);
            resumedObjects.AddRange(actionObjectContainer.AttackObjects);
            resumedObjects.AddRange(actionObjectContainer.GimmickObjects);
            foreach (GameObject obj in resumedObjects)
            {
                IPausable ipausable = obj.GetComponent<IPausable>();
                ipausable?.Resumed();
            }
            cameraBrain.AutoSwitch(true);
            pauseImageController.DisplaySwitch(false);
        }
    }

    //ステージスクロールなどのイベント中、アクションオブジェクトだけを止めたいときに使う。
    public void PauseByEvent()
    {
        if (pauseType == PauseType.NONE && !isPause)
        {
            pauseType = PauseType.EVENT;
            isPause = true;
            List<GameObject> pausedObjects = new List<GameObject>();
            pausedObjects.Add(actionObjectContainer.PlayerObject);
            pausedObjects.AddRange(actionObjectContainer.EnemyObjects);
            pausedObjects.AddRange(actionObjectContainer.AttackObjects);
            pausedObjects.AddRange(actionObjectContainer.GimmickObjects);
            foreach (GameObject obj in pausedObjects)
            {
                IPausable ipausable = obj.GetComponent<IPausable>();
                ipausable?.Paused();
            }
        }
    }
    public void ResumeByEvent()
    {
        if (pauseType == PauseType.EVENT && isPause)
        {
            pauseType = PauseType.NONE;
            isPause = false;
            List<GameObject> resumedObjects = new List<GameObject>();
            resumedObjects.Add(actionObjectContainer.PlayerObject);
            resumedObjects.AddRange(actionObjectContainer.EnemyObjects);
            resumedObjects.AddRange(actionObjectContainer.AttackObjects);
            resumedObjects.AddRange(actionObjectContainer.GimmickObjects);
            foreach (GameObject obj in resumedObjects)
            {
                IPausable ipausable = obj.GetComponent<IPausable>();
                ipausable?.Resumed();
            }
        }
    }
}
