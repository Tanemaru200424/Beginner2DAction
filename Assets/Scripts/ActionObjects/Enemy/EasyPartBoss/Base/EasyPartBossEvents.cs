using UnityEngine;

public class EasyPartBossEvents : MonoBehaviour, ICharactorEvents
{
    private EasyPartBossState state = null;
    private EasyPartBossAttack attack = null;
    private EasyPartBossEffectGenerator effectGenerator = null;
    [SerializeField] private EasyPartBossAnimation epbAnimation = null;

    void Awake()
    {
        state = GetComponent<EasyPartBossState>();
        attack = GetComponent<EasyPartBossAttack>();
        effectGenerator = GetComponent<EasyPartBossEffectGenerator>();
    }

    //生成時に生成側が呼ぶ
    public event System.Action OnBirthStart;
    public void BirthStart()
    {
        state.BirthStart();
        attack.BodyAttackSwitchOff();
        epbAnimation.BirthPlay();
        OnBirthStart?.Invoke();
    }

    //生成終了をアニメーションイベントが検知して呼ぶ。
    public event System.Action OnBirthEnd;
    public void BirthEnd()
    {
        state?.BirthEnd();
        attack.BodyAttackSwitchNormal();
        OnBirthEnd?.Invoke();
    }

    //死亡を検知しダメージスクリプトが呼ぶ。
    public event System.Action OnDeathStart;
    public void DeathStart()
    {
        state.DeathStart();
        attack.BodyAttackSwitchOff();
        epbAnimation.DeathPlay();
        effectGenerator.AttackClear();
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

    public void AreaOverExtinction()
    {
        state.DeathStart();
        attack.BodyAttackSwitchOff();
        effectGenerator.AttackClear();
        OnDeathStart?.Invoke();
        OnDeathEnd?.Invoke();
        Destroy(this.gameObject);
    }
}