using UnityEngine;

public class EasyPartBossMove : MonoBehaviour
{
    [SerializeField] private float tackleSpeed = 20; //突進の速度。加速せず一定速度。
    [SerializeField] private float jumpXSpeed = 10; //ジャンプ時のX軸速度。加速せず一定速度。
    [SerializeField] private float minYSpeed = 3; //最低落下速度
    [SerializeField] private float maxYSpeed = 15; //最大落下速度
    [SerializeField] private float yAccelerationDistance = 3; //通常時にY軸速度加算。

    private IAimPlayer iaimPlayer = null;
    private EasyPartBossState state = null;
    [SerializeField] private AffectedByFloor affectedByFloor = null;
    [SerializeField] private GroundChecker wallTackleStopper = null; //突進を止める壁判定

    private float fallStartY = 0; //落下開始位置
    private float jumpStartY = 0; //ジャンプ開始位置
    private float airSpeed = 0.0f; //空中ジャンプ速度設定

    private Rigidbody2D rigidBody2D = null;

    private void Awake()
    {
        iaimPlayer = GetComponent<IAimPlayer>();
        state = GetComponent<EasyPartBossState>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //生成時に向き調整
        if (iaimPlayer.IsExistPlayer() &&
           this.transform.localScale.x * (this.transform.position.x - iaimPlayer.GetPlayerPos().x) > 0)
        {
            this.transform.localScale = Vector3.Scale(this.transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    void Update()
    {
        if (state.IsFallStart()) { fallStartY = this.transform.position.y; }
        if (state.IsStand()) { airSpeed = 0.0f; }
        this.transform.localScale = UpdateScale();
    }

    void FixedUpdate()
    {
        rigidBody2D.linearVelocity = new Vector2(UpdateXSpeed(), UpdateYSpeed());
    }

    //向き更新
    private Vector3 UpdateScale()
    {
        Vector3 result = this.transform.localScale;
        if (state.CanTurn() && iaimPlayer.IsExistPlayer() &&
            this.transform.localScale.x * (this.transform.position.x - iaimPlayer.GetPlayerPos().x) > 0)
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        return result;
    }
    //突進中に吹き飛ばしが発生したときの向き反転。ただし、吹き飛ばしのX軸方向要素が今の向きと逆である必要がある。
    public void TacckleTurnByHitted(float angle) 
    {
        float cosX = Mathf.Cos(angle * Mathf.Deg2Rad);
        if (cosX * this.transform.localScale.x < 0.0f)
        {
            this.transform.localScale = Vector3.Scale(this.transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    //コントローラーが使うジャンプ開始。この時に横移動速度を変更。
    public void JumpChargeStart()
    {
        if (state.CanJumpCharge())
        {
            state.JumpChargeStart();
            jumpStartY = this.transform.position.y;
            airSpeed = jumpXSpeed;
        }
    }

    //X軸速度更新
    public float UpdateXSpeed()
    {
        float result = 0f;
        if (state.IsTackleXMove()) { result = Mathf.Sign(this.transform.localScale.x) * (wallTackleStopper.IsGround() ? 0 : tackleSpeed) + AffectedSpeed().x; }
        else if (state.IsAirXMove()) { result = Mathf.Sign(this.transform.localScale.x) * airSpeed + AffectedSpeed().x; }
        else if (state.IsCantXMove()) { result = AffectedSpeed().x; }
        else if (state.IsStopXMove()) { result = 0; }

        return result;
    }

    //Y軸速度更新
    public float UpdateYSpeed()
    {
        float result = 0f;
        if (state.IsJumpYMove())
        {
            float speedRatio = Mathf.Abs(this.transform.position.y - jumpStartY) / yAccelerationDistance;
            speedRatio = Mathf.Clamp01(speedRatio);
            result = maxYSpeed - (maxYSpeed - minYSpeed) * speedRatio + AffectedSpeed().y;
        }
        else if (state.IsFallYMove())
        {
            float speedRatio = Mathf.Abs(this.transform.position.y - fallStartY) / yAccelerationDistance;
            speedRatio = Mathf.Clamp01(speedRatio);
            result = -minYSpeed - (maxYSpeed - minYSpeed) * speedRatio + AffectedSpeed().y;
        }
        else if (state.IsCantYMove()) { result = AffectedSpeed().y; }
        else if (state.IsStopYMove()) { result = 0; }
        return result;
    }

    //流れる床、動く床の影響を受けた速度を返す。
    private Vector2 AffectedSpeed()
    {
        return new Vector2(affectedByFloor.AffectedFlowingFloor() + affectedByFloor.AffectedMovingFloor().x, affectedByFloor.AffectedMovingFloor().y);
    }

    //一時停止制御
    public void PauseSwitch(bool ispause)
    {
        if (ispause) { rigidBody2D.Sleep(); }
        else { rigidBody2D.WakeUp(); }
        this.enabled = !ispause;
    }
}
