using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLevelEnemyAnimation : MonoBehaviour
{
    private Animator baseAnimator = null;
    [SerializeField] private Animator wingAnimator = null;

    void Awake()
    {
        baseAnimator = GetComponent<Animator>();
    }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    public void DamagePlay() { baseAnimator.Play("Damage"); }

    //攻撃スクリプトから呼ぶ。射撃。
    public void AttackTrigger(){ baseAnimator.SetTrigger("attack"); }

    //死亡アニメーション再生。イベントスクリプトが使う。
    public void DeathPlay() 
    { 
        baseAnimator.Play("Death");
        wingAnimator.SetTrigger("nonActive");
    }
    //吹き飛びアニメーション再生。イベントスクリプトが使う。
    public void HittedPlay() 
    {
        baseAnimator.Play("Hitted");
        wingAnimator.SetTrigger("nonActive");
    }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        baseAnimator.speed = ispause ? 0 : 1;
        wingAnimator.speed = ispause ? 0 : 1;
    }
}
