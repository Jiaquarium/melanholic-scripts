using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// NOTE: Keep max HP equal to number of thought Slots
/// </summary>
[RequireComponent(typeof(Script_Player))]
public class Script_PlayerStats : Script_CharacterStats
{
    public string defaultHitBoxMessage;
    
    /// TODO: REMOVE ISSWALLEDDEMON, give demons hitboxes and they pass it in
    public override int Hurt(int dmg, Script_HitBox hitBox)
    {
        if (GetComponent<Script_Player>().isInvincible)     return 0;

        // reduce dmg by defense
        dmg -= stats.defense.GetVal();
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);
        
        /// TODO: FILL IN MULTIPLE THOUGHTS FOR >1 DMG
        int heartsToFill =      HandleTakeDamage(dmg);
        if (heartsToFill > 0)   HandleAddThought(hitBox == null ? null : hitBox.Id);

        Debug.Log($"{transform.name} took damage {dmg}. currentHp: {currentHp}. death type: {hitBox.deathType}");

        if (currentHp == 0)     Die(hitBox.deathType);
        return dmg;
    }

    public int HurtThoughts(int dmg, Model_Thought thought)
    {
        if (GetComponent<Script_Player>().isInvincible)     return 0;

        // reduce dmg by defense
        dmg -= stats.defense.GetVal();
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

        /// TODO: FILL IN MULTIPLE THOUGHTS FOR >1 DMG
        int heartsToFill =      HandleTakeDamage(dmg);
        if (heartsToFill > 0)
        {
            Script_Game.Game.AddPlayerThought(thought);
            Script_Game.Game.ShowAndCloseThought(thought);
        }

        Debug.Log($"{transform.name} took damage {dmg} from thought. currentHp: {currentHp}");

        if (currentHp == 0)     Die(Script_GameOverController.DeathTypes.ThoughtsOverload);
        return dmg;
    }

    /// <summary>
    /// Reduce current hp
    /// Fill appropriate number of hearts
    /// Returns actual number of hearts able to fill
    /// </summary>
    private int HandleTakeDamage(int dmgAfterDef)
    {
        // reduce health
        int lastHp = currentHp;
        currentHp -= dmgAfterDef;
        currentHp = Mathf.Clamp(currentHp, 0, int.MaxValue);
        
        int heartsToFill = Mathf.Clamp(lastHp - currentHp, 0, int.MaxValue);
        Script_Game.Game.healthManager.RemoveHearts(heartsToFill);

        return heartsToFill;
    }

    public override int Heal(int healHp)
    {
        int maxHpToHeal = stats.maxHp.GetVal() - currentHp;
        int hpActuallyHealed = Mathf.Min(healHp, maxHpToHeal);
        currentHp += hpActuallyHealed;

        Script_Game.Game.healthManager.FillHearts(hpActuallyHealed);

        for (int i = 0; i < hpActuallyHealed; i++)  HandleRemoveThought();

        return hpActuallyHealed;
    }

    protected override void Die(Script_GameOverController.DeathTypes deathType)
    { 
        Debug.Log($"{transform.name} PLAYER OVERRIDE Die() called");
        Script_Game.Game.DieEffects(deathType);
    }

    private void HandleAddThought(string hitBoxId)
    {
        Script_HitBoxMetadata hitBoxMetadata;
            string hitBoxMessage;
            
            try
            {
                if (
                    Script_HitBoxDictionary.HitBoxDictionary.myDictionary
                        .TryGetValue(hitBoxId, out hitBoxMetadata)
                )
                {
                    hitBoxMessage = hitBoxMetadata.hitBoxMetadata.message;
                }
                else
                {
                    // the key isn't in the dictionary.
                    Debug.LogWarning($"Key:{hitBoxId} is not in dictionary, adding default message");
                    hitBoxMessage = defaultHitBoxMessage;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"HandleAddThought error, adding default message: {e}");
                hitBoxMessage = defaultHitBoxMessage;
            }

            Model_Thought thought = new Model_Thought(System.DateTime.Now, hitBoxMessage);
            Script_Game.Game.AddPlayerThought(thought);
    }

    /// <summary>
    /// Removes one thought FIFO
    /// </summary>
    private void HandleRemoveThought()
    {
        Model_PlayerThoughts thoughts = Script_Game.Game.thoughts;
        
        // remove at last index
        Model_Thought oldestThought = thoughts.uglyThoughts[thoughts.uglyThoughts.Count - 1];
        Script_Game.Game.RemovePlayerThought(oldestThought);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_PlayerStats)), CanEditMultipleObjects]
public class Script_PlayerStatsTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_PlayerStats stats = (Script_PlayerStats)target;
        if (GUILayout.Button("Hurt(1)"))
        {
            stats.Hurt(1, null);
        }

        if (GUILayout.Button("Heal(1)"))
        {
            stats.Heal(1);
        }

        if (GUILayout.Button("FullHeal()"))
        {
            stats.FullHeal();
        }
    }
}
#endif
