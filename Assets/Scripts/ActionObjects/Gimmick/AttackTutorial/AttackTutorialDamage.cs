using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorialDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject rootObject = null;
    [SerializeField] private AttackTutorialEffectGenerator effectGenerator = null;
    [SerializeField] private int maxHp = 3;
    [SerializeField] private AccessoriesLoopEffect attentionMark = null;
    private int nowHp = 0;
    private bool isPause = false;

    void Awake() 
    {
        nowHp = maxHp;
        attentionMark.Init();
        attentionMark.EffectSwitch(true);
    }

    public void PauseSwitch(bool ispause) 
    { 
        isPause = ispause;
        attentionMark.PauseSwitch(isPause);
        this.enabled = !ispause;
    }

    public bool CanDamage() { return !isPause; }
    public void Damage(int value) 
    {
        nowHp -= value;
        if (nowHp <= 0) { Dead(); }
    }
    public void FatalDamage() { Damage(nowHp); }
    public void Dead() 
    {
        effectGenerator.GenerateDestroyEffect();
        attentionMark.EffectSwitch(false);
        Destroy(rootObject); 
    }
}
