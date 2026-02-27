using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEShooterAnimationEvents : MonoBehaviour
{
    [SerializeField] private SEController controller = null;
    [SerializeField] private SEAttack attack = null;

    public void Attack()
    {
        attack.Attack();
    }
    public void AttackEnd()
    {
        controller.AttackEnd();
    }
}
