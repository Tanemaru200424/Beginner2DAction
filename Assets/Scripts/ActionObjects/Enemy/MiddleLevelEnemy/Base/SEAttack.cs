using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAttack : MonoBehaviour/*, IAimPlayer*/
{
    private SEAccessoriesGenerator accessoriesGenerator = null;
    [SerializeField] private Transform shooterTrans = null; //砲身回転させる用
    [SerializeField] private Transform shootTrans = null; //発射位置
    [SerializeField] private float maxChargeTime = 2;
    [SerializeField] private float range = 10;

    private bool isCharge = false;
    private float nowChargeTime = 0;
    private Transform playerTrans = null;

    void Awake()
    {
        accessoriesGenerator = GetComponent<SEAccessoriesGenerator>();
        nowChargeTime = 0;
    }

    void Update()
    {
        if (IsExistTarget())
        {
            if (isCharge)
            {
                nowChargeTime += Time.deltaTime;
                nowChargeTime = Mathf.Clamp(nowChargeTime, 0, maxChargeTime);

                Vector2 targetVector = playerTrans.position - this.transform.position;
                float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
                shooterTrans.rotation = Quaternion.Lerp(shooterTrans.rotation, targetRotation, 10 * Time.deltaTime);
            }
        }
        else
        {
            Vector2 targetVector = new Vector2(0, -1);
            float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            shooterTrans.rotation = Quaternion.Lerp(shooterTrans.rotation, targetRotation, 10 * Time.deltaTime);
            nowChargeTime = 0;
        }
    }

    public void SetPlayerTrans(Transform playerTrans) { this.playerTrans = playerTrans; }
    public void CancelPlayerTrans() { this.playerTrans = null; }

    public void ChargeSwitch(bool ischarge) { isCharge = ischarge; }
    public float ChargeLevel() { return nowChargeTime / maxChargeTime; }
    public void ResetChargeTime() { nowChargeTime = 0; } //攻撃受けたらチャージリセット

    //ターゲットが存在しているか
    public bool IsExistTarget()
    {
        if (playerTrans == null) { return false; }
        else if (Vector2.Distance(this.transform.position, playerTrans.position) > range) { return false; }
        return true;
    }

    //発射
    public bool IsChargeComplete() { return nowChargeTime >= maxChargeTime; }
    public void Attack()
    {
        //インスペクタの回転角はオイラーだがrotationで取得できるのはクォータニオン。オイラーに変換する。
        accessoriesGenerator.GenerateBullet(shootTrans.position, shooterTrans.rotation.eulerAngles.z);
    }

    //一時停止用
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
    }
}
