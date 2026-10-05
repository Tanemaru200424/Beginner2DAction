using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPause : MonoBehaviour, IPausable
{
    private BulletController controller = null;
    private BulletBodyAttack bodyAttack = null;
    [SerializeField] private BulletHitted hitted = null;
    [SerializeField] private BulletHittedAttack hittedAttack = null;

    void Awake()
    {
        controller = GetComponent<BulletController>();
        bodyAttack = GetComponent<BulletBodyAttack>();
    }

    public void Paused()
    {
        controller.PauseSwitch(true);
        bodyAttack.PauseSwitch(true);
        hitted.PauseSwitch(true);
        hittedAttack.PauseSwitch(true);
    }
    public void Resumed()
    {
        controller.PauseSwitch(false);
        bodyAttack.PauseSwitch(false);
        hitted.PauseSwitch(false);
        hittedAttack.PauseSwitch(false);
    }
}
