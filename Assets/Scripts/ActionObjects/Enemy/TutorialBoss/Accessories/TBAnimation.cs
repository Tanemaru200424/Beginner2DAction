using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBAnimation : MonoBehaviour
{
    private Animator animator = null;
    [SerializeField] private TBState state = null;
    [SerializeField] private GroundChecker groundChecker = null;
    private bool isGround = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("ground", isGround);
    }

    void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
    }

    //攻撃待機状態に遷移。攻撃スクリプトが呼ぶ。
    public void ShootChargeTrigger()
    {
        if (state.CanCharge()) { animator.SetTrigger("shootCharge"); }
    }
    public void TackleChargeTrigger()
    {
        if (state.CanCharge()) { animator.SetTrigger("tackleCharge"); }
    }
    //突進終了。攻撃スクリプトが呼ぶ。
    public void TackleEndTrigger() { animator.SetTrigger("tackleEnd"); }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    public void DamagePlay()
    {
        if (state.CanDamage()) { animator.Play("Damage"); }
    }

    //登場アニメーション再生。イベントスクリプトが使う。
    public void BirthPlay() { animator.Play("Birth"); }
    //死亡アニメーション再生。イベントスクリプトが使う。
    public void DeathPlay() { animator.Play("Death"); }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        animator.speed = ispause ? 0 : 1;
    }
}
