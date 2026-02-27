using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEEvents : MonoBehaviour, ICharactorEvents
{
    private SEController controller = null;

    void Awake()
    {
        controller = GetComponent<SEController>();
    }

    void Update()
    {
        //吹き飛び終了を検知次第退場終了を呼ぶ。
        if (controller.IsHittedEnd()) { DeathEnd(); }
    }

    public event System.Action OnBirthStart;
    public void BirthStart()
    {
        controller.BirthStart();
        OnBirthStart?.Invoke();

        BirthEnd(); //開始イベントを直ぐに終了させる。
    }

    public event System.Action OnBirthEnd;
    public void BirthEnd()
    {
        controller?.BirthEnd();
        OnBirthEnd?.Invoke();
    }

    public event System.Action OnDeathStart;
    public void DeathStart()
    {
        controller.DeathStart();
        OnDeathStart?.Invoke();
    }

    public event System.Action OnDeathEnd;
    public void DeathEnd()
    {
        controller.DeathEnd();
        OnDeathEnd?.Invoke();
    }
}
