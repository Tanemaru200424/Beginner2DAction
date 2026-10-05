using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLevelEnemyEvents : MonoBehaviour, ICharactorEvents
{
    private MiddleLevelEnemyState state = null;
    private MiddleLevelEnemyAttack attack = null;
    private MiddleLevelEnemyEffectGenerator effectGenerator = null;
    [SerializeField] private MiddleLevelEnemyAnimation mleAnimation = null;

    private Collider2D charactorCol2D = null; //本体の当たり判定

    void Awake()
    {
        state = GetComponent<MiddleLevelEnemyState>();
        attack = GetComponent<MiddleLevelEnemyAttack>();
        effectGenerator = GetComponent<MiddleLevelEnemyEffectGenerator>();
        charactorCol2D = GetComponent<Collider2D>();
    }

    public event System.Action OnBirthStart;
    public void BirthStart()
    {
        OnBirthStart?.Invoke();

        BirthEnd(); //開始イベントを直ぐに終了させる。
    }

    public event System.Action OnBirthEnd;
    public void BirthEnd()
    {
        OnBirthEnd?.Invoke();
    }

    public event System.Action OnDeathStart;
    public void DeathStart()
    {
        state.DeathStart();
        attack.BodyAttackSwitch(false);
        mleAnimation.DeathPlay();
        effectGenerator.BulletClear();
        effectGenerator.GenerateDeathEffect();
        OnDeathStart?.Invoke();
    }
    public void HittedStart()
    {
        state.HittedStart();
        attack.BodyAttackSwitch(false);
        attack.HittedAttackSwitch(true);
        mleAnimation.HittedPlay();
        effectGenerator.BulletClear();
        effectGenerator.GenerateHittedEffect();
        charactorCol2D.enabled = false;
        OnDeathStart?.Invoke();
    }

    public event System.Action OnDeathEnd;
    public void DeathEnd()
    {
        Destroy(this.gameObject);
        OnDeathEnd?.Invoke();
    }
    public void HittedEnd()
    {
        attack.HittedAttackSwitch(false);
        Destroy(this.gameObject);
        OnDeathEnd?.Invoke();
    }
}
