using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InteractableObjectCreator : MonoBehaviour
{
    public Script_InteractableObject InteractableObjectPrefab;
    public Script_Switch SwitchPrefab;
    public Script_LightSwitch LightSwitchPrefab;
    public Script_InteractableObjectText InteractableObjectTextPrefab;
    public Script_PushablesCreator pushablesCreator;
    
    private Light[] lights;
    private Sprite OnSprite;
    private Sprite OffSprite;


    public float defaultOnIntensity;
    public float defaultOffIntensity;

    public void SetupInteractableObjectsText(
        Transform textObjectParent,
        List<Script_InteractableObject> interactableObjects,
        Vector3 rotationAdjustment,
        Script_DialogueManager dm,
        Script_Player player,
        Vector3 worldOffset,
        bool isInitialize
    )
    {
        for (int i = 0; i < textObjectParent.childCount; i++)
        {
            Script_InteractableObjectText iObj = textObjectParent.GetChild(i)
                .GetComponent<Script_InteractableObjectText>();
            interactableObjects.Add(iObj);
            
            if (isInitialize)
                InitializeTextObject(iObj, interactableObjects, dm, player, worldOffset);
        }

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            Debug.Log("interactable objects count: " + interactableObjects.Count);
        }
    }

    /// <summary>
    /// used when needing to specify particular textObj array instead of a parent
    /// </summary>
    public void SetupInteractableObjectsTextManually(
        Script_InteractableObjectText[] textObjs,
        List<Script_InteractableObject> interactableObjects,
        Vector3 rotationAdjustment,
        Script_DialogueManager dm,
        Script_Player player,
        Vector3 worldOffset,
        bool isInitialize
    )
    {
        for (int i = 0; i < textObjs.Length; i++)
        {
            Script_InteractableObjectText iObj = textObjs[i];
            interactableObjects.Add(iObj);
            
            if (isInitialize)
                InitializeTextObject(iObj, interactableObjects, dm, player, worldOffset);
        }

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            Debug.Log("interactable objects count: " + interactableObjects.Count);
        }
    }

    private void InitializeTextObject(
        Script_InteractableObjectText iObj,
        List<Script_InteractableObject> interactableObjects,
        Script_DialogueManager dialogueManager,
        Script_Player player,
        Vector3 worldOffset
    )
    {
        iObj.SetupDialogueNodeText(dialogueManager, player, worldOffset);
        iObj.Id = interactableObjects.Count - 1;
        
        Script_SortingOrder so = iObj.GetRendererChild().GetComponent<Script_SortingOrder>();
        iObj.Setup(so.enabled, so.sortingOrderIsAxisZ, so.offset);
    }

    public void SetupInteractableFullArt
    (
        Transform fullArtParent,
        List<Script_InteractableObject> interactableObjects,
        Vector3 rotationAdjustment,
        Script_DialogueManager dialogueManager,
        Script_Player player,
        Vector3 worldOffset,
        bool isInitialize   
    )
    {
        for (int i = 0; i < fullArtParent.childCount; i++)
        {
            Script_InteractableFullArt iObj = fullArtParent.GetChild(i)
                .GetComponent<Script_InteractableFullArt>();
            if (iObj == null)   continue;
            interactableObjects.Add(iObj);
            
            print("child is: " + fullArtParent.GetChild(i));
            print("iObj is: " + iObj);
            
            if (isInitialize)
            {
                iObj.SetupDialogueNodeText(dialogueManager, player, worldOffset);
                iObj.Id = interactableObjects.Count - 1;
                
                Script_SortingOrder so = iObj.GetRendererChild().GetComponent<Script_SortingOrder>();
                iObj.Setup(so.enabled, so.sortingOrderIsAxisZ, so.offset);
            }
        }

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            Debug.Log("interactable objects count: " + interactableObjects.Count);
        }
    }

    /// <summary>
    /// Handles ALL switches types here to have a combined switchesState
    /// This currently gives a switches
    /// </summary>
    public bool[] SetupSwitches(
        Transform allSwitchesParent,
        List<Script_InteractableObject> interactableObjects,
        List<Script_Switch> switches,
        Vector3 rotationAdjustment,
        bool[] switchesState,
        bool isInitialize
    )
    {
        /// Initializing level for the first time with no loaded state data
        bool isEmptySwitchesState = false;
        if  (switchesState == null || switchesState.Length == 0)
        {
            switchesState = new bool[allSwitchesParent.childCount];
            isEmptySwitchesState = true;
        }
        
        for (int i = 0; i < allSwitchesParent.childCount; i++)
        {
            Script_LightSwitch lightSwitch = null;
            Script_Switch switchObj = null;
            
            Script_Switch child = allSwitchesParent.GetChild(i).GetComponent<Script_Switch>();
            if (child is Script_LightSwitch)
            {
                lightSwitch = (Script_LightSwitch)child;
                interactableObjects.Add(lightSwitch);
                switches.Add(lightSwitch);
            }
            else if (child is Script_Switch)
            {
                switchObj = (Script_Switch)child;
                interactableObjects.Add(switchObj);
                switches.Add(switchObj);
            }

            if (isInitialize)
            {
                child.Id = interactableObjects.Count - 1;
                child.switchId = switches.Count - 1;

                /// Set the switch with the state in switchesState
                /// if no loaded switchesState is provided, then default to the setting
                /// of the light itself 
                if (child is Script_LightSwitch)
                {
                    if (isEmptySwitchesState)
                    {
                        lightSwitch.SetupSceneLights(child.isOn);
                        switchesState[i] = child.isOn;
                    }
                    else
                    {
                        lightSwitch.SetupSceneLights(switchesState[i]);
                    }
                }
                else if (child is Script_Switch)
                {
                    if (isEmptySwitchesState)
                    {
                        switchObj.SetupSwitch(child.isOn, null, null);
                        switchesState[i] = child.isOn;
                    }
                    else
                    {
                        switchObj.SetupSwitch(switchesState[i], null, null);
                    }
                }
                
                // TODO: REMOVE
                Script_SortingOrder so = child.GetRendererChild().GetComponent<Script_SortingOrder>();
                child.Setup(so.enabled, so.sortingOrderIsAxisZ, so.offset);
            }
        }

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            if (isEmptySwitchesState)
                Script_Utils.PrintArray(switchesState, "no switches data; init switches state based on switches...");
            else
                Script_Utils.PrintArray(switchesState, "loaded switchesState");
        }

        return switchesState;
    }

    // public void CreateInteractableObjects(
    //     Model_InteractableObject[] interactableObjectsData,
    //     List<Script_InteractableObject> interactableObjects,
    //     List<Script_Switch> switches,
    //     Vector3 rotationAdjustment,
    //     Script_DialogueManager dialogueManager,
    //     Script_Player player,
    //     bool[] switchesState,
    //     bool isForceSortingLayer,
    //     bool isSortingLayerAxisZ,
    //     int offset
    // )
    // {
    //     if (interactableObjectsData.Length == 0)    return;

    //     for (int i = 0; i < interactableObjectsData.Length; i++)
    //     {
    //         if (interactableObjectsData[i].type == "text")
    //         {
    //             Script_InteractableObjectText iObj;

    //             iObj = Instantiate(
    //                 InteractableObjectTextPrefab,
    //                 interactableObjectsData[i].objectSpawnLocation,
    //                 Quaternion.Euler(rotationAdjustment)
    //             );
                
    //             iObj.SetupText(dialogueManager, player, interactableObjectsData[i].dialogue);
    //             interactableObjects.Add(iObj);
    //             iObj.Id = i;
    //             iObj.nameId = interactableObjectsData[i].nameId;
    //             iObj.Setup(isForceSortingLayer, isSortingLayerAxisZ, offset);
    //         }
    //         else if (interactableObjectsData[i].type == "lightswitch")
    //         {
    //             Script_LightSwitch iObj;
                
    //             iObj = Instantiate(
    //                 LightSwitchPrefab,
    //                 interactableObjectsData[i].objectSpawnLocation,
    //                 Quaternion.Euler(rotationAdjustment)
    //             );
                
    //             lights = interactableObjectsData[i].lights;
    //             OnSprite = interactableObjectsData[i].onSprite;
    //             OffSprite = interactableObjectsData[i].offSprite;

    //             // if didn't customize, then use default
    //             float onIntensity = interactableObjectsData[i].lightOnIntensity;
    //             float offIntensity = interactableObjectsData[i].lightOffIntensity;
    //             if (onIntensity == 0f && offIntensity == 0)
    //             {
    //                 onIntensity = defaultOnIntensity;
    //                 offIntensity = defaultOffIntensity;
    //             }
    //             // TODO 
    //             interactableObjects.Add(iObj);
    //             switches.Add(iObj);
    //             iObj.Id = i;
    //             iObj.nameId = interactableObjectsData[i].nameId;
    //             iObj.switchId = switches.Count - 1;
    //             iObj.SetupLights(
    //                 lights,
    //                 onIntensity,
    //                 offIntensity,
    //                 switchesState == null
    //                     ? interactableObjectsData[i].isOn
    //                     : switchesState[switches.Count - 1],
    //                 OnSprite,
    //                 OffSprite
    //             );
    //             iObj.Setup(isForceSortingLayer, isSortingLayerAxisZ, offset);
    //         }
    //         else if (interactableObjectsData[i].type == "switch")
    //         {
    //             Script_Switch iObj;

    //             iObj = Instantiate(
    //                 SwitchPrefab,
    //                 interactableObjectsData[i].objectSpawnLocation,
    //                 Quaternion.Euler(rotationAdjustment)
    //             );
    //             // TODO 
    //             interactableObjects.Add(iObj);
    //             switches.Add(iObj);
    //             iObj.Id = i;
    //             iObj.nameId = interactableObjectsData[i].nameId;
    //             iObj.switchId = switches.Count - 1;
    //             iObj.SetupSwitch(
    //                 switchesState == null        
    //                     ? interactableObjectsData[i].isOn
    //                     : switchesState[switches.Count - 1],
    //                 OnSprite,
    //                 OffSprite
    //             );
    //             iObj.Setup(isForceSortingLayer, isSortingLayerAxisZ, offset);
    //         } else
    //         {
    //             Script_InteractableObject iObj;
                
    //             iObj = Instantiate(
    //                 InteractableObjectPrefab,
    //                 interactableObjectsData[i].objectSpawnLocation,
    //                 Quaternion.Euler(rotationAdjustment)
    //             );
    //             interactableObjects.Add(iObj);
    //             iObj.Id = i;
    //             iObj.nameId = interactableObjectsData[i].nameId;
    //             iObj.Setup(isForceSortingLayer, isSortingLayerAxisZ, offset);
    //         }
    //     }
    // }

    public void SetupPushables(
        Transform pushablesParent,
        List<Script_InteractableObject> interactableObjects,
        List<Script_Pushable> pushables,
        bool isInit
    )
    {
        pushablesCreator.SetupPushables(
            pushablesParent, interactableObjects, pushables, isInit
        );
    }

    public void DestroyInteractableObjects(
        List<Script_InteractableObject> interactableObjects,
        List<Script_Switch> switches,
        List<Script_Pushable> pushables
    )
    {
        foreach(Script_InteractableObject io in interactableObjects)
        {
            if (io)    Destroy(io.gameObject);
        }    

        interactableObjects.Clear();
        switches.Clear();
        pushables.Clear();
    }

    public void ClearInteractableObjects(
        List<Script_InteractableObject> interactableObjects,
        List<Script_Switch> switches,
        List<Script_Pushable> pushables
    )
    {
        interactableObjects.Clear();
        switches.Clear();
        pushables.Clear();
    }
}
