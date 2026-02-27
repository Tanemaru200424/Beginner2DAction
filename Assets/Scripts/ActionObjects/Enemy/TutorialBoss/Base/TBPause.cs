using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBPause : MonoBehaviour, IPausable
{
    private TBState state = null;
    private TBMove move = null;
    private TBAttack attack = null;
    private TBController controller = null;
    [SerializeField] private TBAnimation tbAnimation = null;
    [SerializeField] private TBDamage damage = null;

    void Awake()
    {
        state = GetComponent<TBState>();
        move = GetComponent<TBMove>();
        attack = GetComponent<TBAttack>();
        controller = GetComponent<TBController>();
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