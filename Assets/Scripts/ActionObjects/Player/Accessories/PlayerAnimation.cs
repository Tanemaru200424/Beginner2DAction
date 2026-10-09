using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator = null;
    private static readonly int damageStateHash = Animator.StringToHash("Damage");
    private static readonly int birthStateHash = Animator.StringToHash("Birth");
    private static readonly int deathStateHash = Animator.StringToHash("Death");
    private static readonly int standStateHash = Animator.StringToHash("Stand");
    private static readonly int attackParameterHash = Animator.StringToHash("attack");
    private static readonly int jumpParameterHash = Animator.StringToHash("jump");
    private static readonly int walkParameterHash = Animator.StringToHash("walk");
    private static readonly int groundParameterHash = Animator.StringToHash("ground");
    private static readonly int airAttackParameterHash = Animator.StringToHash("airAttack");
    [SerializeField] private PlayerState state = null; //プレイヤー状態管理スクリプト。
    private float inputX = 0;
    [SerializeField] private GroundChecker groundChecker = null;
    private bool isGround = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(jumpParameterHash, state.IsJump());
        animator.SetBool(walkParameterHash, inputX != 0);
        animator.SetBool(groundParameterHash, isGround);
        animator.SetFloat(airAttackParameterHash, !isGround ? 1 : 0);
    }

    void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
    }

    //歩き状態に遷移。入力管理スクリプトが使う。
    public void SetInputX(float x) { inputX = x; }
    //攻撃状態に遷移。入力管理スクリプトが使う。
    public void AttackTrigger()
    {
        if (state.CanAttack()) { animator.SetTrigger(attackParameterHash); }
    }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    public void DamagePlay()
    {
        if (state.CanDamage()) { animator.Play(damageStateHash); }
    }

    //登場アニメーション再生。プレイヤーイベントスクリプトが使う。
    public void BirthPlay() { animator.Play(birthStateHash); }
    //死亡アニメーション再生。プレイヤーイベントスクリプトが使う。
    public void DeathPlay() { animator.Play(deathStateHash); }

    //棒立ち状態にする。
    public void SetStand() { animator.Play(standStateHash); }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause) 
    { 
        this.enabled  = !ispause; 
        animator.speed = ispause ? 0 : 1;
    }
}
