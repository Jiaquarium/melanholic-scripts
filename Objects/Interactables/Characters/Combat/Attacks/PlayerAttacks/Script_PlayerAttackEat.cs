using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// hits only 1 enemy at a time
/// return on CollisionedWith after 1 hit and hitbox is hidden
/// </summary>
public class Script_PlayerAttackEat : Script_Attack
{
    [SerializeField] private float eatingTime;
    [SerializeField] private AudioClip crunchSFX;
    [SerializeField] private Script_Player player;
    [SerializeField] private Animator animator;

    private bool didHit;
    private Coroutine eatingCoroutine;
    private Script_AudioOneShotSource audioOneShotSource;

    public void Eat(Directions direction)
    {
        // disallow any more action inputs
        player.SetIsAttacking();
        
        // play eating animation
        player.AnimatorEffectHold = true;

        didHit = false;
        
        base.Attack(direction);
        eatingCoroutine = StartCoroutine(EndEating());
    }

    IEnumerator EndEating()
    {
        yield return new WaitForSeconds(eatingTime);

        player.AnimatorEffectHold = false;
        
        player.SetLastState();
        
        base.EndAttack();
    }

    public override void CollisionedWith(Collider collider, Script_HitBox hitBox)
    {
        // only allow one hit for Eat
        if (didHit)
        { 
            activeHitBox.StopCheckingCollision();
            return;
        }
        
        Script_HurtBox hurtBox = collider.GetComponent<Script_HurtBox>();
        
        if (hurtBox != null)
        {
            int dmg = GetAttackStat().GetVal();
            Dev_Logger.Debug($"CollisionedWith with {hurtBox.gameObject.name} inflicting dmg: {dmg}");

            int dmgActuallyGiven = hurtBox.Hurt(dmg, hitBox);
            if (dmgActuallyGiven > 0) 
                HitSFX();

            didHit = true;
        }
    }

    protected override void HitSFX()
    {
        // TODO: don't instantiate this, use audio mixer
        audioOneShotSource = Script_Game.Game.CreateAudioOneShotSource(transform.position);
        audioOneShotSource.Setup(crunchSFX);
        audioOneShotSource.PlayOneShot();
    }

    public override Model_Stat GetAttackStat()
    {
        return characterStats.stats.attack;
    }
}
