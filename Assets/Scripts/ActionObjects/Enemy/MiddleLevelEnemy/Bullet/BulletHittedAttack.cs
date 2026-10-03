using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHittedAttack : MonoBehaviour
{
    [SerializeField] private BulletEffectsGenerator effectsGenerator = null;
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

    void OnTriggerStay2D(Collider2D other)
    {
        IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
        if (!isPause &&
            !enemyList.Contains(other.gameObject) &&
            other.gameObject.tag.GetHashCode() == enemyTag &&
            idamageable != null &&
            idamageable.CanDamage())
        {
            enemyList.Add(other.gameObject);
            effectsGenerator.GenerateHitEffect(other.gameObject.transform.position);
            idamageable = other.gameObject.GetComponent<IDamageable>();
            idamageable?.Damage(power);
        }
    }
}
