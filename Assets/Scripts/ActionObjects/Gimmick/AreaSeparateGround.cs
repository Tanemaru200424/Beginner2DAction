using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エリアを区切る床の移動制限。
//プレハブを用意し、そのタイルマップをオーバーライドして形を調整する。
public class AreaSeparateGround : MonoBehaviour
{
    private BoxCollider2D boxCollider2D = null;
    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        IAreaObject iareaObject = GetComponent<IAreaObject>();
        iareaObject.OnActive += () =>
        {
            boxCollider2D.enabled = true;
        };
        iareaObject.OnDeactive += () =>
        {
            boxCollider2D.enabled = false;
        };
    }
}
