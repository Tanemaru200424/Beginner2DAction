using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerState state = null;
    [SerializeField] private PlayerEvents events = null;
    [SerializeField] private PlayerAttack attack = null;
    [SerializeField] private PlayerDamage damage = null;

    //アニメーションイベントで呼ぶ。
    public void Attack()
    {
        attack.Attack();
    }

    //ステートマシンビヘイビアで呼ぶ。
    public void DamageEnd()
    {
        state.DamageEnd();
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

    public void AttackClear()
    {
        attack.AttackClear();
    }
    public void AttackEnd()
    {
        state.AttackEnd();
    }
}