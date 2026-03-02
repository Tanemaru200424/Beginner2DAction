using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEAnimationTransition
{
    private Animator animator = null;

    public WEAnimationTransition(Animator animator)
    {
        this.animator = animator;
    }

    public void UpdateAnimation(bool isGround)
    {
        animator.SetBool("ground", isGround);
    }

    public void DamagePlay()
    {
        animator.Play("Damage");
    }
    public void DeathPlay()
    {
        animator.Play("Death");
    }
    public void HittedPlay()
    {
        animator.Play("Hitted");
    }
}
