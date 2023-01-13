using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_LevelBehavior))]
public class Script_LevelAttackController : MonoBehaviour
{
    // Note: Eileen Spike Room will inject this value on Setup
    [Range(1.80f, 2.50f)][SerializeField] private float attackInterval;
    
    [SerializeField] private Script_UrselkAttacks attacks;

    private float timer;

    public float Timer
    {
        get => timer;
        set => timer = value;
    }

    public float AttackInterval
    {
        get => attackInterval;
        set => attackInterval = value;
    }

    public void AttackTimer(bool isPause)
    {
        if (timer == 0)
            timer = attackInterval;

        // Match Player Movement deltaTime.
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (!isPause)
                Attack();
            
            timer = 0;
        }
    }

    private void Attack()
    {
        attacks.AlternatingSpikesAttack();
    }
}
