using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_LevelBehavior_49 : Script_LevelBehavior
{
    /* =======================================================================
        STATE DATA
    ======================================================================= */
    
    public bool didActivateDoubts;

    /* ======================================================================= */
    
    [SerializeField] private Script_LevelAttackController attackController;

    [SerializeField] private float doubtsFadeOutTime;

    [SerializeField] private bool[] doubtsActivationStates = new bool[3]{false, false, false};
    private Coroutine[] fadeOutDoubts = new Coroutine[3];
    
    protected override void OnDisable()
    {
        for (var i = 0; i < fadeOutDoubts.Length; i++)
        {
            if (fadeOutDoubts[i] != null)
                StopCoroutine(fadeOutDoubts[i]);
            fadeOutDoubts[i] = null;
        }

        Script_TeletypeNotificationManager.Control.InitialState();
    }
    
    protected override void Update()
    {
        attackController.AttackTimer(false);
    }

    public void ShowDoubts(int i)
    {
        string puppeteerId = Const_Items.PuppeteerId;
        int slot;

        Debug.Log($"puppeteer in stickers {game.GetItemsStickerItem(puppeteerId, out slot) != null}");
        Debug.Log($"puppeteer in equipment {game.CheckStickerEquippedById(puppeteerId)}");
        
        bool hasPuppeteerMask = game.GetItemsStickerItem(puppeteerId, out slot) != null
            || game.CheckStickerEquippedById(puppeteerId);
        
        if (didActivateDoubts || doubtsActivationStates[i] || !hasPuppeteerMask)
            return;

        Script_TeletypeNotificationManager.Control.ShowCatWalk2Dialogue(i);

        fadeOutDoubts[i] = StartCoroutine(WaitFadeOutDoubt(i));

        doubtsActivationStates[i] = true;
        
        // If all states are true, save in state.
        if (doubtsActivationStates.All(x => x))
            didActivateDoubts = true;

        IEnumerator WaitFadeOutDoubt(int i)
        {
            yield return new WaitForSeconds(doubtsFadeOutTime);

            Script_TeletypeNotificationManager.Control.FadeOutCatWalk2Dialogue(i);
        }
    }
}