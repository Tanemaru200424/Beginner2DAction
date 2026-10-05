using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLevelEnemyAttack : MonoBehaviour
{
    private MiddleLevelEnemyState state = null;
    protected MiddleLevelEnemyEffectGenerator effectsGenerator = null;
    [SerializeField] private MiddleLevelEnemyAnimation mleAnimation = null;
    [SerializeField] private Collider2D bodyAttack2D = null;
    [SerializeField] private Collider2D hittedAttack2D = null;

    [SerializeField] protected Transform shootTrans = null; //発射位置
    [SerializeField] private float xRange = 10; //プレイヤーとのx軸距離がこの値以上なら攻撃しない
    [SerializeField] private float yRange = 5; //プレイヤーとのy軸距離がこの値以上なら攻撃しない

    protected IAimPlayer aimPlayer = null;

    private bool isPause = false;

    void Awake()
    {
        state = GetComponent<MiddleLevelEnemyState>();
        effectsGenerator = GetComponent<MiddleLevelEnemyEffectGenerator>();
        aimPlayer = GetComponent<IAimPlayer>();

        BodyAttackSwitch(true);
        HittedAttackSwitch(false);
    }

    //ターゲットが存在しているか
    public bool CanAttack() { return !isPause && state.CanAttack() && aimPlayer.IsExistPlayer() && Mathf.Abs(aimPlayer.GetPlayerPos().x - transform.position.x) <= xRange && Mathf.Abs(aimPlayer.GetPlayerPos().y - transform.position.y) <= yRange; }

    //コントローラーが行う攻撃開始
    public void AttackStart() 
    {
        mleAnimation.AttackTrigger(); 
        state.AttackStart();
    }

    //アニメー所にベントが呼ぶ弾発射
    public virtual void Attack()
    {
        bool isFlip = (this.transform.localScale.x < 0f);
        effectsGenerator.GenerateBullet(shootTrans.position, isFlip ? 180 : 0);
    }

    //体の攻撃判定オンオフ。ダメージスクリプトとイベントスクリプトが使う。
    public void BodyAttackSwitch(bool isactive) { bodyAttack2D.enabled = isactive; }

    public void HittedAttackSwitch(bool isactive) { hittedAttack2D.enabled = isactive; }

    //一時停止用
    public void PauseSwitch(bool ispause) { isPause = ispause; }
}
