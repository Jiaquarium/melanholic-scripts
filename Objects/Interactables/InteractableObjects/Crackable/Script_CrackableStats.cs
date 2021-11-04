using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_CrackableStats : Script_CharacterStats
{
    [SerializeField] private UnityEvent<Script_CrackableStats> crackedAction;
    
    [SerializeField] private float defaultHideRemainsTime;

    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite lowHealthImage;
    [SerializeField] private int lowHealthThreshold;
    
    [SerializeField] private SpriteRenderer graphics;

    [SerializeField] private PlayableDirector crackingDirector;

    [SerializeField] private List<GameObject> visibleChildren;

    [SerializeField] private Script_Shatter shatter;

    private Coroutine hideIceCoroutine;
    private bool isHideOnDisable;
    
    protected UnityEvent<Script_CrackableStats> CrackedAction
    {
        get => crackedAction;
    }

    void OnDisable()
    {
        if (hideIceCoroutine != null)
        {
            StopCoroutine(hideIceCoroutine);
            hideIceCoroutine = null;
            HideIce();
        }

        if (isHideOnDisable)
        {
            HideIce();
            isHideOnDisable = false;
        }
    }
    
    public override int Hurt(int dmg, Script_HitBox hitBox)
    {
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

        // reduce health
        currentHp -= dmg;
        currentHp = Mathf.Clamp(currentHp, 0, int.MaxValue);
        
        Debug.Log($"{transform.name} took damage {dmg}. currentHp: {currentHp}");
        
        if (currentHp == 0)
        {
            CrackedAction.SafeInvokeDynamic(this);

            // If there is a Timeline for the Cracking, play that instead.
            // Default is shattering immediately.
            if (crackingDirector != null)
            {
                Script_Game.Game.ChangeStateCutScene();
                crackingDirector.Play();
            }
            else
            {
                Die(Script_GameOverController.DeathTypes.Default);
            }
        }
        else
        {
            // Show a different spirte depending on how hurt the Interactable is
            HandleHurtGraphics(currentHp);
        }

        return dmg;
    }
    
    protected override void Die(Script_GameOverController.DeathTypes deathType)
    {
        Shatter();
        WaitToHideAfterShatter();
    }

    protected void WaitToHideAfterShatter()
    {
        hideIceCoroutine = StartCoroutine(WaitToHide());
        
        IEnumerator WaitToHide()
        {
            yield return new WaitForSeconds(defaultHideRemainsTime);

            hideIceCoroutine = null;
            HideIce();
        }
    }

    private void HideIce()
    {
        Debug.Log($"{name} HideIce");
        gameObject.SetActive(false);
    }

    private void HandleHurtGraphics(int hp)
    {
        if (graphics == null)   return;
        
        if      (hp <= lowHealthThreshold)      graphics.sprite = lowHealthImage;
        else                                    graphics.sprite = defaultImage;
    }

    // ------------------------------------------------------------------
    // Timeline Signals
    public void DieTimeline()
    {
        // Only turn off graphics so we can still play the rest of Timeline.
        foreach (var child in visibleChildren)
            child.SetActive(false);
    }

    // Called from Diagonal Cut Shatter Timeline so player can move around before the ice fully vanishes.
    public void AllowPlayerInteract()
    {
        Script_Game.Game.ChangeStateInteract();   
    }
    
    // Default to be called at end of Timeline Shatter.
    public void OnIceBlockDiagonalCutShatterTimelineDone(Script_CrackableStats ice)
    {
        Debug.Log($"OnIceBlockCrackingTimelineDone ice <{ice}>");
        
        Script_InteractableObjectEventsManager.IceCrackingTimelineDone(this);

        // Hide Ice Remains.
        HideIce();
    }
    
    // Called from Myne Lair Ice Variant.
    // This will signal Level Behavior to move onto the next 
    public void OnMynesLairShatterTimelineDone(Script_CrackableStats ice)
    {
        Debug.Log($"OnIceBlockCrackingTimelineDone ice <{ice}>");
        
        Script_InteractableObjectEventsManager.IceCrackingTimelineDone(this);

        // Leave ice remains for as long as Timeline is active.
        isHideOnDisable = true;
    }

    public void DiagonalCut()
    {
        shatter?.DiagonalCut();
    }

    public void Shatter()
    {
        shatter?.Shatter();
    }
    
    // ------------------------------------------------------------------

    #if UNITY_EDITOR
    [CustomEditor(typeof(Script_CrackableStats))]
    public class Script_CrackableStatsTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_CrackableStats t = (Script_CrackableStats)target;
            if (GUILayout.Button("Shatter"))
            {
                t.Shatter();
            }
        }
    }
    #endif
}
