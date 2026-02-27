using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBDamage : MonoBehaviour, IDamageable, IHittable
{
    [SerializeField] private TBState state = null;
    [SerializeField] private TBAnimation tbAnimation = null;
    [SerializeField] private TBEvents events = null;
    [SerializeField] private TBAttack attack = null;
    [SerializeField] private TBEffectGenerator effectGenerator = null;
    [SerializeField] private BossDataForUI dataForUI = null;
    [SerializeField] private int maxHp = 30;
    [SerializeField] private SpriteRenderer charactorSprite = null;
    [SerializeField] private AccessoriesLoopEffect damageEffect = null;
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
        damageEffect.Init();
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
            attack.BodyAttackSwitch(false);
            events.DeathStart();
            effectGenerator.GenerateDeathEffect();
            damageEffect.EffectSwitch(false);
        }
    }

    public bool CanHitted() { return !isInvincible && state.CanKnockBack() && !isPause; }
    public void Hitted(float zAngle)
    {
        tbAnimation.DamagePlay();
        state.DamageStart();
        attack.BodyAttackSwitch(false);
        damageEffect.EffectSwitch(true);
        effectGenerator.GenerateDownEffect();
    }

    //アニメーションイベントで使うエフェクト無効
    public void EffectOff() { damageEffect.EffectSwitch(false); }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        isPause = ispause;
        damageEffect.PauseSwitch(ispause);
    }
}