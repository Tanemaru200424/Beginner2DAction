using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//少しの間だけ出るエフェクトのアニメーション止める。
public class InstantEffectPause : MonoBehaviour, IPausable
{
    private Animator animator = null;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Paused() { animator.speed = 0; }
    public void Resumed() { animator.speed = 1; }
}
