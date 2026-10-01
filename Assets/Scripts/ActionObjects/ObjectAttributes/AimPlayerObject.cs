using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーを狙う敵やギミックに使う。位置だけ返すように注意。
public class AimPlayerObject : MonoBehaviour, IAimPlayer
{
    private Transform playerTrans = null;

    //生成されたとき、ギミックが起動したときにターゲット初期化により設定。
    public void SetPlayerTrans(Transform playerTrans) { this.playerTrans = playerTrans; }
    public Transform GetPlayerTrans() { return playerTrans; }

    //CancelしなくてもNullチェックが他の関数に入っているので不要と考えた。
    //public void CancelPlayerTrans() { playerTrans = null; }

    public bool IsExistPlayer() {  return playerTrans != null; }
    public Vector3 GetPlayerPos() 
    {
        if (IsExistPlayer())
        {
            return playerTrans.position;
        }
        else
        {
            return this.transform.position;
        }
    }
}
