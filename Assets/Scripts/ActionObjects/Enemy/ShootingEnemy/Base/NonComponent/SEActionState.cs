using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//シンプルな砲台系エネミーの行動
public class SEActionState
{
    public enum State { IDLE, ATTACK, STOP, HITTED } //IDLEは通常状態。攻撃待機状態でもある。ゆっくり追尾。
                                                     //ATTACKは攻撃状態。停止状態。
                                                     //STOPはイベントで停止状態。
                                                     //HITTEDは吹き飛び。指定方向に吹っ飛ぶ。

    public State currentState { get; private set; } = State.IDLE;

    //コンストラクタ
    public SEActionState()
    {
        currentState = State.IDLE;
    }

    //通常時
    public void SetIDLE() { currentState = State.IDLE; }

    //攻撃可能状態か
    public bool CanATTACK() { return currentState == State.IDLE; }
    //攻撃時
    public void SetATTACK() { currentState = State.ATTACK; }
    //攻撃終わり
    public void ATTACKEnd()
    {
        if (currentState == State.ATTACK) { currentState = State.IDLE; }
    }

    //停止時
    public void SetSTOP() { currentState = State.STOP; }

    //吹き飛び時
    public void SetHITTED() { currentState = State.HITTED; }
}
