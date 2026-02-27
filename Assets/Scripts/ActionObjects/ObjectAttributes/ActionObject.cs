using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//アクションシーンで動的に生成されるオブジェクト。プレイヤー、敵、弾丸等。
public class ActionObject : MonoBehaviour, IContainedObject, IActionObjectLabel
{
    public event Action OnRegist;
    public event Action OnRemove;

    [SerializeField] private ActionObjectLabel label = ActionObjectLabel.PLAYER;

    public ActionObjectLabel GetLabel() { return label; }

    //有効化、無効化に応じてコンテナに登録、解除を行う。
    void OnEnable() 
    { 
        OnRegist?.Invoke(); 
    }
    void OnDisable() 
    {
        OnRemove?.Invoke(); 
    }
}