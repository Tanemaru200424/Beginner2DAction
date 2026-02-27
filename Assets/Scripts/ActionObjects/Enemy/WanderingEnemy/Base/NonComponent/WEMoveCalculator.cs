using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//徘徊ゾンビの移動速度計算
public class WEMoveCalculator
{
    //コントローラー側で設定して計算側に渡す。
    [System.Serializable]
    public struct MoveParameter
    {
        public float xSpeed; //歩きのx軸速度
        public float maxYSpeed; //最大落下y軸速度
        public float minYSpeed; //最小落下y軸速度
        public float yAccelerationDistance; //最大、最小速度への遷移に必要な距離
        public float hittedSpeed; //吹っ飛び速度
        public float hittedDistance; //吹っ飛び距離
    }

    private MoveParameter parameter;
    private WEActionState actionState = null;
    private AffectedByFloor affectedByFloor = null;

    private float fallStartY = 0;

    private Vector3 hittedStartPos = new Vector3(0, 0, 0); //吹っ飛び開始位置
    private Vector2 hittedVector = new Vector2(0, 0); //吹っ飛び方向

    public WEMoveCalculator(MoveParameter parameter, WEActionState actionState, AffectedByFloor affectedByFloor)
    {
        this.parameter = parameter;
        this.actionState = actionState;
        this.affectedByFloor = affectedByFloor;
    }

    //x軸の向きを更新。壁にあたったら反転する。
    public Vector3 UpdateScale(Vector3 currentScale, bool isWallGround)
    {
        Vector3 result = currentScale;
        //徘徊中に壁にぶつかる
        if (isWallGround &&
            actionState.currentState == WEActionState.State.IDLE &&
            actionState.isGround)
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        //吹っ飛び
        else if (actionState.currentState == WEActionState.State.HITTED &&
                 currentScale.x * hittedVector.x > 0)
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        return result;
    }

    //x方向の速度計算
    public float UpdateXSpeed(float scaleX)
    {
        float result = 0f;
        //徘徊中
        if (actionState.currentState == WEActionState.State.IDLE)
        {
            if (actionState.isGround)
            {
                float xSpeed = parameter.xSpeed;
                result = Mathf.Sign(scaleX) * xSpeed;
            }
            else { result = 0; }
            result += AffectedSpeed().x;
        }
        else if(actionState.currentState == WEActionState.State.DAMAGE)
        {
            result = AffectedSpeed().x;
        }
        else if(actionState.currentState == WEActionState.State.STOP)
        {
            result = 0;
        }
        //吹っ飛び
        else if(actionState.currentState == WEActionState.State.HITTED)
        {
            result = parameter.hittedSpeed * hittedVector.x;
        }
        return result;
    }

    //y方向の速度計算
    public float UpdateYSpeed(float currentY)
    {
        float result = 0f;
        float yAccelerationDistance = parameter.yAccelerationDistance;
        float maxYSpeed = parameter.maxYSpeed;
        float minYSpeed = parameter.minYSpeed;
        //ダメージ中と通常時のみ落下
        if (actionState.currentState == WEActionState.State.IDLE ||
            actionState.currentState == WEActionState.State.DAMAGE)
        {
            if (actionState.isGround) { result = 0; }
            else
            {
                float speedRatio = Mathf.Abs(currentY - fallStartY) / yAccelerationDistance;
                speedRatio = Mathf.Clamp01(speedRatio);
                result = -minYSpeed - (maxYSpeed - minYSpeed) * speedRatio;
            }
            result += AffectedSpeed().y;
        }
        else if (actionState.currentState == WEActionState.State.STOP)
        {
            result = 0;
        }
        //吹っ飛び
        else if (actionState.currentState == WEActionState.State.HITTED)
        {
            result = parameter.hittedSpeed * hittedVector.y;
        }
        return result;
    }

    public void FallStart(float currentY)
    {
        fallStartY = currentY;
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

    //移動床系の影響
    private Vector2 AffectedSpeed()
    {
        return new Vector2(affectedByFloor.AffectedFlowingFloor() + affectedByFloor.AffectedMovingFloor().x, affectedByFloor.AffectedMovingFloor().y);
    }
}
