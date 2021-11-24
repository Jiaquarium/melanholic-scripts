using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Script_CharacterStats))]
[RequireComponent(typeof(AudioSource))]
public class Script_Attack : MonoBehaviour, IHitBoxResponder
{
    public string hitBoxId;
    [SerializeField] protected Script_HitBox activeHitBox;
    [SerializeField] protected Script_HitBox hitBoxN;
    [SerializeField] protected Script_HitBox hitBoxE;
    [SerializeField] protected Script_HitBox hitBoxS;
    [SerializeField] protected Script_HitBox hitBoxW;
    [SerializeField] protected Script_CharacterStats characterStats;
    [SerializeField] protected Script_HitBoxBehavior hitBoxBehavior;
    
    
    // TODO: actually don't implement this here bc params change depending on what hitbox needs
    public virtual void Attack(Directions dir)
    {
        activeHitBox = GetHitBoxDirection(dir);
        
        activeHitBox.SetResponder(this);
        activeHitBox.StartCheckingCollision();
    }

    protected Script_HitBox GetHitBoxDirection(Directions dir)
    {
        if (dir == Directions.Up)            return hitBoxN;
        else if (dir == Directions.Right)    return hitBoxE;
        else if (dir == Directions.Down)     return hitBoxS;
        else                                 return hitBoxW;
    }

    public virtual void EndAttack()
    {
        activeHitBox.StopCheckingCollision();

        Action onAttackDone = transform.GetParentRecursive<Script_Player>().onAttackDone;
        if (onAttackDone != null)
        {
            Debug.Log("Player doing Action given by an attacked HurtBox");
            onAttackDone();
            transform.GetParentRecursive<Script_Player>().onAttackDone = null;
        }
    }
    
    /// <summary>
    /// via the interface, this handles the hurtbox colliding
    /// </summary>
    /// <param name="collider">the hurtbox</param>
    public virtual void CollisionedWith(Collider collider, Script_HitBox hitBox) {
        // Hurtbox hurtbox = collider.GetComponent<Hurtbox>();
        // hurtbox?.getHitBy(damage);
    }

    protected virtual void HitSFX() {}

    public virtual Model_Stat GetAttackStat()
    {
        return characterStats.stats.attack;
    }
}
