using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�C���G�l�~�[�̐���X�N���v�g
public class SEController : MonoBehaviour
{
    private Rigidbody2D rigidBody2D = null;
    private Collider2D c2D = null;
    private SEAccessoriesGenerator accessoriesGenerator = null;
    private SEAttack attack = null;

    [SerializeField] private Collider2D hitboxC2D = null; //�H�炢����
    [SerializeField] private Collider2D bodyAttackC2D = null; //�̂̍U������
    [SerializeField] private Collider2D hittedAttackC2D = null; //������эU������

    //�A�j���[�V������2�̕��ʂɕ�����Ă���B
    [SerializeField] private Animator baseAnim = null;
    [SerializeField] private Animator shooterAnim = null;

    [SerializeField] private SEMoveCalculator.MoveParameter moveParameter;

    private SEActionState actionState = null;
    private SEMoveCalculator moveCalculator = null;
    private SEAnimationTransition animationTransition = null;

    private bool isHitted = false;
    private float zAngle = 0;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        c2D = GetComponent<Collider2D>();
        accessoriesGenerator = GetComponent<SEAccessoriesGenerator>();
        attack = GetComponent<SEAttack>();

        hitboxC2D.enabled = true;
        bodyAttackC2D.enabled = true;
        hittedAttackC2D.enabled = false;
        actionState = new SEActionState();
        moveCalculator = new SEMoveCalculator(moveParameter, actionState);
        animationTransition = new SEAnimationTransition(baseAnim, shooterAnim);
    }

    void Update()
    {
        if (actionState.currentState == SEActionState.State.IDLE)
        {
            attack.ChargeSwitch(true);
        }
        else if(actionState.currentState == SEActionState.State.ATTACK)
        {
            if (!attack.IsExistTarget()) 
            {
                animationTransition.AttackCancel();
            }
        }

        animationTransition.UpdateAnimation(attack.ChargeLevel());

        if (attack.IsChargeComplete() && actionState.CanATTACK()) 
        {
            AttackStart();
        }
    }

    void FixedUpdate()
    {
        this.transform.localScale = moveCalculator.UpdateScale(this.transform.localScale);
        float moveXSpeed = moveCalculator.UpdateXSpeed();
        float moveYSpeed = moveCalculator.UpdateYSpeed();

        rigidBody2D.linearVelocity = new Vector2(moveXSpeed, moveYSpeed);
    }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
    }

    private void AttackStart()
    {
        animationTransition.AttackPlay();
        actionState.SetATTACK();
        attack.ChargeSwitch(false);
    }

    //�_���[�W�X�N���v�g�ɌĂ�ł��炤�B
    public void AttackCancel() { animationTransition.AttackCancel(); } //�U����ԂŃ^�[�Q�b�g���������Ă��ĂԁB
    public void HittedSwitch(bool ishitted) { isHitted = ishitted; }
    public void SetHittedAngle(float angle) { zAngle = angle; }

    //�C���G�l�~�[�C�x���g���ĂԁB
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
        attack.ChargeSwitch(false);
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
            animationTransition.DeadPlay();
            actionState.SetSTOP();
            c2D.enabled = false;
            hitboxC2D.enabled = false;
            bodyAttackC2D.enabled = false;
        }
    }
    public bool IsHittedEnd()
    {
        return actionState.currentState == SEActionState.State.HITTED && !moveCalculator.IsHitted(this.transform.position);
    }
    public void DeathEnd()
    {
        accessoriesGenerator.GenerateRetireEffect();
        Destroy(this.gameObject);
    }

    //�A�j���[�V�����C�x���g�ŌĂ�ł��炤
    public void AttackEnd() 
    { 
        actionState.ATTACKEnd();
        attack.ChargeSwitch(true);
        attack.ResetChargeTime();
    }
}
