using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//状態変化を管理。各プレイヤー制御スクリプトがこれを参照して処理する。
public class PlayerState : MonoBehaviour
{
    //基礎状態。
    private enum BaseState { NOMAL, ATTACK, DAMAGE, BIRTH, DEATH } 
    [SerializeField] private BaseState currentBase = BaseState.NOMAL;

    //サブ状態。NOMALとATTACK時に参照。
    private enum SubState { GROUND, JUMP, FALL }
    [SerializeField] private SubState currentSub = SubState.GROUND;

    //イベント中無敵にしたいので追加で実装した状態。
    private bool isIncredible = false;

    [SerializeField] private GroundChecker groundChecker = null;
    private bool isGround = false;
    [SerializeField] private HeadGroundChecker headGroundChecker = null;
    private bool isHeadGround = false;
    private bool isFallStart = false;
    private bool isInputJump = false; //ジャンプ入力中か
    private bool isLowMax = true; //ジャンプ高度が最大より低いか
    private bool isPause = false;

    void Awake()
    {
        currentBase = BaseState.NOMAL;
        currentSub = SubState.GROUND;
        isIncredible = false;
        isGround = false; 
        isHeadGround = false;
        isFallStart = false;
        isInputJump = false;
        isLowMax = true;
        isPause = false;
    }

    void Update()
    {
        if (currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK)
        {
            if (currentSub == SubState.GROUND && !isGround)
            {
                currentSub = SubState.FALL;
                isFallStart = true;
            }
            else if (currentSub == SubState.JUMP && (isHeadGround || !isInputJump || !isLowMax))
            {
                currentSub = SubState.FALL;
                isFallStart = true;
            }
            else if (currentSub == SubState.FALL && isGround) { currentSub = SubState.GROUND; }
        }
    }

    private void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
        isHeadGround = headGroundChecker.IsHeadGround();
    }

    //向き反転可能か。挙動管理スクリプトで使う。
    public bool CanTurn() { return currentBase == BaseState.NOMAL && !isPause; }

    //横移動状態について。挙動管理スクリプトで使う。
    //通常（入力通りに動く）
    public bool IsNomalXMove() { return currentBase == BaseState.NOMAL || (currentBase == BaseState.ATTACK && currentSub != SubState.GROUND); }
    //ダメージノックバック
    public bool IsDamageXMove() { return currentBase == BaseState.DAMAGE; }
    //横移動出来ない（床の影響は受ける。）
    public bool IsCantXMove() { return currentBase == BaseState.ATTACK && currentSub == SubState.GROUND; }
    //横移動無効（床の影響も無効。）
    public bool IsStopXMove() { return currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }

    //縦移動状態について。挙動管理スクリプトで使う。
    //通常
    public bool IsNomalYMove() { return ((currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK) && currentSub == SubState.GROUND) || currentBase == BaseState.DAMAGE; }
    //ジャンプ
    public bool IsJumpYMove() { return (currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK) && currentSub == SubState.JUMP; }
    //落下
    public bool IsFallYMove() { return (currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK) && currentSub == SubState.FALL; }
    //縦移動無効（床の影響も無効。）
    public bool IsStopYMove() { return currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }

    //ジャンプ可能か。外部のジャンプ同期スクリプトも使用する。
    public bool CanJump()
    {
        return !isInputJump && !isPause &&
               (currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK) && currentSub == SubState.GROUND;
    }
    //ジャンプ開始時呼び出し。入力管理スクリプトが使う。
    public void JumpStart()
    {
        if (CanJump())
        {
            currentSub = SubState.JUMP;
            isFallStart = false;
        }
    }
    //ジャンプ入力中断。入力管理スクリプトが使う。
    public void HoldJump(bool isInput) { isInputJump = isInput; }
    //ジャンプ中高度を守っているか更新。挙動管理スクリプトで使う。
    public void UpdateLowMax(bool isUpdateLowMax){ isLowMax = isUpdateLowMax; }
    //ジャンプ状態か。アニメーションスクリプトが使う。
    public bool IsJump() { return (currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK) && currentSub == SubState.JUMP; }
    //落下地点更新用。挙動管理スクリプトで使う。
    public bool IsFallStart() 
    {
        if (isFallStart) 
        { 
            isFallStart = false;
            return true;
        }
        return false; 
    }

    //攻撃可能状態か。外部の攻撃同期スクリプトも使用する。
    public bool CanAttack() { return currentBase == BaseState.NOMAL && !isPause; }
    //攻撃遷移。入力管理スクリプトが使う。
    public void AttackStart()
    {
        if (CanAttack()) { currentBase = BaseState.ATTACK; }
    }
    //攻撃終わり。攻撃アニメーション終了検知スクリプトかダメージスクリプトが呼ぶ。
    public void AttackEnd()
    {
        if (currentBase == BaseState.ATTACK) { currentBase = BaseState.NOMAL; }
    }

    //強制無敵状態か。
    public bool IsIncredible() { return isIncredible; }
    //ダメージを受ける状態か。外部のダメージ同期スクリプトも使用する。
    public bool CanDamage() { return !isIncredible && (currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK) && !isPause; }
    //ダメージ開始。ダメージ処理スクリプトが呼び出す。
    public void DamageStart()
    {
        if (CanDamage()) 
        { 
            currentBase = BaseState.DAMAGE;
            currentSub = SubState.GROUND;
        }
    }
    //ダメージ終了。アニメーション終了検知スクリプトが呼び出す。
    public void DamageEnd()
    {
        if (currentBase == BaseState.DAMAGE) { currentBase = BaseState.NOMAL; }
    }

    //登場開始と終了。イベント制御スクリプトが使う。
    public void BirthStart() { currentBase = BaseState.BIRTH; }
    public void BirthEnd() {  currentBase = BaseState.NOMAL; }

    //死亡開始。イベント制御スクリプトが使う。死亡後は破壊されるので終了は無し。
    public void DeathStart() { currentBase = BaseState.DEATH; }

    //イベント中などに外部から無敵にしてもらう。
    public void IncredibleSwitch(bool isIncredible) { this.isIncredible = isIncredible; }

    //通常状態にする。
    public void Nomalization()
    {
        currentBase = BaseState.NOMAL;
        currentSub = SubState.GROUND;
    }

    //一時停止オンオフ処理。
    public void PauseSwitch(bool ispause) 
    { 
        this.enabled = !ispause;
        isPause = ispause;
    }
}
