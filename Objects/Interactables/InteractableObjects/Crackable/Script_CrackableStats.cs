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
    [SerializeField] private bool isIcePersists;
    [SerializeField] private bool isIceMeltsHide;
    [SerializeField] private Script_IceMeltController iceMeltController;

    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite lowHealthImage;
    [SerializeField] private int lowHealthThreshold;
    
    [SerializeField] private SpriteRenderer graphics;

    [SerializeField] private PlayableDirector crackingDirector;

    [SerializeField] private List<GameObject> visibleChildren;

    [SerializeField] private Script_Shatter shatter;

    [SerializeField] private float shakeTime; 
    [SerializeField] private float shakeAmp; 
    [SerializeField] private float shakeFreq;

    public bool IsCracked { get => currentHp <= 0; }
    public PlayableDirector CrackingDirector => crackingDirector;

    private Coroutine hideIceCoroutine;
    
    /// <summary>
    /// Flag if Player left scene before Timeline hides crackable,
    /// just manually hide the ice.
    /// </summary>
    protected bool isHideOnDisable;
    private bool isScreenShakeShatter;
    
    protected UnityEvent<Script_CrackableStats> CrackedAction
    {
        get => crackedAction;
    }

    public bool IsIcePersists
    {
        get => isIcePersists;
        set => isIcePersists = value;
    }

    void OnDisable()
    {
        isScreenShakeShatter = false;
        
        if (hideIceCoroutine != null)
        {
            StopCoroutine(hideIceCoroutine);
            hideIceCoroutine = null;
            HideIce(isOnDisable: true);
        }
        // This ensures consistent behavior, since after Ice Melt is done, the crackable will still be active.
        // Disable the Crackable when leaving the map.
        else if (
            isIceMeltsHide
            && iceMeltController != null
            && iceMeltController.DidStartMelt
        )
        {
            HideIce(isOnDisable: true);
        }

        if (isHideOnDisable)
        {
            HideIce(isOnDisable: true);
            isHideOnDisable = false;
        }
    }
    
    public override int Hurt(int dmg, Script_HitBox hitBox, Script_HitBoxBehavior hitBoxBehavior)
    {
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

        // reduce health
        currentHp -= dmg;
        currentHp = Mathf.Clamp(currentHp, 0, int.MaxValue);
        
        Dev_Logger.Debug($"{transform.name} took damage {dmg}. currentHp: {currentHp}");
        
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

    protected virtual void HideIce(bool isOnDisable = false)
    {
        Dev_Logger.Debug($"{name} HideIce, isIcePersists: {isIcePersists}");
        if (!isIcePersists)
        {
            // On Disable should immediately hide ice
            if (isIceMeltsHide && !isOnDisable)
                iceMeltController.StartMelt();
            else
                gameObject.SetActive(false);
        }
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
        Script_InteractableObjectEventsManager.InteractAfterShatter(this);
    }
    
    // Default to be called at end of Timeline Shatter.
    public void OnIceBlockDiagonalCutShatterTimelineDone(Script_CrackableStats ice)
    {
        Dev_Logger.Debug($"OnIceBlockCrackingTimelineDone ice <{ice}>");
        
        Script_InteractableObjectEventsManager.IceCrackingTimelineDone(this);

        // Hide Ice Remains.
        HideIce();
    }
    
    // Called from Myne Lair Ice Variant.
    // This will signal Level Behavior to move onto the next 
    public void OnMynesLairShatterTimelineDone(Script_CrackableStats ice)
    {
        Dev_Logger.Debug($"OnIceBlockCrackingTimelineDone ice <{ice}>");
        
        Script_InteractableObjectEventsManager.IceCrackingTimelineDone(this);

        // Leave ice remains for as long as Timeline is active.
        isHideOnDisable = true;
    }

    public void OnUnfreezeEffect(Script_CrackableStats ice)
    {
        Dev_Logger.Debug($"OnUnfreezeEffect <{ice}>");
        
        Script_InteractableObjectEventsManager.UnfreezeEffect(this);
    }

    public void DiagonalCut()
    {
        shatter?.DiagonalCut();
        
        // DiagonalCut should always be followed by a Shatter,
        // which will shake screen. If immediate Shatter, IceSpikeEffect
        // will handle screen shake.
        isScreenShakeShatter = true;

        Script_InteractableObjectEventsManager.DiagonalCut(this);
    }

    public void Shatter()
    {
        Script_InteractableObjectEventsManager.Shatter(this);
        
        shatter?.Shatter();
        
        if (isScreenShakeShatter)
        {
            Script_VCamManager.VCamMain.GetComponent<Script_CameraShake>().Shake(
                shakeTime,
                shakeAmp,
                shakeFreq,
                null
            );
            isScreenShakeShatter = false;
        }

        // Note: This SFX is playing at the same time as Spike SFX.
        Script_SFXManager.SFX.PlayIceShatter();
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
