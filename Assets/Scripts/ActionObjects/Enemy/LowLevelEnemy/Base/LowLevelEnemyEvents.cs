using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//徘徊ゾンビの登場退場イベント
public class LowLevelEnemyEvents : MonoBehaviour, ICharactorEvents
{
    private LowLevelEnemyState state = null;
    private LowLevelEnemyAttack attack = null;
    private LowLevelEnemyEffectGenerator effectGenerator = null;
    [SerializeField] private LowLevelEnemyAnimation lleAnimation = null;

    private Collider2D charactorCol2D = null; //本体の当たり判定

    void Awake()
    {
        state = GetComponent<LowLevelEnemyState>();
        attack = GetComponent<LowLevelEnemyAttack>();
        effectGenerator = GetComponent<LowLevelEnemyEffectGenerator>();
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
        lleAnimation.DeathPlay();
        effectGenerator.GenerateDeathEffect();
        OnDeathStart?.Invoke();
    }
    public void HittedStart()
    {
        state.HittedStart();
        attack.BodyAttackSwitch(false);
        attack.HittedAttackSwitch(true);
        lleAnimation.HittedPlay();
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
    public void AreaOverExtinction()
    {
        state.DeathStart();
        attack.BodyAttackSwitch(false);
        OnDeathStart?.Invoke();
        OnDeathEnd?.Invoke();
        Destroy(this.gameObject);
    }
}
