using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBAttack : MonoBehaviour
{
    private TBState state = null;
    private TBEffectGenerator effectGenerator = null;
    [SerializeField] private Collider2D bodyAttack2D = null;
    [SerializeField] private TBAnimation tbAnimation = null;
    [SerializeField] private Transform shootTrans = null;
    [SerializeField] private AccessoriesLoopEffect tackleEffect = null;

    void Awake()
    {
        state = GetComponent<TBState>();
        effectGenerator = GetComponent<TBEffectGenerator>();
        tackleEffect.Init();
    }

    //攻撃待機開始。コントローラ―が呼び出す。
    public void ShootChargeStart()
    {
        if (state.CanCharge())
        {
            tbAnimation.ShootChargeTrigger();
            state.ChargeStart();
        }
    }
    public void TackleChargeStart()
    {
        if (state.CanCharge())
        {
            tbAnimation.TackleChargeTrigger();
            state.ChargeStart();
        }
    }

    public void TackleEffectSwitch(bool istackle)
    {
        tackleEffect.EffectSwitch(istackle);
    }

    //突撃終了。コントローラーが呼び出す。
    public void TackleEnd() 
    { 
        tbAnimation.TackleEndTrigger();
    }

    //弾を発射。アニメーションイベントが呼び出す。
    public void Shoot()
    {
        bool isFlip = (this.transform.localScale.x < 0f);
        effectGenerator.GenerateBullet(shootTrans.position, isFlip);
        effectGenerator.GenerateShootEffect(shootTrans.position, isFlip);
    }

    //体の攻撃判定オンオフ。ダメージスクリプトとイベントスクリプトが使う。
    public void BodyAttackSwitch(bool isactive) { bodyAttack2D.enabled = isactive; }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        tackleEffect.PauseSwitch(ispause);
    }
}
