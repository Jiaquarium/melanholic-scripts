using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles life stats, taking damage
/// </summary>
public class Script_CharacterStats : MonoBehaviour
{
    public Model_CharacterStats stats;
    [SerializeField] protected int currentHp;

    void Awake()
    {
        InitialState();
    }
    
    public virtual int Hurt(int dmg, Script_HitBox hitBox)
    {
        // reduce dmg by defense
        dmg -= stats.defense.GetVal();
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

        // reduce health
        currentHp -= dmg;
        currentHp = Mathf.Clamp(currentHp, 0, int.MaxValue);
        Debug.Log($"{transform.name} took damage {dmg}. currentHp: {currentHp}");

        if (currentHp == 0)
        {
            Die(Script_GameOverController.DeathTypes.Default);
        }

        return dmg;
    }

    public virtual int Heal(int healHp)
    {
        int maxHpToHeal = stats.maxHp.GetVal() - currentHp;
        int hpActuallyHealed = Mathf.Min(healHp, maxHpToHeal);
        currentHp += hpActuallyHealed;

        Debug.Log($"Heal healed {hpActuallyHealed}");

        return hpActuallyHealed;
    }

    public virtual int FullHeal()
    {
        int maxHpToHeal = stats.maxHp.GetVal() - currentHp;
        
        Debug.Log($"Full Heal heals {maxHpToHeal}");

        return Heal(maxHpToHeal);
    }

    /// <summary>
    /// override this for custom way to die e.g. cutscene, drop item, etc.
    /// </summary>
    protected virtual void Die(Script_GameOverController.DeathTypes deathType) { }

    public void InitialState()
    {
        currentHp = stats.maxHp.GetVal();
    }

    public void Setup() {}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_CharacterStats))]
public class Script_StatsTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_CharacterStats stats = (Script_CharacterStats)target;
        if (GUILayout.Button("InitialState()"))
        {
            stats.InitialState();
        }
        if (GUILayout.Button("Hurt(1)"))
        {
            stats.Hurt(1, null);
        }
    }
}
#endif
