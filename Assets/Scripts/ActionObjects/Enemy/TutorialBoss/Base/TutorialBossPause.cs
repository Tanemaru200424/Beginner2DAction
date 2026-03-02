using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossPause : MonoBehaviour, IPausable
{
    private TutorialBossState state = null;
    private TutorialBossMove move = null;
    private TutorialBossAttack attack = null;
    private TutorialBossController controller = null;
    [SerializeField] private TutorialBossAnimation tbAnimation = null;
    [SerializeField] private TutorialBossDamage damage = null;

    void Awake()
    {
        state = GetComponent<TutorialBossState>();
        move = GetComponent<TutorialBossMove>();
        attack = GetComponent<TutorialBossAttack>();
        controller = GetComponent<TutorialBossController>();
    }

    public void Paused()
    {
        damage.PauseSwitch(true);
        state.PauseSwitch(true);
        move.PauseSwitch(true);
        attack.PauseSwitch(true);
        controller.PauseSwitch(true);
        tbAnimation.PauseSwitch(true);
    }
    public void Resumed()
    {
        damage.PauseSwitch(false);
        state.PauseSwitch(false);
        move.PauseSwitch(false);
        attack.PauseSwitch(false);
        controller.PauseSwitch(false);
        tbAnimation.PauseSwitch(false);
    }
}