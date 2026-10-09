using UnityEngine;

public class EasyPartBossAnimation : MonoBehaviour
{
    private Animator animator = null;
    private static readonly int birthStateHash = Animator.StringToHash("Birth");
    private static readonly int deathStateHash = Animator.StringToHash("Death");
    private static readonly int tackleChargeParameterHash = Animator.StringToHash("tackleCharge");
    private static readonly int tackleParameterHash = Animator.StringToHash("tackle");
    private static readonly int tackleStunParameterHash = Animator.StringToHash("tackleStun");
    private static readonly int tackleStunEndParameterHash = Animator.StringToHash("tackleStunEnd");
    private static readonly int shootChargeParameterHash = Animator.StringToHash("shootCharge");
    private static readonly int shootParameterHash = Animator.StringToHash("shoot");
    private static readonly int shootEndParameterHash = Animator.StringToHash("shootEnd");
    private static readonly int groundParameterHash = Animator.StringToHash("ground");
    private static readonly int jumpParameterHash = Animator.StringToHash("jump");
    [SerializeField] private GroundChecker groundChecker = null;
    [SerializeField] private EasyPartBossState state = null;
    private bool isGround = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(groundParameterHash, isGround);
        animator.SetBool(jumpParameterHash, state.IsJump());
    }

    void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
    }

    //攻撃スクリプトが呼ぶ。
    //突進遷移系。
    public void TackleChargeTrigger() { animator.SetTrigger(tackleChargeParameterHash); }
    public void TackleTrigger() { animator.SetTrigger(tackleParameterHash); }
    public void TackleStunTrigger() { animator.SetTrigger(tackleStunParameterHash); }
    public void TackleStunEndTrigger() { animator.SetTrigger(tackleStunEndParameterHash); }
    //弾発射遷移系。
    public void ShootChargeTrigger() { animator.SetTrigger(shootChargeParameterHash); }
    public void ShootTrigger() { animator.SetTrigger(shootParameterHash); }
    public void ShootEndTrigger() { animator.SetTrigger(shootEndParameterHash); }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    //登場アニメーション再生。イベントスクリプトが使う。
    public void BirthPlay() { animator.Play(birthStateHash); }
    //死亡アニメーション再生。イベントスクリプトが使う。
    public void DeathPlay() { animator.Play(deathStateHash); }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        animator.speed = ispause ? 0 : 1;
    }
}
