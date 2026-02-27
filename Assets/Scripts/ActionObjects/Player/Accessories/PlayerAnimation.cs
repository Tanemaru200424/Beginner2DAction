using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator = null;
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
        animator.SetBool("jump", state.IsJump());
        animator.SetBool("walk", inputX != 0);
        animator.SetBool("ground", isGround);
        animator.SetFloat("airattack", !isGround ? 1 : 0);
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
        if (state.CanAttack()) { animator.SetTrigger("attack"); }
    }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    public void DamagePlay()
    {
        if (state.CanDamage()) { animator.Play("Damage"); }
    }

    //登場アニメーション再生。プレイヤーイベントスクリプトが使う。
    public void BirthPlay() { animator.Play("Birth"); }
    //死亡アニメーション再生。プレイヤーイベントスクリプトが使う。
    public void DeathPlay() { animator.Play("Death"); }

    //棒立ち状態にする。
    public void SetStand() { animator.Play("Stand"); }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause) 
    { 
        this.enabled  = !ispause; 
        animator.speed = ispause ? 0 : 1;
    }
}
