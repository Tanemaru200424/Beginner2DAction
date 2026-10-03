using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAnimationTransition
{
    private Animator baseAnim = null;
    private Animator shooterAnim = null;

    public SEAnimationTransition(Animator baseAnim, Animator shooterAnim)
    {
        this.baseAnim = baseAnim;
        this.shooterAnim = shooterAnim;
    }

    public void UpdateAnimation(float level)
    {
        shooterAnim.SetFloat("level", level);
    }

    public void AttackPlay() { shooterAnim.Play("Attack"); }
    public void AttackCancel() { shooterAnim.Play("Stand"); }

    public void DeadPlay()
    {
        baseAnim.Play("Death");
        shooterAnim.Play("Death");
    }
    public void HittedPlay()
    {
        baseAnim.Play("Hitted");
        shooterAnim.Play("Hitted");
    }
}
