using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージのエリアに存在する敵や攻撃オブジェクトなどを一掃するためにオブジェクトを管理する。
//一掃はステージのエリア切り替えやクリア時等で実行する。
public class ObjectCleaningManager : MonoBehaviour
{
    [SerializeField] private ActionObjectContainer actionObjectContainer = null;//この中からエリア切り替えで破棄するオブジェクトを取り出す

    //エリアの敵と攻撃オブジェクトを全て破棄。
    //エリア切り替えやステージクリア等で呼び出す。
    public void Cleaning()
    {
        List<GameObject> clearedObjects = new List<GameObject>();
        clearedObjects.AddRange(actionObjectContainer.EnemyObjects);
        clearedObjects.AddRange(actionObjectContainer.AttackObjects);
        foreach (GameObject obj in clearedObjects)
        {
            Destroy(obj);
        }
    }
}
