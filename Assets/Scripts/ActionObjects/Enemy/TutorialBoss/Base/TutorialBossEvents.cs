using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossEvents : MonoBehaviour, ICharactorEvents
{
    private TutorialBossState state = null;
    private TutorialBossAttack attack = null;
    private TutorialBossEffectGenerator effectGenerator = null;
    [SerializeField] private TutorialBossAnimation tbAnimation = null;

    void Awake()
    {
        state = GetComponent<TutorialBossState>();
        attack = GetComponent<TutorialBossAttack>();
        effectGenerator = GetComponent<TutorialBossEffectGenerator>();
    }

    //生成時に生成側が呼ぶ
    public event System.Action OnBirthStart;
    public void BirthStart()
    {
        state.BirthStart();
        attack.BodyAttackSwitch(false);
        tbAnimation.BirthPlay();
        OnBirthStart?.Invoke();
    }

    //生成終了をアニメーションイベントが検知して呼ぶ。
    public event System.Action OnBirthEnd;
    public void BirthEnd()
    {
        state?.BirthEnd();
        attack.BodyAttackSwitch(true);
        OnBirthEnd?.Invoke();
    }

    //死亡を検知しダメージスクリプトが呼ぶ。
    public event System.Action OnDeathStart;
    public void DeathStart()
    {
        state.DeathStart();
        attack.BodyAttackSwitch(false);
        tbAnimation.DeathPlay();
        effectGenerator.BulletClear();
        effectGenerator.GenerateDeathEffect();
        OnDeathStart?.Invoke();
    }

    //死亡終了をアニメーションイベントが検知して呼ぶ。ここで自分を破壊。
    public event System.Action OnDeathEnd;
    public void DeathEnd()
    {
        Destroy(this.gameObject);
        OnDeathEnd?.Invoke();
    }
}