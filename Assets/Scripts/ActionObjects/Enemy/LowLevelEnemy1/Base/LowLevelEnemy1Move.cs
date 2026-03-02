using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowLevelEnemy1Move : MonoBehaviour
{
    [SerializeField] private float maxYSpeed = 15;
    [SerializeField] private float minYSpeed = 3;
    [SerializeField] private float yAccelerationDistance = 3;
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float hittedSpeed = 20;
    [SerializeField] private float hittedDistance = 20;
    private Vector3 hittedStartPos = new Vector3(0, 0, 0);
    private Vector2 hittedVector = new Vector2 (0, 0);

    private LowLevelEnemy1State state = null;
    [SerializeField] private AffectedByFloor affectedByFloor = null;
    [SerializeField] private GroundChecker wallWalkStopper = null;

    private float fallStartY = 0;

    private Rigidbody2D rigidBody2D = null;

    private void Awake()
    {
        state = GetComponent<LowLevelEnemy1State>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (state.IsFallStart()) { fallStartY = this.transform.position.y; }
        this.transform.localScale = UpdateScale();
    }

    void FixedUpdate()
    {
        rigidBody2D.linearVelocity = new Vector2(UpdateXSpeed(), UpdateYSpeed());
    }

    private Vector3 UpdateScale()
    {
        Vector3 result = this.transform.localScale;
        if (state.CanTurn() && wallWalkStopper.IsGround())
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        return result;
    }

    public float UpdateXSpeed()
    {
        float result = 0f;
        if (state.IsWalkXMove())
        {
            result = Mathf.Sign(this.transform.localScale.x) * walkSpeed;
            result += AffectedSpeed().x;
        }
        else if (state.IsCantXMove()) { result = AffectedSpeed().x; }
        else if (state.IsStopXMove()) { result = 0; }
        else if(state.IsHittedXMove()) { result = hittedSpeed * hittedVector.x; }

        return result;
    }

    public float UpdateYSpeed()
    {
        float result = 0f;
        if (state.IsCantYMove()) { result = AffectedSpeed().y; }
        else if (state.IsFallYMove())
        {
            float speedRatio = Mathf.Abs(this.transform.position.y - fallStartY) / yAccelerationDistance;
            speedRatio = Mathf.Clamp01(speedRatio);
            result = -minYSpeed - (maxYSpeed - minYSpeed) * speedRatio + AffectedSpeed().y;
        }
        else if (state.IsStopYMove()) { result = 0; }
        else if (state.IsHittedXMove()) { result = hittedSpeed * hittedVector.y; }
        return result;
    }

    private Vector2 AffectedSpeed()
    {
        return new Vector2(affectedByFloor.AffectedFlowingFloor() + affectedByFloor.AffectedMovingFloor().x, affectedByFloor.AffectedMovingFloor().y);
    }


    public void HittedStart(Vector3 startPos, float angle)
    {
        hittedStartPos = startPos;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        hittedVector = rotation * Vector2.right.normalized;
        Vector3 currentScale = this.transform.localScale;
        if (currentScale.x * hittedVector.x > 0)
        {
            this.transform.localScale = Vector3.Scale(currentScale, new Vector3(-1, 1, 1));
        }
    }
    public bool IsHittedLimmit() { return Vector3.Distance(this.transform.position, hittedStartPos) > hittedDistance; }

    public void PauseSwitch(bool ispause)
    {
        if (ispause) { rigidBody2D.Sleep(); }
        else { rigidBody2D.WakeUp(); }
        this.enabled = !ispause;
    }
}
