using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyAttack : MonoBehaviour
{
    private int playerTag = "Player".GetHashCode();

    [SerializeField] private int power = 1;

    private bool isPause = false;

    void Awake()
    {
        isPause = false;
    }

    public void PauseSwitch(bool ispause)
    {
        isPause = ispause;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ( !isPause && other.gameObject.tag.GetHashCode() == playerTag)
        {
            //ヒットフラグ立てたのちダメージ処理。
            IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
            if (idamageable.CanDamage()) { idamageable?.Damage(power); }
        }
    }
}
