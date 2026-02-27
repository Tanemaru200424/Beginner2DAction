using UnityEngine;
using UnityEngine.UI;

//プレイヤーとボスの体力ゲージなどアクションに関係するUIを操作する。
public class ActionUIController : MonoBehaviour
{
    [SerializeField] private GameObject playerUI = null;
    [SerializeField] private Slider playerHp = null;
    [SerializeField] private Slider playerCharge = null;
    [SerializeField] private Image chargeImage = null;

    [SerializeField] private GameObject bossUI = null;
    [SerializeField] private Slider bossHp = null;

    void Awake()
    {
        playerHp.maxValue = 1;
        playerCharge.maxValue = 1;
        bossHp.maxValue = 1;
        UIClear();
    }

    //UI表示関係
    //UIをすべて見えなくする。
    public void UIClear()
    {
        PlayerUISwitch(false);
        BossUISwitch(false);
    }
    //プレイヤーのUI表示非表示。
    public void PlayerUISwitch(bool isactive) { playerUI.SetActive(isactive); }
    //ボス戦UIの表示非表示。
    public void BossUISwitch(bool isactive) { bossUI.SetActive(isactive); }

    //スライダーの数値変更。
    public void SetPlayerHpRate(float rate) 
    {
        rate = Mathf.Clamp(rate, 0, 1);
        playerHp.value = rate;
    }
    public void SetPlayerChargeRate(float rate)
    {
        rate = Mathf.Clamp(rate, 0, 1);
        if (rate < 1) { chargeImage.color = new Color(200, 200, 200, 1); }
        else { chargeImage.color = new Color(0, 255, 255, 1); }
        playerCharge.value = rate;
    }
    public void SetBossHpRate(float rate)
    {
        rate = Mathf.Clamp(rate, 0, 1);
        bossHp.value = rate;
    }
}
