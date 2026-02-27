using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//動的生成されずにエリアに最初からあるギミックはエリアによりゲームオブジェクトが有効無効されたらコンテナに登録、解除
//ギミック機能の活性、非活性は
public class GimmickObject : MonoBehaviour, IContainedObject, IActionObjectLabel, IAreaObject
{
    public event Action OnRegist;
    public event Action OnRemove;

    public event Action OnActive;
    public event Action OnDeactive;

    private ActionObjectLabel label = ActionObjectLabel.GIMMICK; //ラベルはギミック固定

    public ActionObjectLabel GetLabel() { return label; }

    public void ActiveSwitch(bool isactive)
    {
        if (isactive) { OnActive?.Invoke(); }
        else { OnDeactive?.Invoke(); }
    }

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