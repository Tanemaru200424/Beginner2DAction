using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBAnimationEvents : MonoBehaviour
{
    [SerializeField] private TBState state = null;
    [SerializeField] private TBEvents events = null;
    [SerializeField] private TBAttack attack = null;
    [SerializeField] private TBMove move = null;
    [SerializeField] private TBDamage damage = null;

    //アニメーションイベントで呼ぶ。
    public void Shoot()
    {
        attack.Shoot();
    }

    //ステートマシンビヘイビアで呼ぶ。
    public void DamageEnd()
    {
        state.DamageEnd();
        attack.BodyAttackSwitch(true);
        damage.EffectOff();
    }
    public void BirthEnd()
    {
        events.BirthEnd();
    }
    public void DeathEnd()
    {
        events.DeathEnd();
    }
    public void ShootChargeEnd()
    {
        state.ShootChargeEnd();
    }
    public void TackleChargeEnd()
    {
        state.TackleChargeEnd();
        move.SetTacklePoint();
        attack.TackleEffectSwitch(true);
    }
    public void AttackEnd()
    {
        state.AttackEnd();
        attack.TackleEffectSwitch(false);
    }
}
