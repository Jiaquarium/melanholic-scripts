using System.Collections;
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

    public delegate void OnHitCancelUIDelegate(Script_HitBox hitBox, Script_HitBoxBehavior hitBoxBehavior);
    public static event OnHitCancelUIDelegate OnHitCancelUI;
    public static void HitCancelUI(Script_HitBox hitBox, Script_HitBoxBehavior hitBoxBehavior)
    {
        if (OnHitCancelUI != null)
            OnHitCancelUI(hitBox, hitBoxBehavior);
    }    
}
