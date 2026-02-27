using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵の吹っ飛び攻撃判定
public class EnemyHittedAttack : MonoBehaviour
{
    private int enemyTag = "Enemy".GetHashCode();

    [SerializeField] private int power = 1;

    private bool isPause = false;
    private List<GameObject> enemyList = new List<GameObject>();

    void Awake()
    {
        isPause = false;
        enemyList.Clear();
    }

    public void PauseSwitch(bool ispause)
    {
        isPause = ispause;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!enemyList.Contains(other.gameObject) && !isPause && other.gameObject.tag.GetHashCode() == enemyTag)
        {
            enemyList.Add(other.gameObject);

            //ヒットフラグ立てたのちダメージ処理。
            IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
            if (idamageable.CanDamage()) { idamageable?.Damage(power); }
        }
    }
}
