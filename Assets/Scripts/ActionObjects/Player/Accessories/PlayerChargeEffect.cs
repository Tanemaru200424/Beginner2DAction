using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeEffect : MonoBehaviour
{
    private Animator animator = null;

    public void Init()
    {
        animator = GetComponent<Animator>();
        ChargeSwitch(false);
    }

    public void ChargeSwitch(bool ischarge)
    {
        this.gameObject.SetActive(ischarge);
        if (ischarge) { animator?.Play("Charge"); }
    }

    public void ChargeComplete(bool iscomplete)
    {
        animator?.SetFloat("complete", iscomplete ? 1 : 0);
    }

    public void PauseSwitch(bool ispause)
    {
        if (ispause) { animator.speed = 0; }
        else { animator.speed = 1; }
    }
}
