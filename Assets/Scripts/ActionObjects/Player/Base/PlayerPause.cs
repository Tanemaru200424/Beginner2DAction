using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPause : MonoBehaviour, IPausable
{
    private PlayerState state = null;
    private PlayerMove move = null;
    private PlayerAttack attack = null;
    [SerializeField] private PlayerAnimation pAnimation = null;
    [SerializeField] private PlayerDamage damage = null;

    void Awake()
    {
        state = GetComponent<PlayerState>();
        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
    }

    public void Paused()
    {
        damage.PauseSwitch(true);
        state.PauseSwitch(true);
        move.PauseSwitch(true);
        attack.PauseSwitch(true);
        pAnimation.PauseSwitch(true);
    }
    public void Resumed()
    {
        damage.PauseSwitch(false);
        state.PauseSwitch(false);
        move.PauseSwitch(false);
        attack.PauseSwitch(false);
        pAnimation.PauseSwitch(false);
    }
}

/*
//リファクタリング前のコード
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPause : MonoBehaviour, IPausable
{
    private PlayerController controller = null;
    private PlayerAttack attack = null;
    [SerializeField] private PlayerDamage damage = null;

    private Rigidbody2D rigidBody2D = null;
    [SerializeField] private Animator animator = null;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        attack = GetComponent<PlayerAttack>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Paused()
    {
        controller.PauseSwitch(true);
        attack.PauseSwitch(true);
        damage.PauseSwitch(true);
        rigidBody2D.Sleep();
        animator.speed = 0;
    }
    public void Resumed()
    {
        controller.PauseSwitch(false);
        attack.PauseSwitch(false);
        damage.PauseSwitch(false);
        rigidBody2D.WakeUp();
        animator.speed = 1;
    }
}
 */