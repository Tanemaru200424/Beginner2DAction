using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シンプルな徘徊エネミーの行動
public class WEActionState
{
    public enum State { IDLE, DAMAGE, STOP, HITTED } //IDLEは通常状態。
                                                     //DAMAGEはダメージを受けている状態。横移動はノックバック。縦移動は落下か停止。
                                                     //STOPは停止状態。縦移動、横移動ともに無し。登場、退場時。
                                                     //HITTEDは吹き飛び。指定方向に吹っ飛ぶ。
    public bool isGround { get; private set; } = true; //着地フラッグ。参照のみ可能。サブステートのようなもの。

    public State currentState { get; private set; } = State.IDLE;

    //コンストラクタ
    public WEActionState()
    {
        currentState = State.IDLE;
        isGround = true;
    }

    //通常時
    public void SetIDLE() { currentState = State.IDLE; }

    //ダメージ時
    public void SetDAMAGE() { currentState = State.DAMAGE; }

    //停止時
    public void SetSTOP() { currentState = State.STOP; }

    //吹き飛び時
    public void SetHITTED() {  currentState = State.HITTED; }

    //ダメージ終わり
    public void DAMAGEEnd()
    {
        if (currentState == State.DAMAGE) { currentState = State.IDLE; }
    }

    //着地状態更新
    public void UpdateGroundFlag(bool isGround) { this.isGround = isGround; }
}
