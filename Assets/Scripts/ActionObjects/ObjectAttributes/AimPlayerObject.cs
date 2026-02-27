using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーを狙う敵やギミックに使う。位置だけ返すように注意。
public class AimPlayerObject : MonoBehaviour, IAimPlayer
{
    private Transform playerTrans = null;

    public void SetPlayerTrans(Transform playerTrans) { this.playerTrans = playerTrans; }
    public void CancelPlayerTrans() { playerTrans = null; }

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
