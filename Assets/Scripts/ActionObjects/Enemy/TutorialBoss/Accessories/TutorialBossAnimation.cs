using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossAnimation : MonoBehaviour
{
    private Animator animator = null;
    private static readonly int damageStateHash = Animator.StringToHash("Damage");
    private static readonly int birthStateHash = Animator.StringToHash("Birth");
    private static readonly int deathStateHash = Animator.StringToHash("Death");
    private static readonly int shootChargeParameterHash = Animator.StringToHash("shootCharge");
    private static readonly int tackleChargeParameterHash = Animator.StringToHash("tackleCharge");
    private static readonly int tackleEndParameterHash = Animator.StringToHash("tackleEnd");
    private static readonly int groundParameterHash = Animator.StringToHash("ground");
    [SerializeField] private GroundChecker groundChecker = null;
    private bool isGround = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(groundParameterHash, isGround);
    }

    void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
    }

    //攻撃待機状態に遷移。攻撃スクリプトが呼ぶ。
    public void ShootChargeTrigger(){ animator.SetTrigger(shootChargeParameterHash); }
    public void TackleChargeTrigger(){ animator.SetTrigger(tackleChargeParameterHash); }
    //突進終了。攻撃スクリプトが呼ぶ。
    public void TackleEndTrigger() { animator.SetTrigger(tackleEndParameterHash); }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    public void DamagePlay(){ animator.Play(damageStateHash); }
    //登場アニメーション再生。イベントスクリプトが使う。
    public void BirthPlay() { animator.Play(birthStateHash); }
    //死亡アニメーション再生。イベントスクリプトが使う。
    public void DeathPlay() { animator.Play(deathStateHash); }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        animator.speed = ispause ? 0 : 1;
    }
}
