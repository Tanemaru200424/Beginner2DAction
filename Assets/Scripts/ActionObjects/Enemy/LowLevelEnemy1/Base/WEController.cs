using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEController : MonoBehaviour
{
    private Rigidbody2D rigidBody2D = null;
    private Collider2D c2D = null; //�{�̂̓����蔻��
    private WEAccessoriesGenerator accessoriesGenerator = null;
    [SerializeField] private Collider2D hitboxC2D = null; //�H�炢����
    [SerializeField] private Collider2D bodyAttackC2D = null; //�̂̍U������
    [SerializeField] private Collider2D hittedAttackC2D = null; //������эU������
    [SerializeField] private Animator animator = null;

    [SerializeField] private GroundChecker groundChecker = null;
    private bool isGround = false;
    [SerializeField] private GroundChecker wallChecker = null;
    private bool isWallGround = false;

    [SerializeField] private WEMoveCalculator.MoveParameter moveParameter;
    [SerializeField] private AffectedByFloor affectedByFloor = null;

    private WEActionState actionState = null;
    private WEMoveCalculator moveCalculator = null;
    private WEAnimationTransition animationTransition = null;

    private bool isHitted = false;
    private float zAngle = 0;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        c2D = GetComponent<Collider2D>();
        accessoriesGenerator = GetComponent<WEAccessoriesGenerator>();
        c2D.enabled = true;
        hitboxC2D.enabled = true;
        bodyAttackC2D.enabled = true;
        hittedAttackC2D.enabled = false;
        actionState = new WEActionState();
        moveCalculator = new WEMoveCalculator(moveParameter, actionState, affectedByFloor);
        animationTransition = new WEAnimationTransition(animator);
    }

    void Update()
    {
        if (actionState.currentState == WEActionState.State.IDLE && !isGround)
        {
            moveCalculator.FallStart(this.transform.position.y);
        }

        animationTransition.UpdateAnimation(isGround);
    }

    void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
        isWallGround = wallChecker.IsGround();

        actionState.UpdateGroundFlag(isGround);

        this.transform.localScale = moveCalculator.UpdateScale(this.transform.localScale, isWallGround);
        float moveXSpeed = moveCalculator.UpdateXSpeed(this.transform.localScale.x);
        float moveYSpeed = moveCalculator.UpdateYSpeed(this.transform.position.y);

        rigidBody2D.linearVelocity = new Vector2(moveXSpeed, moveYSpeed);
    }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
    }

    //�_���[�W�X�N���v�g�ɌĂ�ł��炤�B
    public void DamageStart()
    {
        animationTransition.DamagePlay();
        actionState.SetDAMAGE();
        bodyAttackC2D.enabled = false;
    }
    public void HittedSwitch(bool ishitted) { isHitted = ishitted; }
    public void SetHittedAngle(float angle) { zAngle = angle; }

    //�p�j�G�l�~�[�C�x���g���ĂԁB
    public void BirthStart()
    {
        actionState.SetSTOP();
        hitboxC2D.enabled = false;
        bodyAttackC2D.enabled = false;
    }
    public void BirthEnd()
    {
        actionState.SetIDLE();
        hitboxC2D.enabled = true;
        bodyAttackC2D.enabled = true;
    }
    public void DeathStart()
    {
        if (isHitted)
        {
            moveCalculator.HittedStart(this.transform.position, zAngle);
            animationTransition.HittedPlay();
            actionState.SetHITTED();
            c2D.enabled = false;
            hitboxC2D.enabled = false;
            bodyAttackC2D.enabled = false;
            hittedAttackC2D.enabled = true;
        }
        else
        {
            animationTransition.DeathPlay();
            actionState.SetSTOP();
            c2D.enabled = false;
            hitboxC2D.enabled = false;
            bodyAttackC2D.enabled = false;
        }
    }
    public bool IsHittedEnd()
    {
        return actionState.currentState == WEActionState.State.HITTED && !moveCalculator.IsHitted(this.transform.position);
    }
    public void DeathEnd()
    {
        accessoriesGenerator.GenerateRetireEffect();
        Destroy(this.gameObject);
    }

    //�A�j���[�V�����C�x���g�ŌĂ�ł��炤
    public void DamageEnd() 
    { 
        actionState.DAMAGEEnd();
        bodyAttackC2D.enabled = true;
    }
}
