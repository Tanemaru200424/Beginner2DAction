using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//プレイヤーの入力検知を行う。
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerDamage damage = null;
    private PlayerState state = null;
    private PlayerMove move = null;
    private PlayerAttack attack = null;
    [SerializeField] private PlayerAnimation pAnimation = null;

    private bool isActive = true; //入力を受け付ける状態か。

    private float walkDirection = 0;
    private bool isHoldJump = false;
    private bool isHoldAttack = false;
    private float attackShift = 0;
    private bool saveHoldAttack = false; //入力無効化中も攻撃の溜め、発動制御をしたいので無効化直前の状態を保存。

    //入力によって実行される各イベント
    private event Action OnPausePressed = null; //一時停止は外部から注入

    void Awake()
    {
        state = GetComponent<PlayerState>();
        move = GetComponent<PlayerMove>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        state.HoldJump(isActive ? isHoldJump : false);

        move.SetXDirection(isActive ? walkDirection : 0);

        attack.HoldAttack(isActive ? isHoldAttack : saveHoldAttack);
        attack.SetShift(isActive ? attackShift : 0);

        pAnimation.SetInputX(isActive ? walkDirection : 0);
    }

    public void ActiveSwitch(bool isactive)
    {
        isActive = isactive;
        saveHoldAttack = isHoldAttack;
    }

    //移動入力検知。
    public void OnWalk(InputAction.CallbackContext context)
    {
        if (context.performed) { walkDirection = context.ReadValue<float>(); }
        else if (context.canceled) { walkDirection = 0; }
    }

    //ジャンプ検知
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHoldJump = true;
            if (isActive) { move.JumpStart(); }
        }
        else if (context.canceled) { isHoldJump = false; }
    }

    //イベント等で入力が無効化された時の再現用
    public void OnTest(InputAction.CallbackContext context)
    {
        if (context.started && isActive)
        {
            damage.Damage(10);
        }
    }

    //攻撃入力検知。
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) { isHoldAttack = true; }
        else if (context.canceled) { isHoldAttack = false; }
    }
    //吹き飛ばし方向入力検知。
    public void OnShift(InputAction.CallbackContext context)
    {
        if (context.performed) { attackShift = context.ReadValue<float>(); }
        else if (context.canceled) { attackShift = 0; }
    }

    //一時停止入力検知
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && isActive)
        {
            OnPausePressed?.Invoke();
        }
    }
    //生成時に一時停止イベントを受け取る。
    public void AddPausePressed(Action action)
    {
        OnPausePressed += action;
    }
}

/*
//リファクタリング前の元のコード
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//プレイヤーの入力検知を行う。
public class PlayerInputHandler : MonoBehaviour
[SerializeField] private PlayerDamage damage = null;
private PlayerController controller = null;

private bool isActive = true; //入力を受け付ける状態か。

private float walkDirection = 0;
private bool isHoldJump = false;
private bool isHoldAttack = false;
private bool saveHoldAttack = false; //入力無効化中も攻撃の溜め、発動制御をしたいので無効化直前の状態を保存。

//入力によって実行される各イベント
private event Action OnPausePressed = null; //一時停止は外部から注入

void Awake()
{
    controller = GetComponent<PlayerController>();
}

void Update()
{
    controller.HoldHorizontal(isActive ? walkDirection : 0);
    controller.HoldJump(isActive ? isHoldJump : false);
    controller.HoldAttack(isActive ? isHoldAttack : saveHoldAttack);
}

public void ActiveSwitch(bool isactive)
{
    isActive = isactive;
    saveHoldAttack = isHoldAttack;
}

//移動入力検知。
public void OnWalk(InputAction.CallbackContext context)
{
    if (context.performed) { walkDirection = context.ReadValue<float>(); }
    else if (context.canceled) { walkDirection = 0; }
}

//ジャンプ検知
public void OnJump(InputAction.CallbackContext context)
{
    if (context.started)
    {
        isHoldJump = true;
        if (isActive)
        { 
            controller.JumpUp();
            controller.HoldJump(isHoldJump); //ジャンプ開始時の時点で書き換えしないとUpdate反映では間に合わない。
        }
    }
    else if (context.canceled) { isHoldJump = false; }
}

//イベント等で入力が無効化された時の再現用
public void OnTest(InputAction.CallbackContext context)
{
    if (context.started && isActive)
    {
        damage.Damage(10);
    }
}

//攻撃入力検知。
public void OnAttack(InputAction.CallbackContext context)
{
    if (context.started) { isHoldAttack = true; }
    else if (context.canceled) { isHoldAttack = false; }
}
//吹き飛ばし方向入力検知。
public void OnShift(InputAction.CallbackContext context)
{
    if (context.performed) { controller.SetShift(context.ReadValue<float>()); }
    else if (context.canceled) { controller.SetShift(0); }
}

//一時停止入力検知
public void OnPause(InputAction.CallbackContext context)
{
    if (context.started && isActive)
    {
        OnPausePressed?.Invoke();
    }
}
public void AddPausePressed(Action action)
{
    OnPausePressed += action;
}
*/