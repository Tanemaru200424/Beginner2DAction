using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class MiddleLevelEnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private MiddleLevelEnemyState state = null;
    [SerializeField] private MiddleLevelEnemyAttack attack = null;
    [SerializeField] private MiddleLevelEnemyDamage damage = null;
    [SerializeField] private MiddleLevelEnemyEvents events = null;

    //アニメーションイベントで起こす弾発射
    public void Attack()
    {
        attack.Attack();
    }

    //StateMachineBehaviorから呼び出す攻撃終了
    public void AttackEnd()
    {
        state.AttackEnd();
    }
    public void DamageEnd()
    {
        state.DamageEnd();
        attack.BodyAttackSwitch(true);
        damage.EffectOff();
    }
    public void DeathEnd()
    {
        events.DeathEnd();
    }
}
