using UnityEngine;

public class LowLevelEnemyAnimation : MonoBehaviour
{
    private Animator animator = null;
    private static readonly int damageStateHash = Animator.StringToHash("Damage");
    private static readonly int hittedStateHash = Animator.StringToHash("Hitted");
    private static readonly int deathStateHash = Animator.StringToHash("Death");
    private static readonly int walkParameterHash = Animator.StringToHash("walk");
    private static readonly int groundParameterHash = Animator.StringToHash("ground");
    [SerializeField] private LowLevelEnemyState state = null;
    [SerializeField] private GroundChecker groundChecker = null;
    private bool isGround = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(walkParameterHash, state.IsWalkBool());
        animator.SetBool(groundParameterHash, isGround);
    }

    void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
    }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    public void DamagePlay() { animator.Play(damageStateHash); }

    //死亡アニメーション再生。イベントスクリプトが使う。
    public void DeathPlay() { animator.Play(deathStateHash); }
    //吹き飛びアニメーション再生。イベントスクリプトが使う。
    public void HittedPlay() { animator.Play(hittedStateHash); }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        animator.speed = ispause ? 0 : 1;
    }
}
