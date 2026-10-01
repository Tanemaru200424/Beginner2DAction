using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowLevelEnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private LowLevelEnemyState state = null;
    [SerializeField] private LowLevelEnemyAttack attack = null;
    [SerializeField] private LowLevelEnemyDamage damage = null;
    [SerializeField] private LowLevelEnemyEvents events = null;

    //ステートマシンビヘイビアで呼ぶ。
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
