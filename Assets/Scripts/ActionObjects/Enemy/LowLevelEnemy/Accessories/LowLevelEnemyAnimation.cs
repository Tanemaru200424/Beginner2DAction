using UnityEngine;

public class LowLevelEnemyAnimation : MonoBehaviour
{
    private Animator animator = null;
    [SerializeField] private LowLevelEnemyState state = null;
    [SerializeField] private GroundChecker groundChecker = null;
    private bool isGround = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("walk", state.IsWalkBool());
        animator.SetBool("ground", isGround);
    }

    void FixedUpdate()
    {
        isGround = groundChecker.IsGround();
    }

    //ダメージアニメーション再生。ダメージスクリプトが使う。
    public void DamagePlay() { animator.Play("Damage"); }

    //死亡アニメーション再生。イベントスクリプトが使う。
    public void DeathPlay() { animator.Play("Death"); }
    //吹き飛びアニメーション再生。イベントスクリプトが使う。
    public void HittedPlay() { animator.Play("Hitted"); }

    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
        animator.speed = ispause ? 0 : 1;
    }
}
