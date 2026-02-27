using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アクションに関わるインターフェース系をまとめたもの

//オブジェクト一括管理用コンテナ。
public interface IObjectContainer
{
    public void RegistObject(GameObject obj);
    public void RemoveObject(GameObject obj);
}
//攻撃時にオブジェクト生成するなど自分以外のオブジェクトを作り出すオブジェクトに付ける。
public interface IGenerator
{
    public void SetObjectContainer(IObjectContainer iobjectContainer);//生成オブジェクト登録先のコンテナ。
    public GameObject Generate(GameObject generateObject, Vector3 generatePos, Vector3 generateScale, float zAngle);//登録されたコンテナにゲームオブジェクトと生成時の設定を反映し生成。
                                                                                                           //生成対象、生成位置、生成時のスケール、z軸角度
                                                                                                           //ゲームオブジェクトを返すので最初の登録も可能。
    public void InitRegist(IObjectContainer iobjectContainer, GameObject generateObject); //生成したオブジェクトが有効なら最初に登録。更にここで生成オブジェクトで登録、解除イベントを登録。
}
//コンテナに入っているオブジェクト。生成時にコンテナへの登録、解除イベントを登録
public interface IContainedObject
{
    public event Action OnRegist; //登録イベント。コンテナのリストに自分を格納する。
                                         //有効化された時等の自分で観測できるタイミングで自分で呼び出す。
                                         //生成者に呼び出してもらう。
    public event Action OnRemove; //解除イベント。コンテナのリストから自分を削除する。自分が無効化、
                                         //破壊された時に呼び出す。
}
//アクションシーンで管理するオブジェクトを分類するためのラベルとそれを持つオブジェクトに付ける読み取り用インターフェース。
public enum ActionObjectLabel
{
    PLAYER,
    ENEMY,
    ATTACK,
    GIMMICK
}
public interface IActionObjectLabel
{
    public ActionObjectLabel GetLabel();
}

//プレイヤーをターゲットとして位置参照するオブジェクトに付ける。Transformだと位置変更の権利も渡してしまうのでVectorだけ渡すのが最も良いが一端これ。
public interface IAimPlayer
{
    public void SetPlayerTrans(Transform playerTrans); //ターゲット設定
    public void CancelPlayerTrans(); //ターゲット解除
    public bool IsExistPlayer(); //ターゲットが存在しているか
    public Vector3 GetPlayerPos(); //ターゲットの位置だけ返す。
}

//一時停止可能オブジェクトに付ける。
public interface IPausable
{
    public void Paused(); //一時停止
    public void Resumed(); //一時停止解除
}

//ステージのエリアに所属するギミックにつける。
//ゲームオブジェクトそのもののオンオフではなくスクリプトの機能変更により実装。
public interface IAreaObject
{
    public event Action OnActive; //起動
    public event Action OnDeactive; //停止
    public void ActiveSwitch(bool isactive); //イベントを外部から起動してほしいので実装
}
//初期化が必要なギミックに付ける
public interface IInitGimmick
{
    public void Initialize();
}

//ダメージを受けるオブジェクトに付ける。
public interface IDamageable
{
    public bool CanDamage(); //ダメージが与えられる状態か。参照側が使う。
    public void Damage(int value); //数値分のダメージを受ける。
    public void FatalDamage(); //即死ダメージ。残り体力ダメージを与えるのと同じ。 
    public void Dead(); //死亡
}
//プレイヤーの攻撃で吹き飛ばせるオブジェクトに付ける。
//敵の場合、先に吹き飛ばし処理後、ダメージ適用。死んだら吹っ飛ぶようにする。
public interface IHittable
{
    public bool CanHitted();
    public void Hitted(float angle); //チャージ攻撃された時の吹き飛びアングルを引数で渡す。
}

//キャラクターの誕生・死亡イベント。
//外部から起こせるように関数準備。
//使用例
//誕生開始は生成者が生成と同時に呼び。終了はアニメーション終了時に呼ぶ。
//死亡開始はダメージで呼び、終了はアニメーション終了時に呼ぶ。
public interface ICharactorEvents
{
    public event System.Action OnBirthStart;
    public void BirthStart();

    public event System.Action OnBirthEnd;
    public void BirthEnd();

    public event System.Action OnDeathStart;
    public void DeathStart();

    public event System.Action OnDeathEnd;
    public void DeathEnd();
}

//流れる床。影響を受けるオブジェクトで参照。
public interface IFlowingFloor
{
    public float FlowingSpeed(); //どれくらいの速度変化を行うか。
}
//動く床。影響を受けるオブジェクトで参照。
public interface IMovingFloor
{
    public Vector2 MovingSpeed();
}