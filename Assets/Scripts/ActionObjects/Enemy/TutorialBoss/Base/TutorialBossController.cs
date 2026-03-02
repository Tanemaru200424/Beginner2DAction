using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボスの行動パターンを司る。
public class TutorialBossController : MonoBehaviour
{
    private TutorialBossState state = null;
    private TutorialBossAttack attack = null;
    private TutorialBossMove move = null;

    [SerializeField] private float maxCoolTime = 0.5f;
    private float nowCoolTime = 0;
    private bool tackleSwitch = true;
    
    void Awake()
    {
        state = GetComponent<TutorialBossState>();
        attack = GetComponent<TutorialBossAttack>();
        move = GetComponent<TutorialBossMove>();
        nowCoolTime = maxCoolTime;
        tackleSwitch = true;
    }

    void Update()
    {
        //地上にいる時の通常状態で待機時間を減らす。突進と弾を交互に繰り出す。
        if (state.CanCountCoolTime()) { nowCoolTime -= Time.deltaTime; }
        if(nowCoolTime <= 0 && state.CanCharge()) 
        {
            if(tackleSwitch) 
            {
                attack.TackleChargeStart();
                tackleSwitch = false;
            }
            else
            {
                attack.ShootChargeStart();
                tackleSwitch = true;
            }
            nowCoolTime = maxCoolTime;
        }
        //突進状態で特定位置まで行ったら突進解除。
        if (state.IsTackle() && move.IsReachTacklePoint()) { attack.TackleEnd(); }
    }
    
    //一時停止。一時停止管理スクリプトが使う。
    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
    }
}
