using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEDamage : MonoBehaviour, IDamageable, IHittable
{
    [SerializeField] private WEController controller = null;
    [SerializeField] private WEEvents events = null;
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
    public void Damage(int value)
    {
        nowHp -= value;
        if (nowHp > 0)
        {
            controller.DamageStart();
        }
        else
        {
            Dead();
        }
        canHitted = false;
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
        if(!isPause) 
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
