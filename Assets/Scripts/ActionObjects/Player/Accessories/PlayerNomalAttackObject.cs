using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNomalAttackObject : MonoBehaviour
{
    private int enemyTag = "Enemy".GetHashCode();

    [SerializeField] private PlayerEffectGenerator effectGenerator = null;
    [SerializeField] private int power = 1;

    private bool isPause = false;
    private List<GameObject> enemyList = new List<GameObject>();

    private Animator animator = null;

    public void Init()
    {
        animator = GetComponent<Animator>();
        isPause = false;
        AttackSwitch(false);
    }

    public void PauseSwitch(bool ispause)
    {
        isPause = ispause;
        if (ispause) { animator.speed = 0; }
        else { animator.speed = 1; }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!enemyList.Contains(other.gameObject) && !isPause && other.gameObject.tag.GetHashCode() == enemyTag)
        {
            enemyList.Add(other.gameObject);

            //ヒットフラグ立てたのちダメージ処理。
            IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
            if (idamageable != null && idamageable.CanDamage()) 
            { 
                idamageable.Damage(power);
                effectGenerator.GenerateNomalHitEffect(other.gameObject.transform.position);
            }
        }
    }

    public void AttackSwitch(bool isactive)
    {
        if (isactive)
        {
            enemyList.Clear();
            this.gameObject.SetActive(true);
            animator.Play("Attack");
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
