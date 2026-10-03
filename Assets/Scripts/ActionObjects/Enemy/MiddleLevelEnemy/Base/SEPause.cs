using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPause : MonoBehaviour, IPausable
{
    private SEController controller = null;
    private SEAttack attack = null;
    [SerializeField] private SEDamage damage = null;
    [SerializeField] private SEBodyAttack bodyAttack = null;
    [SerializeField] private SEHittedAttack hittedAttack = null;

    private Rigidbody2D rigidBody2D = null;
    [SerializeField] private Animator baseAnim = null;
    [SerializeField] private Animator shooterAnim = null;

    void Awake()
    {
        controller = GetComponent<SEController>();
        attack = GetComponent<SEAttack>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Paused()
    {
        controller.PauseSwitch(true);
        attack.PauseSwitch(true);
        damage.PauseSwitch(true);
        bodyAttack.PauseSwitch(true);
        hittedAttack.PauseSwitch(true);

        rigidBody2D.Sleep();
        baseAnim.speed = 0;
        shooterAnim.speed = 0;
    }
    public void Resumed()
    {
        controller.PauseSwitch(false);
        attack.PauseSwitch(false);
        damage.PauseSwitch(false);
        bodyAttack.PauseSwitch(false);
        hittedAttack.PauseSwitch(false);

        rigidBody2D.WakeUp();
        baseAnim.speed = 1;
        shooterAnim.speed = 1;
    }
}