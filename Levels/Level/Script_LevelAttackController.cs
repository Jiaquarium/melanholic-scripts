using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_LevelBehavior))]
public class Script_LevelAttackController : MonoBehaviour
{
    // Must be >1 because because the spike animation lasts 1 sec (30 frames)
    // 1.9s is very tight 8 moves; 1.95s is reasonable
    // 23 frames at 30 fps, the hitbox is exposed (0.7666 sec) meaning 1.95 - 0.7666 = 1.1833
    // So Player has 1.1833s to move 8 spaces (with default speed 8 spaces takes 1.0666 sec)
    // They have .11666 margin for error (~7 frames)
    [Range(1.80f, 2.00f)][SerializeField] private float attackInterval;
    
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
