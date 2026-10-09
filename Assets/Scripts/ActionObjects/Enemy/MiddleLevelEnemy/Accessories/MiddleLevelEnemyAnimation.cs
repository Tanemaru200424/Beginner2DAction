using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLevelEnemyAnimation : MonoBehaviour
{
    private Animator baseAnimator = null;
    [SerializeField] private Animator wingAnimator = null;
    private static readonly int damageStateHash = Animator.StringToHash("Damage");
    private static readonly int hittedStateHash = Animator.StringToHash("Hitted");
    private static readonly int deathStateHash = Animator.StringToHash("Death");
    private static readonly int attackParameterHash = Animator.StringToHash("attack");
    private static readonly int nonActiveParameterHash = Animator.StringToHash("nonActive");

    void Awake()
    {
        baseAnimator = GetComponent<Animator>();
    }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    public void DamagePlay() { baseAnimator.Play(damageStateHash); }

    //攻撃スクリプトから呼ぶ。射撃。
    public void AttackTrigger(){ baseAnimator.SetTrigger(attackParameterHash); }

    //死亡アニメーション再生。イベントスクリプトが使う。
    public void DeathPlay() 
    { 
        baseAnimator.Play(deathStateHash);
        wingAnimator.SetTrigger(nonActiveParameterHash);
    }
    //吹き飛びアニメーション再生。イベントスクリプトが使う。
    public void HittedPlay() 
    {
        baseAnimator.Play(hittedStateHash);
        wingAnimator.SetTrigger(nonActiveParameterHash);
    }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        baseAnimator.speed = ispause ? 0 : 1;
        wingAnimator.speed = ispause ? 0 : 1;
    }
}
