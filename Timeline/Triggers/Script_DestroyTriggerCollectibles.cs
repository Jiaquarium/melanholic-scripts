using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// helps in timeline to destroy objects on trigger
/// </summary>
public class Script_DestroyTriggerCollectibles : MonoBehaviour
{
    [SerializeField] private Script_CollectibleTriggerStay trigger;

    void Start()
    {
        Script_CollectibleObject[] collectibles = new Script_CollectibleObject[trigger.collectibles.Count];

        // must place in static lenghted array to Destroy reliably        
        for (int i = 0; i < collectibles.Length; i++)
            collectibles[i] = trigger.collectibles[i];

        foreach (Script_CollectibleObject collectible in collectibles)
        {
            collectible.gameObject.SetActive(false);
            Destroy(collectible.gameObject);
        }
    }
}
