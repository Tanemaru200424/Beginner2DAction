using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�̈ړ��Ɋւ��鏈�����s���B
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float xSpeed = 6; //x�����x
    [SerializeField] private float knockBackSpeed = 10; //�m�b�N�o�b�N����x�����x
    [SerializeField] private float maxYSpeed = 15; //�ő�y�����x
    [SerializeField] private float minYSpeed = 3; //�ŏ�y�����x
    [SerializeField] private float yAccelerationDistance = 3; //�ő�A�ŏ����x�ւ̑J�ڂɕK�v�ȋ���

    private PlayerState state = null; //�v���C���[��ԊǗ��X�N���v�g�B
    [SerializeField] private AffectedByFloor affectedByFloor = null; //���̉e���`�B�X�N���v�g�B

    private float xDirection = 0; //�v���C���[�̉��ړ������B
    private float jumpStartY = 0; //�W�����v�J�n�ʒu
    private float fallStartY = 0; //�����J�n�ʒu

    private Rigidbody2D rigidBody2D = null;

    private void Awake()
    {
        state = GetComponent<PlayerState>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (state.IsFallStart()) { fallStartY = this.transform.position.y; }
        state.UpdateLowMax(this.transform.position.y < fallStartY);
        this.transform.localScale = UpdateScale();
    }

    void FixedUpdate()
    {
        rigidBody2D.linearVelocity = new Vector2(UpdateXSpeed(), UpdateYSpeed());
    }

    //x���̓��͕����ɉ����Č������X�V�B���͊Ǘ��X�N���v�g���g���B
    public void SetXDirection(float direction) { xDirection = direction; }
    private Vector3 UpdateScale()
    {
        Vector3 result = this.transform.localScale;
        if (state.CanTurn() && xDirection * result.x < 0)
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        return result;
    }

    //x�����̑��x�v�Z
    public float UpdateXSpeed()
    {
        float result = 0f;
        if (state.IsNomalXMove())
        {
            if (xDirection != 0) { result = Mathf.Sign(xDirection) * xSpeed; }
            else { result = 0; }
            result += AffectedSpeed().x;
        }
        else if (state.IsDamageXMove())
        {
            result = -Mathf.Sign(this.transform.localScale.x) * knockBackSpeed;
            result += AffectedSpeed().x;
        }
        else if (state.IsCantXMove()) { result = AffectedSpeed().x; }
        else if (state.IsStopXMove()) { result = 0; }

        return result;
    }

    //y�����̑��x�v�Z
    public float UpdateYSpeed()
    {
        float result = 0f;
        if (state.IsNomalYMove()) { result = AffectedSpeed().y; }
        else if (state.IsJumpYMove())
        {
            float speedRatio = Mathf.Abs(this.transform.position.y - jumpStartY) / yAccelerationDistance;
            speedRatio = Mathf.Clamp01(speedRatio);
            result = maxYSpeed - (maxYSpeed - minYSpeed) * speedRatio + AffectedSpeed().y;
        }
        else if (state.IsFallYMove())
        {
            float speedRatio = Mathf.Abs(this.transform.position.y - fallStartY) / yAccelerationDistance;
            speedRatio = Mathf.Clamp01(speedRatio);
            result = -minYSpeed - (maxYSpeed - minYSpeed) * speedRatio + AffectedSpeed().y;
        }
        else if (state.IsStopYMove()) { result = 0; }
        return result;
    }

    //���݂̍�������W�����v�Ɨ����̊J�n�ʒu�ݒ�B���͊Ǘ��X�N���v�g���g���B
    public void JumpStart()
    {
        if (state.CanJump())
        {
            jumpStartY = this.transform.position.y;
            fallStartY = this.transform.position.y + yAccelerationDistance; 
            state.UpdateLowMax(this.transform.position.y < fallStartY);
            state.JumpStart();
        }
    }

    //�ړ����n�̉e���𔽉f�B
    private Vector2 AffectedSpeed()
    {
        return new Vector2(affectedByFloor.AffectedFlowingFloor() + affectedByFloor.AffectedMovingFloor().x, affectedByFloor.AffectedMovingFloor().y);
    }

    //�ꎞ��~�B�ꎞ��~�Ǘ��X�N���v�g���g���B
    public void PauseSwitch(bool ispause)
    {
        if (ispause) { rigidBody2D.Sleep(); }
        else { rigidBody2D.WakeUp(); }
        this.enabled = !ispause;
    }
}
