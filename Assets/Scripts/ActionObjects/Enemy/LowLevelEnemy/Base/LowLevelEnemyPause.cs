using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowLevelEnemyPause : MonoBehaviour, IPausable
{
    private LowLevelEnemyController controller = null;
    private LowLevelEnemyMove move = null;
    [SerializeField] private LowLevelEnemyDamage damage = null;
    [SerializeField] private LowLevelEnemyAnimation lleAnimation = null;
    [SerializeField] private EnemyBodyAttack bodyAttack = null;
    [SerializeField] private EnemyHittedAttack hittedAttack = null;

    private Rigidbody2D rigidBody2D = null;
    [SerializeField] private Animator animator = null;

    void Awake()
    {
        controller = GetComponent<LowLevelEnemyController>();
        move = GetComponent<LowLevelEnemyMove>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Paused()
    {
        controller.PauseSwitch(true);
        move.PauseSwitch(true);
        damage.PauseSwitch(true);
        lleAnimation.PauseSwitch(true);
        bodyAttack.PauseSwitch(true);
        hittedAttack.PauseSwitch(true);

        rigidBody2D.Sleep();
        animator.speed = 0;
    }
    public void Resumed()
    {
        controller.PauseSwitch(false);
        move.PauseSwitch(false);
        damage.PauseSwitch(false);
        lleAnimation.PauseSwitch(false);
        bodyAttack.PauseSwitch(false);
        hittedAttack.PauseSwitch(false);

        rigidBody2D.WakeUp();
        animator.speed = 1;
    }
}