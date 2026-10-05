using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//中級の敵はギミックのような存在なのでエリア外死亡がない。
public class MiddleLevelEnemyDamage : MonoBehaviour, IDamageable, IHittable
{
    [SerializeField] private MiddleLevelEnemyState state = null;
    [SerializeField] private MiddleLevelEnemyEvents events = null;
    [SerializeField] private MiddleLevelEnemyAttack attack = null;
    [SerializeField] private Transform parentTrans = null;
    [SerializeField] private MiddleLevelEnemyMove move = null;
    [SerializeField] private MiddleLevelEnemyAnimation mleAnimation = null;
    [SerializeField] private int maxHp = 3;
    [SerializeField] private SpriteRenderer charactorSprite = null;
    [SerializeField] private AccessoriesLoopEffect damageEffect = null;
    private int nowHp = 0;
    private bool canHitted = false;
    [SerializeField] private float maxInvincibleTime = 0.3f;
    private float nowInvincibleTime = 0f;
    private bool isInvincible = false;//無敵
    private bool isPause = false;//一時停止

    void Awake()
    {
        nowHp = maxHp;
        damageEffect.Init();
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

    public bool CanDamage() { return state.CanDamage() && !isPause && nowHp > 0; }
    public void Damage(int value)
    {
        nowHp -= value;
        move.DamageFlip();
        if (nowHp > 0)
        {
            isInvincible = true;
            nowInvincibleTime = 0;
            state.DamageStart();
            mleAnimation.DamagePlay();
            attack.BodyAttackSwitch(false);
            damageEffect.EffectSwitch(true);
        }
        else
        {
            Dead();
        }
        canHitted = false;
    }
    public void FatalDamage() { Damage(nowHp); }
    public void Dead()
    {
        if (!isPause)
        {
            if (canHitted)
            {
                events.HittedStart();
            }
            else
            {
                events.DeathStart();
            }
        }
    }

    public bool CanHitted() { return !isPause; }
    public void Hitted(float zAngle)
    {
        if (!isPause)
        {
            canHitted = true;
            move.HittedStart(parentTrans.position, zAngle);
        }
        else
        {
            canHitted = false;
        }
    }

    //アニメーションイベントで使うエフェクト無効
    public void EffectOff() { damageEffect.EffectSwitch(false); }

    public void PauseSwitch(bool ispause)
    {
        isPause = ispause;
        isPause = ispause;
        damageEffect.PauseSwitch(ispause);
    }
}
