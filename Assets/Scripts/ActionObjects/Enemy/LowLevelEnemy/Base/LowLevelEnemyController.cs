using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowLevelEnemyController : MonoBehaviour
{
    private LowLevelEnemyState state = null;
    private LowLevelEnemyMove move = null;
    private LowLevelEnemyEvents events = null;


    void Awake()
    {
        state = GetComponent<LowLevelEnemyState>();
        move = GetComponent<LowLevelEnemyMove>();
        events = GetComponent<LowLevelEnemyEvents>();
    }

    void Update()
    {
        if (state.IsHiited() && move.IsHittedLimmit())
        {
           events.HittedEnd();
        }
    }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
    }
}
