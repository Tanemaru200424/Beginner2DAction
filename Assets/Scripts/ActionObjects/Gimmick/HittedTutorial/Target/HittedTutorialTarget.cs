using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedTutorialTarget : MonoBehaviour
{
    [SerializeField] private HittedTutorialWallDamage damage = null;
    [SerializeField] private HittedTutorialWallEffectGenerator effectGenerator = null;
    [SerializeField] private AccessoriesLoopEffect attentionMark = null;
    private Animator animator = null;
    private Collider2D col2D = null;
    private bool isDamaged = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
        attentionMark.Init();
        isDamaged = false;
    }

    public void Init() 
    {
        animator.Play("Active");
        col2D.enabled = true;
        attentionMark.EffectSwitch(true);
    }
    public void TargetDamage()
    {
        animator.Play("Deactive");
        col2D.enabled = false;
        isDamaged = true;
        attentionMark.EffectSwitch(false);
        damage.CountDown();
        effectGenerator.GenerateTargetDestroyEffect(this.transform.position);
    }

    public void PauseSwitch(bool ispause)
    {
        if (!isDamaged) { col2D.enabled = !ispause; }
        attentionMark.PauseSwitch(ispause);
    }
}
