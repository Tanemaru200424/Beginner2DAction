using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの登場退場クリア時イベント
public class PlayerEvents : MonoBehaviour, ICharactorEvents 
{
    private PlayerState state = null;
    [SerializeField] private PlayerAnimation pAnimation = null;
    [SerializeField] private Collider2D eventCol2D = null;
    [SerializeField] private Collider2D hitBoxCol2D = null;

    void Awake()
    {
        state = GetComponent<PlayerState>();
    }

    public event System.Action OnBirthStart;
    public void BirthStart()
    {
        state.BirthStart();
        pAnimation.BirthPlay();
        OnBirthStart?.Invoke();
    }

    public event System.Action OnBirthEnd;
    public void BirthEnd()
    {
        state?.BirthEnd();
        OnBirthEnd?.Invoke();
    }

    public event System.Action OnDeathStart;
    public void DeathStart()
    {
        state.DeathStart();
        eventCol2D.enabled = false;
        hitBoxCol2D.enabled = false;
        pAnimation.DeathPlay();
        OnDeathStart?.Invoke();
    }

    public event System.Action OnDeathEnd;
    public void DeathEnd()
    {
        Destroy(this.gameObject);
        OnDeathEnd?.Invoke();
    }
}

/*
 リファクタリング前のコード
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの登場退場クリア時イベント
public class PlayerEvents : MonoBehaviour, ICharactorEvents 
{
    private PlayerController controller = null;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    public event System.Action OnEntryStart;
    public void EntryStart()
    {
        controller.EntryStart();
        OnEntryStart?.Invoke();
    }

    public event System.Action OnEntryEnd;
    public void EntryEnd()
    {
        controller.EntryEnd();
        OnEntryEnd?.Invoke();
    }

    public event System.Action OnRetireStart;
    public void RetireStart()
    {
        controller.RetireStart();
        OnRetireStart?.Invoke();
    }

    public event System.Action OnRetireEnd;
    public void RetireEnd()
    {
        controller.RetireEnd();
        OnRetireEnd?.Invoke();
    }
}

 */