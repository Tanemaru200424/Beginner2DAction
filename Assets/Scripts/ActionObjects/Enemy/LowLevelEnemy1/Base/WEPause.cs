using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEPause : MonoBehaviour, IPausable
{
    private WEController controller = null;
    [SerializeField] private WEDamage damage = null;
    [SerializeField] private WEBodyAttack bodyAttack = null;
    [SerializeField] private WEHittedAttack hittedAttack = null;

    private Rigidbody2D rigidBody2D = null;
    [SerializeField] private Animator animator = null;

    void Awake()
    {
        controller = GetComponent<WEController>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Paused()
    {
        controller.PauseSwitch(true);
        damage.PauseSwitch(true);
        bodyAttack.PauseSwitch(true);
        hittedAttack.PauseSwitch(true);

        rigidBody2D.Sleep();
        animator.speed = 0;
    }
    public void Resumed()
    {
        controller.PauseSwitch(false);
        damage.PauseSwitch(false);
        bodyAttack.PauseSwitch(false);
        hittedAttack.PauseSwitch(false);

        rigidBody2D.WakeUp();
        animator.speed = 1;
    }
}