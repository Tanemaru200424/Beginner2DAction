using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleLevelEnemyPause : MonoBehaviour, IPausable
{
    private MiddleLevelEnemyController controller = null;
    private MiddleLevelEnemyAttack attack = null;
    private MiddleLevelEnemyMove move = null;
    private MiddleLevelEnemyState state = null;
    [SerializeField] private MiddleLevelEnemyDamage damage = null;
    [SerializeField] private EnemyBodyAttack bodyAttack = null;
    [SerializeField] private EnemyHittedAttack hittedAttack = null;

    [SerializeField] private MiddleLevelEnemyAnimation mleAnimation = null;

    void Awake()
    {
        controller = GetComponent<MiddleLevelEnemyController>();
        attack = GetComponent<MiddleLevelEnemyAttack>();
        move = GetComponent<MiddleLevelEnemyMove>();
        state = GetComponent<MiddleLevelEnemyState>();
    }

    public void Paused()
    {
        controller.PauseSwitch(true);
        attack.PauseSwitch(true);
        move.PauseSwitch(true);
        state.PauseSwitch(true);

        damage.PauseSwitch(true);
        bodyAttack.PauseSwitch(true);
        hittedAttack.PauseSwitch(true);
        mleAnimation.PauseSwitch(true);
    }
    public void Resumed()
    {
        controller.PauseSwitch(false);
        attack.PauseSwitch(false);
        move.PauseSwitch(false);
        state.PauseSwitch(false);

        damage.PauseSwitch(false);
        bodyAttack.PauseSwitch(false);
        hittedAttack.PauseSwitch(false);
        mleAnimation.PauseSwitch(false);
    }
}