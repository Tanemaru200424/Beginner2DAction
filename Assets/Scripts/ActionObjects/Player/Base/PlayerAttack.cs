using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerState state = null; //プレイヤー状態管理スクリプト。
    private PlayerDataForUI dataForUI = null; //プレイヤーのHPバーコントロール。
    [SerializeField] private PlayerAnimation pAnimation = null;
    [SerializeField] private PlayerNomalAttackObject nomalAttackObject = null;
    [SerializeField] private PlayerChargeAttackObject chargeAttackObject = null;
    [SerializeField] private PlayerChargeEffect chargeEffect = null;


    [SerializeField] private float chargeTime = 1;//溜め2時間
    private float nowChargeTime = 0;//現在の溜め時間

    private bool isHoldAttack = false;
    private bool isStandby = false;
    private bool isCharged = false;
    private float zAngle = 0;

    void Awake()
    {
        state = GetComponent<PlayerState>();
        dataForUI = GetComponent<PlayerDataForUI>();
        nomalAttackObject.Init();
        chargeAttackObject.Init();
        chargeEffect.Init();
    }

    void Start()
    {
        dataForUI.ChargeChanged(nowChargeTime / chargeTime);
    }

    void Update()
    {
        if (isStandby && nowChargeTime < chargeTime)
        {
            nowChargeTime += Time.deltaTime;
            if (nowChargeTime < chargeTime) { chargeEffect.ChargeComplete(false); }
            else { chargeEffect.ChargeComplete(true); }
        }

        if (isHoldAttack) 
        {
            if (state.CanAttack()) { isStandby = true; }
            else 
            { 
                isStandby = false;
                nowChargeTime = 0;
            }
        }
        else
        {
            if (state.CanAttack() && isStandby) { AttackStart(); }
            isStandby = false;
            nowChargeTime = 0;
        }

        dataForUI.ChargeChanged(nowChargeTime / chargeTime);
        chargeEffect.ChargeSwitch(isStandby);
    }

    //攻撃入力継続しているか。入力管理スクリプトが使う。
    public void HoldAttack(bool isHold) { isHoldAttack = isHold; }
    //攻撃方向指定。入力管理スクリプトが使う。
    public void SetShift(float shift)
    {
        if (this.transform.localScale.x > 0)
        {
            if (shift > 0) { zAngle = 45; }
            else if (shift < 0) { zAngle = -45; }
            else { zAngle = 0; }
        }
        else
        {
            if (shift > 0) { zAngle = 135; }
            else if (shift < 0) { zAngle = -135; }
            else { zAngle = 180; }
        }
    }

    //攻撃開始。
    private void AttackStart()
    {
        isCharged = (nowChargeTime >= chargeTime);
        pAnimation.AttackTrigger();
        state.AttackStart();
    }
    //攻撃発生時にアニメーションイベントで呼ぶ。
    public void Attack()
    {
        AttackClear();
        if (!isCharged) { nomalAttackObject.AttackSwitch(true); }
        else
        {
            chargeAttackObject.SetAngle(zAngle);
            chargeAttackObject.AttackSwitch(true);
        }
    }
    //攻撃終了時やステート抜けたときに攻撃オブジェクトを全てクリアにする。
    public void AttackClear()
    {
        nomalAttackObject.AttackSwitch(false);
        chargeAttackObject.AttackSwitch(false);
    }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        nomalAttackObject.PauseSwitch(ispause);
        chargeAttackObject.PauseSwitch(ispause);
        chargeEffect.PauseSwitch(ispause);
    }
}