using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//各イベント発生スクリプトが参照にする道具を入れたもの。
public class StageEventManager : MonoBehaviour
{
    public bool isEvent { get; private set; } = false; //イベント再生中か。他のイベント実行者達が衝突しないようにする。

    void Awake() { isEvent = false; }

    //イベント開始終了時に使う。
    public void EventStart() { isEvent = true; }
    public void EventEnd() { isEvent = false; }
}