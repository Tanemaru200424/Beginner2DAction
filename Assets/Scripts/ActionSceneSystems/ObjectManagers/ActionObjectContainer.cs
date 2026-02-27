using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アクション部分を担うオブジェクトたちを分類し格納する。
//生成能力を持つスクリプトはこれをインターフェースとして参照にする必要あり。
//一時停止やエリア掃除の管理者はこれらオブジェクトのうち担当インターフェース等を参照してオブジェクトを管理する。
//生成時に登録。有効無効等に応じて登録、解除する。
public class ActionObjectContainer : MonoBehaviour, IObjectContainer
{
    private GameObject playerObject = null;//有効プレイヤーオブジェクト。原則1つ。
    public GameObject PlayerObject => playerObject;

    private List<GameObject> enemyObjects = new List<GameObject>();//有効敵オブジェクト。
    public IReadOnlyList<GameObject> EnemyObjects => enemyObjects;

    private List<GameObject> attackObjects = new List<GameObject>();//有効攻撃オブジェクト。
    public IReadOnlyList<GameObject> AttackObjects => attackObjects;

    private List<GameObject> gimmickObjects = new List<GameObject>();//有効ステージギミックオブジェクト。
    public IReadOnlyList<GameObject> GimmickObjects => gimmickObjects;

    //生成者が生成オブジェクトを作った時このコンテナスクリプトを渡す。
    //生成オブジェクトは状態に応じて登録や解除を行う。
    //登録
    public void RegistObject(GameObject obj)
    {
        if (obj == null) { return; }
        IActionObjectLabel iactionObjectLabel = obj.GetComponent<IActionObjectLabel>();
        if(iactionObjectLabel == null) { return; }

        ActionObjectLabel label = iactionObjectLabel.GetLabel();
        if (label == ActionObjectLabel.PLAYER) 
        {
            if (playerObject == null) { playerObject = obj; }
        }
        else if (label == ActionObjectLabel.ENEMY) 
        {
            if (!enemyObjects.Contains(obj)) { enemyObjects.Add(obj); }
        }
        else if (label == ActionObjectLabel.ATTACK)
        {
            if (!attackObjects.Contains(obj)) { attackObjects.Add(obj); }
        }
        else if (label == ActionObjectLabel.GIMMICK)
        {
            if (!gimmickObjects.Contains(obj)) { gimmickObjects.Add(obj); }
        }
    }
    //解除
    public void RemoveObject(GameObject obj)
    {
        if (obj == null) { return; }
        IActionObjectLabel iactionObjectLabel = obj.GetComponent<IActionObjectLabel>();
        if (iactionObjectLabel == null) { return; }

        ActionObjectLabel label = iactionObjectLabel.GetLabel();
        if (label == ActionObjectLabel.PLAYER)
        {
            if (playerObject != null) { playerObject = null; }
        }
        else if (label == ActionObjectLabel.ENEMY) 
        {
            if (enemyObjects.Contains(obj)) { enemyObjects.Remove(obj); }
        }
        else if (label == ActionObjectLabel.ATTACK)
        {
            if (attackObjects.Contains(obj)) { attackObjects.Remove(obj); }
        }
        else if (label == ActionObjectLabel.GIMMICK)
        {
            if (gimmickObjects.Contains(obj)) { gimmickObjects.Remove(obj); }
        }
    }
}
