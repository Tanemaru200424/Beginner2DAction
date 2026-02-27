using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDamage : MonoBehaviour, IDamageable, IHittable
{
    [SerializeField] private SEController controller = null;
    [SerializeField] private SEEvents events = null;
    [SerializeField] private SEAttack attack = null;
    [SerializeField] private int maxHp = 3;
    private int nowHp = 0;

    private float zAngle = 0;
    private bool canHitted = false;

    private bool isPause = false;//ˆêŽž’âŽ~

    void Awake()
    {
        nowHp = maxHp;
    }

    public bool CanDamage() { return !isPause && nowHp > 0; }

    //œpœjƒ]ƒ“ƒr‚Í¬Œ^BUŒ‚‹­“x0ˆÈã‚Å‚æ‚ë‚¯A1ˆÈã‚ÌUŒ‚‚ÅŽ€‚ñ‚¾‚ç‚Á”ò‚Ô
    public void Damage(int value)
    {
        if (!isPause && nowHp > 0)
        {
            attack.ResetChargeTime();
            nowHp -= value;
            if (nowHp > 0)
            {
                controller.AttackCancel();
            }
            else
            {
                Dead();
            }
        }
    }
    public void FatalDamage() { Damage(nowHp); }

    public void Dead()
    {
        if (!isPause)
        {
            if (canHitted)
            {
                controller.HittedSwitch(canHitted);
                controller.SetHittedAngle(zAngle);
            }
            events.DeathStart();
        }
    }

    public bool CanHitted() { return !isPause; }
    public void Hitted(float zAngle)
    {
        if (!isPause)
        {
            canHitted = true;
            this.zAngle = zAngle;
        }
        else
        {
            canHitted = false;
        }
    }

    public void PauseSwitch(bool ispause)
    {
        isPause = ispause;
    }
}
