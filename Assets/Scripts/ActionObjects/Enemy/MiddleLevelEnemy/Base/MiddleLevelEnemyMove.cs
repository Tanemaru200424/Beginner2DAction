using UnityEngine;

public class MiddleLevelEnemyMove : MonoBehaviour
{
    [SerializeField] private float verticalMoveWidth = 6; //上下移動の移動幅
    [SerializeField] private float maxVerticalSpeed = 2.0f;   // 最大速度
    [SerializeField] private float minVerticalSpeed = 0.5f;   // 最小速度
    private float initY = 0; //生成時のY座標
    private float fromY = 0; //出発元のY座標
    private float toY = 0; //目指すのY座標
    private bool isUp = true; //移動の向き。上向きか否か。

    [SerializeField] private float hittedSpeed = 20; //吹き飛び速度
    [SerializeField] private float hittedDistance = 20; //吹き飛び距離
    private Vector3 hittedStartPos = new Vector3(0, 0, 0);
    private Vector2 hittedVector = new Vector2(0, 0);

    private MiddleLevelEnemyState state = null;

    private IAimPlayer iaimPlayer = null;

    private Rigidbody2D rigidBody2D = null;

    private void Awake()
    {
        state = GetComponent<MiddleLevelEnemyState>();
        iaimPlayer = GetComponent<IAimPlayer>();
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

        initY = this.transform.position.y;
        isUp = true;
        fromY = initY;
        toY = initY + (verticalMoveWidth * 0.5f) * (isUp ? 1 : -1);
    }

    void Update()
    {
        this.transform.localScale = UpdateScale();
    }

    void FixedUpdate()
    {
        rigidBody2D.linearVelocity = new Vector2(UpdateXSpeed(), UpdateYSpeed());
    }

    protected virtual Vector3 UpdateScale()
    {
        Vector3 result = this.transform.localScale;
        if (state.CanTurn() &&
            this.transform.localScale.x * (this.transform.position.x - iaimPlayer.GetPlayerPos().x) > 0)
        {
            result = Vector3.Scale(result, new Vector3(-1, 1, 1));
        }
        return result;
    }
    public void DamageFlip()
    {
        if (state.CanDamageFlip()) { this.transform.localScale = Vector3.Scale(this.transform.localScale, new Vector3(-1, 1, 1)); }
    }

    public float UpdateXSpeed()
    {
        float result = 0f;
        if (state.IsStopXMove()) { result = 0; }
        else if (state.IsHittedXMove()) { result = hittedSpeed * hittedVector.x; }

        return result;
    }

    public float UpdateYSpeed()
    {
        float result = 0f; 
        if (state.IsFlightYMove())
        {
            float centerY = (fromY + toY) * 0.5f;
            float velocity = minVerticalSpeed + (1.0f - Mathf.Abs(this.transform.position.y - centerY) / (verticalMoveWidth * 0.25f)) * (maxVerticalSpeed - minVerticalSpeed);
            result = isUp ? velocity : -velocity;
        }
        else if (state.IsStopYMove()) { result = 0; }
        else if (state.IsHittedYMove()) { result = hittedSpeed * hittedVector.y; }
        return result;
    }


    public void HittedStart(Vector3 startPos, float angle)
    {
        hittedStartPos = startPos;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        hittedVector = rotation * Vector2.right.normalized;
        Vector3 currentScale = this.transform.localScale;
        if (currentScale.x * hittedVector.x > 0)
        {
            this.transform.localScale = Vector3.Scale(currentScale, new Vector3(-1, 1, 1));
        }
    }
    public bool IsHittedLimmit() { return Vector3.Distance(this.transform.position, hittedStartPos) > hittedDistance; }

    //攻撃位置に到達したか。コントローラーがこれを参照して攻撃開始する。
    public bool IsReachAttackStartPos() 
    {
        return isUp ? this.transform.position.y >= toY : this.transform.position.y <= toY;
    }
    //攻撃位置に到達したなら次の移動目標に変更する。
    public void ChangeNextMoveTarget()
    {
        this.transform.position = new Vector3(this.transform.position.x, toY, this.transform.position.z);
        fromY = toY;

        //現在位置が上の端なら下降、下の端なら上昇に変更する。
        if (fromY >= initY + (verticalMoveWidth * 0.5f)) { isUp = false; }
        else if (fromY <= initY - (verticalMoveWidth * 0.5f)) { isUp = true; }

        toY = fromY + (verticalMoveWidth * 0.5f) * (isUp ? 1 : -1);
    }

    public void PauseSwitch(bool ispause)
    {
        if (ispause) { rigidBody2D.Sleep(); }
        else { rigidBody2D.WakeUp(); }
        this.enabled = !ispause;
    }
}
