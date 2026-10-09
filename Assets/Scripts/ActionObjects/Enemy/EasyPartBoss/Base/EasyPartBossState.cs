using UnityEngine;

public class EasyPartBossState : MonoBehaviour
{
    //基礎状態。
    private enum BaseState { NOMAL, JUMPCHARGE, SHOOTCHARGE, SHOOT, TACKLECHARGE, TACKLE, TACKLESTUN, BIRTH, DEATH }
    [SerializeField] private BaseState currentBase = BaseState.NOMAL;

    //サブ状態。NOMALとATTACK時に参照。
    private enum SubState { GROUND, JUMP, FALL }
    [SerializeField] private SubState currentSub = SubState.GROUND;

    [SerializeField] private GroundChecker groundChecker = null;
    private IAimPlayer iaimPlayer = null;
    private bool isGround = false;
    private bool isFallStart = false;
    [SerializeField] private HeadGroundChecker headGroundChecker = null;
    private bool isHeadGround = false; //頭が天井に当たっているか
    private bool isLowMax = true; //ジャンプ高度が最大より低いか
    private bool isPause = false;

    void Awake()
    {
        currentBase = BaseState.NOMAL;
        currentSub = SubState.GROUND;
        iaimPlayer = GetComponent<IAimPlayer>();
        isGround = false;
        isFallStart = false;
        isHeadGround = false;
        isLowMax = true;
        isPause = false;
    }

    void Update()
    {

        if ((currentBase == BaseState.NOMAL || currentBase == BaseState.JUMPCHARGE || currentBase == BaseState.TACKLECHARGE || currentBase == BaseState.TACKLE || currentBase == BaseState.TACKLESTUN) &&
            currentSub == SubState.GROUND && !isGround)
        {
            if (currentBase != BaseState.NOMAL) { currentBase = BaseState.NOMAL; }
            currentSub = SubState.FALL;
            isFallStart = true;
        }
        else if (currentBase == BaseState.NOMAL && currentSub == SubState.JUMP && (isHeadGround || !isLowMax))
        {
            currentSub = SubState.FALL;
            isFallStart = true;
        }
        else if (currentBase == BaseState.NOMAL && currentSub == SubState.FALL && isGround) { currentSub = SubState.GROUND; }
    }

    private void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
        isHeadGround = headGroundChecker.IsHeadGround();
    }

    //コントローラーが使う。
    //待機時間を減らす状態。
    public bool CanCountCoolTime() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer() && !isPause; }

    //アニメーション制御が呼ぶ。ジャンプ状態か。
    public bool IsJump() { return currentBase == BaseState.NOMAL && currentSub == SubState.JUMP; }

    //向き反転可能か。挙動管理スクリプトで使う。
    public bool CanTurn() { return ((currentSub == SubState.GROUND && currentBase == BaseState.NOMAL) || currentBase == BaseState.TACKLECHARGE || currentBase == BaseState.SHOOTCHARGE || currentBase == BaseState.SHOOT) && !isPause; }
    //横移動状態について。挙動管理スクリプトで使う。
    //突進の横移動
    public bool IsTackleXMove() { return currentBase == BaseState.TACKLE; }
    //空中の横移動
    public bool IsAirXMove() { return currentBase == BaseState.NOMAL && (currentSub == SubState.JUMP || currentSub == SubState.FALL); }
    //横移動出来ない（床の影響は受ける。）
    public bool IsCantXMove() { return (currentBase == BaseState.NOMAL && currentSub == SubState.GROUND) || currentBase == BaseState.JUMPCHARGE || currentBase == BaseState.SHOOTCHARGE || currentBase == BaseState.SHOOT || currentBase == BaseState.TACKLECHARGE || currentBase == BaseState.TACKLESTUN; }
    //横移動無効（床の影響も無効。）
    public bool IsStopXMove() { return currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }
    //縦移動状態について。挙動管理スクリプトで使う。
    //ジャンプ
    public bool IsJumpYMove() { return currentBase == BaseState.NOMAL && currentSub == SubState.JUMP; }
    //落下
    public bool IsFallYMove() { return currentBase == BaseState.NOMAL && currentSub == SubState.FALL; }
    //縦移動できない（床の影響は受ける。）
    public bool IsCantYMove() { return ((currentBase == BaseState.NOMAL || currentBase == BaseState.JUMPCHARGE || currentBase == BaseState.TACKLE || currentBase == BaseState.TACKLESTUN) && currentSub == SubState.GROUND) || currentBase == BaseState.SHOOTCHARGE || currentBase == BaseState.SHOOT; }
    //縦移動無効（床の影響も無効。）
    public bool IsStopYMove() { return currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }
    public void UpdateLowMax(bool isUpdateLowMax) { isLowMax = isUpdateLowMax; }
    //落下地点更新用。挙動管理スクリプトで使う。移動スクリプトが落下検知するまでフラグは下がらない。
    public bool IsFallStart()
    {
        if (isFallStart)
        {
            isFallStart = false;
            return true;
        }
        return false;
    }
    //ジャンプへの行動遷移可能な状態か。移動スクリプトが呼ぶ。
    public bool CanJumpCharge() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer() && !isPause; }
    public void JumpChargeStart()
    {
        if (CanJumpCharge()) { currentBase = BaseState.JUMPCHARGE; }
    }
    //直立状態を一度挟んだら空中X軸速度を0にしなおす。
    public bool IsStand() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND; }

    //攻撃遷移系。攻撃スクリプトが呼ぶ。
    //このボスは射撃を除きアニメーション依存ではなく時間依存で攻撃状態を遷移させる。
    public bool CanTackleChargeStart() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer() && !isPause; }
    public bool CanTackleStart() { return currentBase == BaseState.TACKLECHARGE && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer() && !isPause; }
    public bool CanTackleStunStart() { return currentBase == BaseState.TACKLE && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer() && !isPause; }
    public bool CanTackleStunEnd() { return currentBase == BaseState.TACKLESTUN && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer() && !isPause; }
    public bool CanShootChargeStart() { return currentBase == BaseState.NOMAL && currentSub == SubState.JUMP && iaimPlayer.IsExistPlayer() && !isPause; }
    public bool CanShootStart() { return currentBase == BaseState.SHOOTCHARGE && currentSub == SubState.FALL && iaimPlayer.IsExistPlayer() && !isPause; }
    public bool CanShootEnd() { return currentBase == BaseState.SHOOT && currentSub == SubState.FALL && iaimPlayer.IsExistPlayer() && !isPause; }
    public void TackleChargeStart()
    {
        if (CanTackleChargeStart()) { currentBase = BaseState.TACKLECHARGE; }
    }
    public void TackleStart()
    {
        if (CanTackleStart()) { currentBase = BaseState.TACKLE; }
    }
    public void TackleStunStart()
    {
        if (CanTackleStunStart()) { currentBase = BaseState.TACKLESTUN; }
    }
    public void TackleStunEnd()
    {
        if (CanTackleStunEnd()) { currentBase = BaseState.NOMAL; }
    }
    public void ShootChargeStart()
    {
        if (CanShootChargeStart()) { currentBase = BaseState.SHOOTCHARGE; }
    }
    public void ShootStart()
    {
        if (CanShootStart())
        {
            currentBase = BaseState.SHOOT;
            currentSub = SubState.FALL;
        }
    }
    public void ShootEnd()
    {
        if (CanShootEnd()) { currentBase = BaseState.NOMAL; }
    }
    //攻撃スクリプトがカウントを進める、現攻撃状態を観測するために使う。
    //突進待機状態か。
    public bool IsTackleCharge() { return currentBase == BaseState.TACKLECHARGE && currentSub == SubState.GROUND; }
    //突進状態か。
    public bool IsTackle() { return currentBase == BaseState.TACKLE && currentSub == SubState.GROUND; }
    //突進スタン状態か。
    public bool IsTackleStun() { return currentBase == BaseState.TACKLESTUN && currentSub == SubState.GROUND; }
    //射撃待機状態か。
    public bool IsShootCharge() { return currentBase == BaseState.SHOOTCHARGE && currentSub == SubState.FALL; }
    //射撃状態か。
    public bool IsShoot() { return currentBase == BaseState.SHOOT && currentSub == SubState.FALL; }

    //アニメーションイベントが使うジャンプ開始。
    //ジャンプ可能か。
    private bool CanJump() { return currentBase == BaseState.JUMPCHARGE && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer() && !isPause; }
    public void JumpStart()
    {
        if (CanJump()) 
        { 
            currentBase = BaseState.NOMAL;
            currentSub = SubState.JUMP;
        }
    }

    //ダメージを受ける状態か。外部のダメージ同期スクリプトも使用する。
    public bool CanDamage()
    {
        return (currentBase == BaseState.NOMAL || currentBase == BaseState.JUMPCHARGE || currentBase == BaseState.SHOOTCHARGE || currentBase == BaseState.SHOOT || currentBase == BaseState.TACKLECHARGE || currentBase == BaseState.TACKLE || currentBase == BaseState.TACKLESTUN) && !isPause;
    }
    //吹き飛ばしによる反転を受けるか
    public bool CanTurnByHitted() { return currentBase == BaseState.TACKLE; }

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
