using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アクションシーンで王的に作られたり、ギミックの一部に付ける。
//プレイヤーを狙うオブジェクトのターゲット登録解除を行う。
public class AimPlayerManager : MonoBehaviour
{
    [SerializeField] private ActionObjectContainer actionObjectContainer = null;//追従オブジェクトを含む様々なオブジェクトが入れられている。

    //プレイヤー追跡オブジェクトに対して使う。通常はプレイヤーの最新位置、参照できない場合NULLを返す。
    //インスペクターでセットしないTransformならむやみに変更する権利は渡すべきでないと考えてとりあえず実装。
    private Func<Vector3?> GetPlayerPosition => () =>
    {
        if (actionObjectContainer.PlayerObject == null) return null;
        return actionObjectContainer.PlayerObject.transform.position;
    };

    //プレイヤーをターゲットにとるオブジェクトが生成されたらこれでセット。
    public void InitSetPlayerTrans(IAimPlayer iaimPlayer)
    {
        if (actionObjectContainer.PlayerObject != null) 
        {
            iaimPlayer?.SetPlayerTrans(actionObjectContainer.PlayerObject.transform);
        }
    }

    //プレイヤー死亡時の一斉ターゲット解除
    public void AllCancelPlayerTrans()
    {
        List<GameObject> aimObjects = new List<GameObject>();
        aimObjects.Add(actionObjectContainer.PlayerObject);
        aimObjects.AddRange(actionObjectContainer.EnemyObjects);
        aimObjects.AddRange(actionObjectContainer.AttackObjects);
        aimObjects.AddRange(actionObjectContainer.GimmickObjects);
        foreach (GameObject obj in aimObjects)
        {
            IAimPlayer iaimPlayer = obj?.GetComponent<IAimPlayer>();
            iaimPlayer?.CancelPlayerTrans();
        }
    }
}
