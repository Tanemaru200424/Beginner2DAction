using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBState : MonoBehaviour
{
    //基礎状態。
    private enum BaseState { NOMAL, CHARGE, SHOOT, TACKLE, DAMAGE, BIRTH, DEATH }
    [SerializeField] private BaseState currentBase = BaseState.NOMAL;

    //サブ状態。NOMALとATTACK時に参照。
    private enum SubState { GROUND, FALL }
    [SerializeField] private SubState currentSub = SubState.GROUND;

    [SerializeField] private GroundChecker groundChecker = null;
    private IAimPlayer iaimPlayer = null;
    private bool isGround = false;
    private bool isFallStart = false;
    private bool isPause = false;

    void Awake()
    {
        currentBase = BaseState.NOMAL;
        currentSub = SubState.GROUND;
        iaimPlayer = GetComponent<IAimPlayer>();
        isGround = false;
        isFallStart = false;
        isPause = false;
    }

    void Update()
    {
        if (currentBase == BaseState.NOMAL)
        {
            if (currentSub == SubState.GROUND && !isGround)
            {
                currentSub = SubState.FALL;
                isFallStart = true;
            }
            else if (currentSub == SubState.FALL && isGround) 
            { 
                currentSub = SubState.GROUND; 
            }
        }
    }

    private void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
    }

    //コントローラーが使う。
    //待機時間を減らす状態。
    public bool CanCountCoolTime() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND; }
    //突進状態か。
    public bool IsTackle() { return currentBase == BaseState.TACKLE; }

    //向き反転可能か。挙動管理スクリプトで使う。
    public bool CanTurn() { return currentBase == BaseState.CHARGE && !isPause; }
    //横移動状態について。挙動管理スクリプトで使う。
    //通常（入力通りに動く）
    public bool IsTackleXMove() { return currentBase == BaseState.TACKLE; }
    //横移動出来ない（床の影響は受ける。）
    public bool IsCantXMove() { return currentBase == BaseState.NOMAL || currentBase == BaseState.CHARGE || currentBase == BaseState.SHOOT || currentBase == BaseState.DAMAGE; }
    //横移動無効（床の影響も無効。）
    public bool IsStopXMove() { return currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }
    //縦移動状態について。挙動管理スクリプトで使う。
    //通常
    public bool IsNomalYMove() 
    { 
        return 
            ((currentBase == BaseState.NOMAL || currentBase == BaseState.DAMAGE) && currentSub == SubState.GROUND) || 
            currentBase == BaseState.CHARGE || currentBase == BaseState.SHOOT || currentBase == BaseState.TACKLE; 
    }
    //落下
    public bool IsFallYMove() { return (currentBase == BaseState.NOMAL || currentBase == BaseState.DAMAGE) && currentSub == SubState.FALL; }
    //縦移動無効（床の影響も無効。）
    public bool IsStopYMove() { return currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }
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

    //攻撃待機可能な状態か。攻撃スクリプトが呼ぶ。
    public bool CanCharge() { return currentBase == BaseState.NOMAL && iaimPlayer.IsExistPlayer() && !isPause; }
    //攻撃待機開始。攻撃スクリプトが呼ぶ。
    public void ChargeStart()
    {
        if (CanCharge()) { currentBase = BaseState.CHARGE; }
    }

    //攻撃可能か。
    private bool CanAttack() { return currentBase == BaseState.CHARGE && iaimPlayer.IsExistPlayer() && !isPause; }
    //攻撃待機終了。アニメーションイベントが呼ぶ。
    public void TackleChargeEnd()
    {
        if (CanAttack()) { currentBase = BaseState.TACKLE; }
    }
    public void ShootChargeEnd()
    {
        if (CanAttack()) { currentBase = BaseState.SHOOT; }
    }
    //攻撃状態終了。アニメーションイベントが呼ぶ。
    public void AttackEnd()
    {
        if (currentBase == BaseState.SHOOT || currentBase == BaseState.TACKLE) { currentBase = BaseState.NOMAL; }
    }

    //ダメージを受ける状態か。外部のダメージ同期スクリプトも使用する。
    public bool CanDamage() 
    { 
        return (currentBase == BaseState.NOMAL || currentBase == BaseState.CHARGE || currentBase == BaseState.SHOOT || currentBase == BaseState.TACKLE || currentBase == BaseState.DAMAGE) && !isPause; 
    }
    //ノックバック可能な状態か。ダメージスクリプトが呼ぶ。
    public bool CanKnockBack()
    {
        return (currentBase == BaseState.TACKLE && !isPause);
    }
    //ダメージ開始。ダメージスクリプトが呼び出す。
    public void DamageStart()
    {
        if (CanDamage()) { currentBase = BaseState.DAMAGE; }
    }
    //ダメージ終了。アニメーションイベントが呼び出す。
    public void DamageEnd()
    {
        if (currentBase == BaseState.DAMAGE) { currentBase = BaseState.NOMAL; }
    }

    //登場開始と終了。イベント制御スクリプトが使う。
    public void BirthStart() { currentBase = BaseState.BIRTH; }
    public void BirthEnd() { currentBase = BaseState.NOMAL; }
    //死亡開始。イベント制御スクリプトが使う。死亡後は破壊されるので終了は無し。
    public void DeathStart() { currentBase = BaseState.DEATH; }

    //一時停止オンオフ処理。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        isPause = ispause;
    }
}
