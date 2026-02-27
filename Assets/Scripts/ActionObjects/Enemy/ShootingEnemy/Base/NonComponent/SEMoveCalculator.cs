using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEMoveCalculator
{
    //コントローラー側で設定して計算側に渡す。
    [System.Serializable]
    public struct MoveParameter
    {
        public float hittedSpeed; //吹っ飛び速度
        public float hittedDistance; //吹っ飛び距離
    }

    private MoveParameter parameter;
    private SEActionState actionState = null;

    private Vector3 hittedStartPos = new Vector3(0, 0, 0); //吹っ飛び開始位置
    private Vector2 hittedVector = new Vector2(0, 0); //吹っ飛び方向

    public SEMoveCalculator(MoveParameter parameter, SEActionState actionState)
    {
        this.parameter = parameter;
        this.actionState = actionState;
    }

    //x軸の向き設定。吹っ飛ばし時に反映。
    public Vector3 UpdateScale(Vector3 currentScale)
    {
        Vector3 result = currentScale;
        if (actionState.currentState == SEActionState.State.HITTED &&
                 currentScale.x * hittedVector.x > 0)
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        return result;
    }

    //x方向の速度計算
    public float UpdateXSpeed()
    {
        float result = 0f;
        if (actionState.currentState == SEActionState.State.HITTED)
        {
            result = parameter.hittedSpeed * hittedVector.x;
        }
        return result;
    }

    //y方向の速度計算
    public float UpdateYSpeed()
    {
        float result = 0f;
        if (actionState.currentState == SEActionState.State.HITTED)
        {
            result = parameter.hittedSpeed * hittedVector.y;
        }
        return result;
    }

    //吹っ飛び開始
    public void HittedStart(Vector3 startPos, float angle)
    {
        hittedStartPos = startPos;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        hittedVector = rotation * Vector2.right.normalized;
    }
    //吹っ飛び途中か
    public bool IsHitted(Vector3 nowPos)
    {
        return Vector3.Distance(hittedStartPos, nowPos) <= parameter.hittedDistance;
    }
}
