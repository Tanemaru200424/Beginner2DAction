using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEBodyAttack : MonoBehaviour
{
    [SerializeField] private WEAccessoriesGenerator accessoriesGenerator = null;
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
        IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
        if (!isPause && 
            other.gameObject.tag.GetHashCode() == playerTag &&
            idamageable != null &&
            idamageable.CanDamage())
        {
            accessoriesGenerator.GenerateHitEffect(other.gameObject.transform.position);
            idamageable.Damage(power);
        }
    }
}
