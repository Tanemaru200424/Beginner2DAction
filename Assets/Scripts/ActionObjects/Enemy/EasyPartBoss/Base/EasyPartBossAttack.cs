using UnityEngine;

//アニメーションのステート時間ではなく、攻撃側で攻撃の溜めや攻撃時間を管理する。
public class EasyPartBossAttack : MonoBehaviour
{
    private EasyPartBossState state = null;
    //private EasyPartBossEffectGenerator effectGenerator = null;
    [SerializeField] private Collider2D bodyAttack2D = null;
    [SerializeField] private Collider2D tackleAttack2D = null;
    [SerializeField] private EasyPartBossAnimation epbAnimation = null;
    //[SerializeField] private Transform shootTrans = null; //弾を発射する位置
    //[SerializeField] private Transform rubbleGenerateTrans = null; //突進中壁にぶつかっている間、瓦礫を生成する位置
    [SerializeField] private GroundChecker wallGroundChecker = null; //突進中壁にぶつかっているか判定する
    [SerializeField] private AccessoriesLoopEffect tackleEffect = null;

    //攻撃の溜め時間や攻撃時間は、射撃を除きアニメーションのステート時間ではなく、攻撃側で時間管理する。
    [SerializeField] private float tackleChargeTime = 1.0f;
    [SerializeField] private float tackleTime = 3.0f;
    [SerializeField] private float tackleStunTime = 1.0f;
    [SerializeField] private float shootChargeTime = 1.0f;
    private float holdAttackStateTime = 0.0f; //攻撃の溜めや攻撃時間を計測するカウントタイマー。攻撃は同時に複数状態をとらないので１つのタイマー。
    [SerializeField] private float rubbleGenerateInterval = 0.5f; //突進中壁にぶつかっている間、瓦礫を生成する間隔
    private float rubbleGenerateTime = 0.0f; //突進中壁にぶつかっている間、瓦礫を生成する間隔を計測するカウントタイマー

    void Awake()
    {
        state = GetComponent<EasyPartBossState>();
        //effectGenerator = GetComponent<EasyPartBossEffectGenerator>();
        tackleEffect.Init();
        BodyAttackSwitchNormal();
        holdAttackStateTime = 0.0f;
    }

    void Update()
    {
        if(holdAttackStateTime > 0.0f)
        {
            holdAttackStateTime -= Time.deltaTime;
            if (holdAttackStateTime <= 0.0f)
            {
                holdAttackStateTime = 0.0f;
                if (state.IsTackleCharge())
                {
                    TackleStart();
                }
                else if (state.IsTackle())
                {
                    TackleStunStart();
                }
                else if (state.IsTackleStun())
                {
                    TackleStunEnd();
                }
                else if (state.IsShootCharge())
                {
                    ShootStart();
                }
            }
            else
            {
                if (state.IsTackle())
                {
                    if (wallGroundChecker.IsGround())
                    {
                        rubbleGenerateTime -= Time.deltaTime;
                        if (rubbleGenerateTime <= 0.0f)
                        {
                            rubbleGenerateTime = rubbleGenerateInterval;
                            GenerateRubble();
                        }
                    }
                    else { rubbleGenerateTime = rubbleGenerateInterval; }
                }
            }
        }
    }

    //攻撃待機開始。コントローラ―が呼び出す。
    public void TackleChargeStart()
    {
        if (state.CanTackleChargeStart())
        {
            epbAnimation.TackleChargeTrigger();
            state.TackleChargeStart();
            holdAttackStateTime = tackleChargeTime;
        }
    }
    public void ShootChargeStart()
    {
        if (state.CanShootChargeStart())
        {
            epbAnimation.ShootChargeTrigger();
            state.ShootChargeStart();
            holdAttackStateTime = shootChargeTime;
        }
    }

    //攻撃の遷移。基本的に攻撃管理で時間計測して遷移。
    private void TackleStart()
    {
        if (state.CanTackleStart())
        {
            epbAnimation.TackleTrigger();
            state.TackleStart();
            BodyAttackSwitchTackle();
            holdAttackStateTime = tackleTime;
            rubbleGenerateTime = rubbleGenerateInterval;
        }
    }
    private void TackleStunStart()
    {
        if (state.CanTackleStunStart())
        {
            epbAnimation.TackleStunTrigger();
            state.TackleStunStart();
            BodyAttackSwitchNormal();
            holdAttackStateTime = tackleStunTime;
        }
    }
    private void TackleStunEnd()
    {
        if (state.CanTackleStunEnd())
        {
            epbAnimation.TackleStunEndTrigger();
            state.TackleStunEnd();
        }
    }
    private void ShootStart()
    {
        if (state.CanShootStart())
        {
            epbAnimation.ShootTrigger();
            state.ShootStart();
        }
    }
    //射撃終了遷移。これだけはアニメーションイベントが呼び出す。
    public void ShootEnd()
    {
        if (state.CanShootEnd())
        {
            epbAnimation.ShootEndTrigger();
            state.ShootEnd();
            holdAttackStateTime = 0.0f;
        }
    }

    public void TackleEffectSwitch(bool istackle)
    {
        tackleEffect.EffectSwitch(istackle);
    }

    //弾を発射。アニメーションイベントが呼び出す。
    public void Shoot()
    {
        //bool isFlip = (this.transform.localScale.x < 0f);
        //effectGenerator.GenerateBullet(shootTrans.position, isFlip);
        //effectGenerator.GenerateShootEffect(shootTrans.position, isFlip);
    }
    //瓦礫生成。突進中壁にぶつかっている間に定期的に生成。
    private void GenerateRubble()
    {
        //bool isFlip = (this.transform.localScale.x < 0f);
        //effectGenerator.GenerateRubble(rubbleGenerateTrans.position, isFlip);
    }

    //体の攻撃判定オンオフ。ダメージスクリプトとイベントスクリプトが使う。
    public void BodyAttackSwitchNormal() 
    {
        bodyAttack2D.enabled = true;
        tackleAttack2D.enabled = false;
    }
    private void BodyAttackSwitchTackle()
    {
        bodyAttack2D.enabled = false;
        tackleAttack2D.enabled = true;
    }
    public void BodyAttackSwitchOff()
    {
        bodyAttack2D.enabled = false;
        tackleAttack2D.enabled = false;
    }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        tackleEffect.PauseSwitch(ispause);
    }
}
