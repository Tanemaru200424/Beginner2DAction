using UnityEngine;

//キャラクターなどにつける、アニメーションを繰り返すだけのエフェクトにつける。
public class AccessoriesLoopEffect : MonoBehaviour
{
    private Animator animator = null;

    public void Init()
    {
        animator = GetComponent<Animator>();
        EffectSwitch(false);
    }

    public void EffectSwitch(bool isdamage)
    {
        this.gameObject.SetActive(isdamage);
        if (isdamage) { animator?.Play("Effect"); }
    }

    public void PauseSwitch(bool ispause)
    {
        if (ispause) { animator.speed = 0; }
        else { animator.speed = 1; }
    }
}
