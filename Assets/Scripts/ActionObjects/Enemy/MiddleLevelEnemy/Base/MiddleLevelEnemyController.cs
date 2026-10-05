using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�C���G�l�~�[�̐���X�N���v�g
public class MiddleLevelEnemyController : MonoBehaviour
{
    private MiddleLevelEnemyState state = null;
    private MiddleLevelEnemyAttack attack = null;
    private MiddleLevelEnemyMove move = null;
    private MiddleLevelEnemyEvents events = null;


    void Awake()
    {
        state = GetComponent<MiddleLevelEnemyState>();
        attack = GetComponent<MiddleLevelEnemyAttack>();
        move = GetComponent<MiddleLevelEnemyMove>();
        events = GetComponent<MiddleLevelEnemyEvents>();
    }

    void Update()
    {
        if (state.IsHiited() && move.IsHittedLimmit())
        {
            events.HittedEnd();
        }

        if (move.IsReachAttackStartPos())
        {
            if (attack.CanAttack()) { attack.AttackStart(); }
            move.ChangeNextMoveTarget();
        }
    }

    public void PauseSwitch(bool ispause)
    {
        this.enabled = !ispause;
    }
}
