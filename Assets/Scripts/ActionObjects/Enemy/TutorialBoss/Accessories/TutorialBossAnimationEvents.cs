using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossAnimationEvents : MonoBehaviour
{
    [SerializeField] private TutorialBossState state = null;
    [SerializeField] private TutorialBossEvents events = null;
    [SerializeField] private TutorialBossAttack attack = null;
    [SerializeField] private TutorialBossMove move = null;
    [SerializeField] private TutorialBossDamage damage = null;

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
