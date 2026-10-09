using UnityEngine;

//簡単ステージのボスは突進中にHittedされると反転する。
public class EasyPartBossDamage : MonoBehaviour, IDamageable, IHittable, IAreaOverExtinctionable
{
    [SerializeField] private EasyPartBossState state = null;
    [SerializeField] private EasyPartBossMove move = null;
    //[SerializeField] private TutorialBossEvents events = null;
    [SerializeField] private EasyPartBossAttack attack = null;
    //[SerializeField] private TutorialBossEffectGenerator effectGenerator = null;
    [SerializeField] private BossDataForUI dataForUI = null;
    [SerializeField] private int maxHp = 30;
    [SerializeField] private SpriteRenderer charactorSprite = null;
    private int nowHp = 0;
    [SerializeField] private float maxInvincibleTime = 0.5f;
    private float nowInvincibleTime = 0f;
    private bool isInvincible = false;//無敵
    private bool isPause = false;//一時停止

    void Awake()
    {
        nowHp = maxHp;
        nowInvincibleTime = 0;
        isInvincible = false;
    }

    void Start()
    {
        dataForUI.HpChanged((float)nowHp / (float)maxHp);
    }

    void Update()
    {
        if (nowHp > 0 && isInvincible && nowInvincibleTime < maxInvincibleTime)
        {
            nowInvincibleTime += Time.deltaTime;
            float alpha = 0.1f + 0.5f * (nowInvincibleTime / maxInvincibleTime);
            alpha = Mathf.Clamp(alpha, 0.1f, 1f);
            charactorSprite.color = new Color(255, 255, 255, 0.3f);
            if (nowInvincibleTime >= maxInvincibleTime)
            {
                charactorSprite.color = new Color(255, 255, 255, 1);
                isInvincible = false;
            }
        }
    }

    public bool CanDamage() { return !isInvincible && state.CanDamage() && !isPause; }
    public void Damage(int value)
    {
        nowHp -= value;
        dataForUI.HpChanged((float)nowHp / (float)maxHp);
        if (nowHp > 0)
        {
            isInvincible = true;
            nowInvincibleTime = 0;
        }
        else
        {
            Dead();
        }
    }
    public void FatalDamage() { Damage(nowHp); }
    public void Dead()
    {
        if (!isPause)
        {
            attack.BodyAttackSwitchOff();
            //events.DeathStart();
            //effectGenerator.GenerateDeathEffect();
        }
    }

    //吹き飛ばしはない。突進中にHittedされると反転する。
    public bool CanHitted() { return !isInvincible && state.CanTurnByHitted() && !isPause; }
    public void Hitted(float zAngle)
    {
        move.TacckleTurnByHitted(zAngle);
        //effectGenerator.GenerateDownEffect();
    }

    public bool CanExtinction() { return state.CanDamage() && !isPause; }
    public void Extinction()
    {
        //events.AreaOverExtinction();
    }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        isPause = ispause;
    }
}
