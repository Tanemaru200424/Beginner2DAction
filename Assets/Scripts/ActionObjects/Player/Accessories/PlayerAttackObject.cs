using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackObject : MonoBehaviour
{
    private int enemyTag = "Enemy".GetHashCode();
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private PlayerEffectGenerator effectGenerator = null;
    [SerializeField] private int power = 1;
    private float zAngle = 0;

    private bool isPause = false;
    private List<GameObject> enemyList = new List<GameObject>();

    private Animator animator = null;

    void Awake()
    {
        animator = GetComponent<Animator>();
        isPause = false;
        enemyList.Clear();
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

    public void AttackSwitch(bool isactive, float angle)
    {
        if(isactive)
        {
            enemyList.Clear();
            zAngle = angle;
            this.gameObject.SetActive(true);
            if (animator == null) { animator = GetComponent<Animator>(); }
            animator.Play("Attack");
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
