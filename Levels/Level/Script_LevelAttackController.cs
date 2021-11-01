using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_LevelBehavior))]
public class Script_LevelAttackController : MonoBehaviour
{
    // Must be >1 because because the spike animation lasts 1 sec (30 frames)
    // 1.9s allows for a very tight 7 moves.
    // E.g. time of 2s gives 1s of movement time, 1s of animation where hitbox is exposed.
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
        timer -= Time.smoothDeltaTime;

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
