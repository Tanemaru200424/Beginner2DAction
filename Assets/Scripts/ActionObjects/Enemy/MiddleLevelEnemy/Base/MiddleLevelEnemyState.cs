using NUnit.Framework.Interfaces;
using UnityEngine;

public class MiddleLevelEnemyState : MonoBehaviour
{
    //基礎状態。
    private enum BaseState { NOMAL, ATTACK, DAMAGE, BIRTH, DEATH, HITTED }
    [SerializeField] private BaseState currentBase = BaseState.NOMAL;

    private IAimPlayer iaimPlayer = null;
    private bool isPause = false;

    void Awake()
    {
        currentBase = BaseState.NOMAL;
        iaimPlayer = GetComponent<IAimPlayer>();
        isPause = false;
    }

    //コントローラーで使う
    //吹き飛び状態か否か
    public bool IsHiited() { return currentBase == BaseState.HITTED; }

    //挙動管理スクリプトで使う。
    //向き反転可能か。
    public bool CanTurn() { return currentBase == BaseState.NOMAL && iaimPlayer.IsExistPlayer() ; }
    //横移動状態について。
    //横移動無効（床の影響も無効。）
    public bool IsStopXMove() { return currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK || currentBase == BaseState.DAMAGE || currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }
    //吹き飛び移動状態
    public bool IsHittedXMove() { return currentBase == BaseState.HITTED; }
    //縦移動状態について。
    //上下往復移動
    public bool IsFlightYMove() { return currentBase == BaseState.NOMAL; }
    //縦移動無効（床の影響も無効。）
    public bool IsStopYMove() { return currentBase == BaseState.ATTACK || currentBase == BaseState.DAMAGE || currentBase == BaseState.BIRTH || currentBase == BaseState.DEATH; }
    //吹き飛び移動状態
    public bool IsHittedYMove() { return currentBase == BaseState.HITTED; }

    //ダメージを受ける状態か。外部のダメージ同期スクリプトも使用する。
    public bool CanDamage()
    {
        return (currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK || currentBase == BaseState.DAMAGE) && !isPause;
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
    //被ダメージ時にプレイヤーと遠ざかるように動きたいので向き調整をする。
    public bool CanDamageFlip()
    {
        return (this.transform.position.x - iaimPlayer.GetPlayerPos().x) * this.transform.localScale.x > 0;
    }

    //攻撃スクリプトが呼び出す。
    public bool CanAttack() { return currentBase == BaseState.NOMAL && iaimPlayer.IsExistPlayer() && !isPause; }
    public void AttackStart()
    {
        if (CanAttack()) { currentBase = BaseState.ATTACK; }
    }
    //攻撃状態終了。アニメーションイベントが呼ぶ。
    public void AttackEnd()
    {
        if (currentBase == BaseState.ATTACK) { currentBase = BaseState.NOMAL; }
    }

    //吹き飛び可能な状態か
    public bool CanHitted()
    {
        return (currentBase == BaseState.NOMAL || currentBase == BaseState.ATTACK || currentBase == BaseState.DAMAGE) && !isPause;
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
        isPause = ispause;
    }
}
