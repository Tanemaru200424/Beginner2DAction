using UnityEngine;

public class LowLevelEnemy1State : MonoBehaviour
{
    //基礎状態。
    private enum BaseState { NOMAL, DAMAGE, BIRTH, DEATH, HITTED }
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

    //歩行状態か否か
    public bool IsWalk() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer(); }
    //直立状態か否か
    public bool IsStand() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND && !iaimPlayer.IsExistPlayer(); }
    //吹き飛び状態か否か
    public bool IsHiited() { return currentBase == BaseState.HITTED; }

    //向き反転可能か。挙動管理スクリプトで使う。
    public bool CanTurn() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND; }
    //横移動状態について。挙動管理スクリプトで使う。
    //歩き
    public bool IsWalkXMove() { return currentBase == BaseState.NOMAL && currentSub == SubState.GROUND && iaimPlayer.IsExistPlayer(); }
    //横移動出来ない（床の影響は受ける。）
    public bool IsCantXMove() { return (currentBase == BaseState.NOMAL && (currentSub == SubState.FALL || !iaimPlayer.IsExistPlayer())) || currentBase == BaseState.DAMAGE; }
    //横移動無効（床の影響も無効。）
    public bool IsStopXMove() { return currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }
    //吹き飛び移動状態
    public bool IsHittedXMove() { return currentBase == BaseState.HITTED; }
    //縦移動状態について。挙動管理スクリプトで使う。
    //縦移動できない（床の影響は受ける。）
    public bool IsCantYMove() { return (currentBase == BaseState.NOMAL || currentBase == BaseState.DAMAGE) && currentSub == SubState.GROUND; }
    //落下
    public bool IsFallYMove() { return (currentBase == BaseState.NOMAL || currentBase == BaseState.DAMAGE) && currentSub == SubState.FALL; }
    //縦移動無効（床の影響も無効。）
    public bool IsStopYMove() { return currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }
    //吹き飛び移動状態
    public bool IsHittedYMove() { return currentBase == BaseState.HITTED; }
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

    //ダメージを受ける状態か。外部のダメージ同期スクリプトも使用する。
    public bool CanDamage()
    {
        return (currentBase == BaseState.NOMAL || currentBase == BaseState.DAMAGE) && !isPause;
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

    //吹き飛び可能な状態か
    public bool CanHitted()
    {
        return (currentBase == BaseState.NOMAL || currentBase == BaseState.DAMAGE) && !isPause;
    }
    //吹き飛び開始
    public void HittedStart() 
    {
        if (CanHitted()) { currentBase = BaseState.HITTED; }
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
