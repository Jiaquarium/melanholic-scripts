using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class Script_MelancholyPianoEffect : Script_StickerEffect
{
    [SerializeField] private Script_Player player; 
    
    [SerializeField] private float disabledReactionWaitTime;
    
    private AudioSource audioSource;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();        
    }
    
    public override void Effect()
    {
        Debug.Log($"{name} Effect()");

        Script_Game.Game.ChangeStateCutScene();
        
        // Check Level Behavior to ensure Piano effect is not disabled in current room.
        if (Script_Game.Game.levelBehavior.IsMelancholyPianoDisabled)
        {
            audioSource.PlayOneShot(Script_SFXManager.SFX.piano, Script_SFXManager.SFX.pianoVol);

            StartCoroutine(WaitToReact());
            
            return;
        }

        // Open Piano UI
        Script_PianoManager.Control.SetPianosCanvasGroupActive(true);

        IEnumerator WaitToReact()
        {
            yield return new WaitForSeconds(disabledReactionWaitTime);

            Script_DialogueManager.DialogueManager.DisabledMelancholyPianoReaction();
        }
    }

    protected override void OnEquip()
    {
        player.SetIsLastElevatorEffect();
        
        base.OnEquip();
        OnEquipControllerSynced();
        
        player.StopMovingAnimations();
    }

    protected override void OnUnequip()
    {
        base.OnEquip();
        OnUnequipControllerSynced();

        player.SetIsInteract();
    }

    protected override void OnUnequipSwitch()
    {
        base.OnEquip();

        player.SetIsInteract();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_MelancholyPianoEffect))]
public class Script_MelancholyPianoEffectTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_MelancholyPianoEffect t = (Script_MelancholyPianoEffect)target;
        if (GUILayout.Button("Effect()"))
        {
            t.Effect();
        }
    }
}
#endif