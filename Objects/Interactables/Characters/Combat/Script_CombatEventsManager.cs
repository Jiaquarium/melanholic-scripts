﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CombatEventsManager : MonoBehaviour
{
    public delegate void EnemyAttackEndDelegate(string hitBoxId);
    public static event EnemyAttackEndDelegate OnEnemyAttackEnd;
    public static void EnemyAttackEnd(string hitBoxId)
    {
        if (OnEnemyAttackEnd != null)   OnEnemyAttackEnd(hitBoxId);
    }

    public delegate void CombinedSpikeAttackEndDelegate();
    public static event CombinedSpikeAttackEndDelegate OnCombinedSpikeAttackEnd;
    public static void CombinedSpikeAttackEnd()
    {
        if (OnCombinedSpikeAttackEnd != null)
            OnCombinedSpikeAttackEnd();
    }

    // - Combined Spikes Timeline (Spike Room)
    public void CombinedSpikeAttackEndEvent()
    {
        CombinedSpikeAttackEnd();
    }
}
