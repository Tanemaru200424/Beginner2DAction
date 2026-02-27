using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBodyAttack : MonoBehaviour
{
    //[SerializeField] private BulletEffectsGenerator effectsGenerator = null;
    private int playerTag = "Player".GetHashCode();

    [SerializeField] private int power = 1;

    private bool isPause = false;
    private bool isCollision = false; //çUåÇçœÇ›

    public void PauseSwitch(bool ispause)
    {
        isPause = ispause;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        IDamageable idamageable = other.gameObject.GetComponent<IDamageable>();
        if (!isCollision && 
            !isPause &&
            other.gameObject.tag.GetHashCode() == playerTag &&
            idamageable != null &&
            idamageable.CanDamage())
        {
            //effectsGenerator.GenerateHitEffect(other.gameObject.transform.position);
            idamageable.Damage(power);
            isCollision = true;
        }
    }
}
