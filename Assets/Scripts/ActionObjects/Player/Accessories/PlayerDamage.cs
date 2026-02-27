using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerState state = null;
    [SerializeField] private PlayerAnimation pAnimation = null;
    [SerializeField] private PlayerDataForUI dataForUI = null;
    [SerializeField] private PlayerEvents events = null;
    [SerializeField] private int maxHp = 30;
    [SerializeField] private SpriteRenderer charactorSprite = null;
    [SerializeField] private AccessoriesLoopEffect damageEffect = null;
    private int nowHp = 0;
    [SerializeField] private float maxInvincibleTime = 1.0f;
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
            pAnimation.DamagePlay();
            state.DamageStart();
            nowInvincibleTime = 0;
            damageEffect.EffectSwitch(true);
        }
        else
        {
            Dead();
        }
    }
    public void FatalDamage()
    {
        if (!state.IsIncredible()) { Damage(nowHp); }
    }
    public void Dead()
    {
        if (!isPause) { events.DeathStart(); }
    }

    //アニメーションイベントで使うエフェクト無効
    public void EffectOff() { damageEffect.EffectSwitch(false); }

    //無敵状態解除
    public void IncredibleOff()
    {
        nowInvincibleTime = 0;
        isInvincible = false;
        charactorSprite.color = new Color(255, 255, 255, 1);
    }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        isPause = ispause;
        damageEffect.PauseSwitch(ispause);
    }
}