using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossMove : MonoBehaviour
{
    [SerializeField] private float minTackleSpeed = 10; //�ő�ːi���x
    [SerializeField] private float maxTackleSpeed = 5; //�ŏ��ːi���x
    [SerializeField] private float maxYSpeed = 15; //�ő�y�����x
    [SerializeField] private float minYSpeed = 3; //�ŏ�y�����x
    [SerializeField] private float yAccelerationDistance = 3; //�ő�A�ŏ����x�ւ̑J�ڂɕK�v�ȋ���

    private IAimPlayer iaimPlayer = null;
    private TutorialBossState state = null; //�v���C���[��ԊǗ��X�N���v�g�B
    [SerializeField] private AffectedByFloor affectedByFloor = null; //���̉e���`�B�X�N���v�g�B
    [SerializeField] private GroundChecker wallTackleStopper = null; //���̉e���`�B�X�N���v�g�B
    [SerializeField] private GroundChecker groundTackleStopper = null; //���̉e���`�B�X�N���v�g�B

    private float tackleStartX = 0; //�ːi�J�n�ʒu
    private float tackleEndX = 0; //�ːi�I���ʒu
    private float fallStartY = 0; //�����J�n�ʒu

    private Rigidbody2D rigidBody2D = null;

    private void Awake()
    {
        iaimPlayer = GetComponent<IAimPlayer>();
        state = GetComponent<TutorialBossState>();
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

    //x���̓��͕����ɉ����Č������X�V�B���͊Ǘ��X�N���v�g���g���B
    private Vector3 UpdateScale()
    {
        Vector3 result = this.transform.localScale;
        if (state.CanTurn() && iaimPlayer.IsExistPlayer() && 
            this.transform.localScale.x * (this.transform.position.x - iaimPlayer.GetPlayerPos().x) > 0)
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        return result;
    }

    //�ːi���̊J�n�_�ƏI���_�ݒ�B�U���X�N���v�g���ĂԁB
    public void SetTacklePoint()
    {
        tackleStartX = this.transform.position.x;
        tackleEndX = iaimPlayer.GetPlayerPos().x + Mathf.Sign(this.transform.localScale.x)*2;
    }
    //�R���g���[���[���ĂԁB�ːi�I�����m�p�B
    public bool IsReachTacklePoint() 
    { 
        if(wallTackleStopper.IsGround() || !groundTackleStopper.IsGround() || Mathf.Abs(this.transform.position.x - tackleEndX) < 0.1f) {  return true; }
        return false; 
    }

    //x�����̑��x�v�Z
    public float UpdateXSpeed()
    {
        float result = 0f;
        if (state.IsTackleXMove())
        {
            float middleX = (tackleEndX + tackleStartX) / 2;
            float speedRatio = 1 - Mathf.Abs(this.transform.position.x - middleX) / Mathf.Abs(middleX - tackleStartX);
            speedRatio = Mathf.Clamp01(speedRatio);
            result = Mathf.Sign(this.transform.localScale.x) * (minTackleSpeed + (maxTackleSpeed - minTackleSpeed) * speedRatio);
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
        else if (state.IsFallYMove())
        {
            float speedRatio = Mathf.Abs(this.transform.position.y - fallStartY) / yAccelerationDistance;
            speedRatio = Mathf.Clamp01(speedRatio);
            result = -minYSpeed - (maxYSpeed - minYSpeed) * speedRatio + AffectedSpeed().y;
        }
        else if (state.IsStopYMove()) { result = 0; }
        return result;
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
