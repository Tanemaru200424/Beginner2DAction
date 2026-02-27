using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPause : MonoBehaviour, IPausable
{
    private Rigidbody2D rb2D = null;
    private Animator animator = null;
    private BulletController controller = null;
    private BulletBodyAttack bodyAttack = null;
    [SerializeField] private BulletHitted hitted = null;
    [SerializeField] private BulletHittedAttack hittedAttack = null;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = GetComponent<BulletController>();
        bodyAttack = GetComponent<BulletBodyAttack>();
    }

    public void Paused()
    {
        controller.PauseSwitch(true);
        bodyAttack.PauseSwitch(true);
        hitted.PauseSwitch(true);
        hittedAttack.PauseSwitch(true);

        rb2D.Sleep();
        animator.speed = 0;
    }
    public void Resumed()
    {
        controller.PauseSwitch(false);
        bodyAttack.PauseSwitch(false);
        hitted.PauseSwitch(false);
        hittedAttack.PauseSwitch(false);

        rb2D.WakeUp();
        animator.speed = 1;
    }
}
