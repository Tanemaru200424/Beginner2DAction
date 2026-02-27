using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedTutorialObjectHittedAttack : MonoBehaviour
{
    private int enemyTag = "Enemy".GetHashCode();

    private bool isPause = false;
    private List<GameObject> enemyList = new List<GameObject>();

    private Animator animator = null;
    private Collider2D col2D = null;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
        isPause = false;
        AttackSwitch(false);
    }

    public void PauseSwitch(bool ispause)
    {
        isPause = ispause;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!enemyList.Contains(other.gameObject) && !isPause && other.gameObject.tag.GetHashCode() == enemyTag)
        {
            enemyList.Add(other.gameObject);

            HittedTutorialTarget target = other.gameObject.GetComponent<HittedTutorialTarget>();
            if (target != null) 
            { 
                target.TargetDamage();
            }
        }
    }

    public bool IsHitSomething() { return enemyList.Count > 0;}

    public void AttackSwitch(bool isactive)
    {
        if (isactive)
        {
            enemyList.Clear();
            col2D.enabled = true;
        }
        else
        {
            col2D.enabled = false;
        }
    }
}
