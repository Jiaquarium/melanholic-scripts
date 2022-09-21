using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Script_LevelBehavior_11 : Script_LevelBehavior
{
    public Script_SavePoint sp;
    public bool isInitialize = true;
    [SerializeField] private float waitTimeToLockSFX;
    [SerializeField] private float doorLockTime;
    [SerializeField] private Script_LevelBehavior_10 LB10;

    protected override void OnEnable()
    {
        Dev_Logger.Debug($"LastLevelBehavior: {game.LastLevelBehavior}");
        if (game.LastLevelBehavior == LB10)
        {
            Dev_Logger.Debug("Player coming from LB10_IdsRoom");
            // Pause bg music until after
            game.PauseBgMusic();
            Script_GameEventsManager.OnLevelInitComplete += DoorLock;
        }
    }

    protected override void OnDisable()
    {
        if (game.LastLevelBehavior == LB10)
        {
            Dev_Logger.Debug("Player came from LB10_IdsRoom, removing event Handler");
            Script_GameEventsManager.OnLevelInitComplete -= DoorLock;
        }
    }
    
    private void DoorLock()
    {
        game.ChangeStateCutScene();
        StartCoroutine(WaitToDoorLock());

        IEnumerator WaitToDoorLock()
        {
            yield return new WaitForSeconds(waitTimeToLockSFX);
            GetComponent<AudioSource>().PlayOneShot(
                Script_SFXManager.SFX.doorLock, Script_SFXManager.SFX.doorLockVol
            );
            yield return new WaitForSeconds(doorLockTime);
            game.ChangeStateInteract();
            game.UnPauseBgMusic();
        }
    }

    /// <summary>
    /// NextNode action
    /// </summary>
    public void UpdateTedwich()
    {
        Script_Names.UpdateTedwich();
    }
    
    public override void Setup()
    {
        game.GetPlayer().SetInvisible(false);
        game.SetupSavePoint(sp, isInitialize);
        isInitialize = false;
    }
}
