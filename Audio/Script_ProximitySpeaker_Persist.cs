using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    meant to be instantiated through code
*/
public class Script_ProximitySpeaker_Persist : Script_ProximitySpeaker
{
    public int levelToPersistUntil;
    [SerializeField] private int originLevel;
    
    /// <summary>
    /// Note: Defined here to null child OnDisable
    /// </summary>
    protected override void OnDisable()
    {
        Script_AudioConfiguration.Instance.RemoveSpeaker(this);
    }

    protected override void Awake() {
        base.Awake();
        
        originLevel = Script_Game.Game.level;
    }

    protected override void Update()
    {
        if (Script_Game.Game.level == originLevel)
        {
            audioSource.volume = 1f;
            AdjustVolume();
        }
        else if (Script_Game.Game.level < originLevel || Script_Game.Game.level > levelToPersistUntil)
        {
            audioSource.Stop();
            Destroy(this.gameObject);
        }
    }
}
