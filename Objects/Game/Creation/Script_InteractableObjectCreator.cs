using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_InteractableObjectCreator : MonoBehaviour
{
    public Script_InteractableObject InteractableObjectPrefab;
    public Script_Switch SwitchPrefab;
    public Script_LightSwitch LightSwitchPrefab;
    public Script_InteractableObjectText InteractableObjectTextPrefab;
    public Script_PushablesCreator pushablesCreator;
    [SerializeField] private Script_InteractableObjectExitCreator exitCreator;
    
    private Light[] lights;
    private Sprite OnSprite;
    private Sprite OffSprite;


    public float defaultOnIntensity;
    public float defaultOffIntensity;
    
    public void AddInteractableObject(
        Script_InteractableObject interactableObject,
        List<Script_InteractableObject> interactableObjects
    )
    {
        interactableObjects.Add(interactableObject);
    }

    public void SetupInteractableObjectsText(
        Transform textObjectParent,
        List<Script_InteractableObject> interactableObjects,
        Script_DialogueManager dm,
        Script_Player player,
        bool isInitialize
    )
    {
        Script_InteractableObjectText[] texts = textObjectParent
            .GetComponentsInChildren<Script_InteractableObjectText>(true);
        
        for (int i = 0; i < texts.Length; i++)
        {
            Script_InteractableObjectText text = texts[i];
            if (isInitialize)   InitializeTextObject(text, interactableObjects, dm, player);
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
        Script_DialogueManager dm,
        Script_Player player,
        bool isInitialize
    )
    {
        for (int i = 0; i < textObjs.Length; i++)
        {
            Script_InteractableObjectText iObj = textObjs[i];
            interactableObjects.Add(iObj);
            
            if (isInitialize)
                InitializeTextObject(iObj, interactableObjects, dm, player);
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
        Script_Player player
    )
    {
        interactableObjects.Add(iObj);
        iObj.SetupDialogueNodeText(dialogueManager, player);
        iObj.Id = interactableObjects.Count - 1;
        
        Script_SortingOrder so = iObj.GetRendererChild().GetComponent<Script_SortingOrder>();
        iObj.Setup(so.enabled, so.sortingOrderIsAxisZ, so.offset);
    }

    public void SetupInteractableFullArt
    (
        Transform fullArtParent,
        List<Script_InteractableObject> interactableObjects,
        Script_DialogueManager dialogueManager,
        Script_Player player,
        bool isInitialize
    )
    {
        Script_InteractableFullArt[] fullArts = fullArtParent
            .GetComponentsInChildren<Script_InteractableFullArt>(true);
        
        for (int i = 0; i < fullArts.Length; i++)
        {
            Script_InteractableFullArt fullArt = fullArts[i];
            if (fullArt == null)   continue;
            interactableObjects.Add(fullArt);
            
            print("fullArt is: " + fullArt);
            
            if (isInitialize)
            {
                fullArt.SetupDialogueNodeText(dialogueManager, player);
                fullArt.Id = interactableObjects.Count - 1;
                
                Script_SortingOrder so = fullArt.GetRendererChild().GetComponent<Script_SortingOrder>();
                fullArt.Setup(so.enabled, so.sortingOrderIsAxisZ, so.offset);
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
        bool[] switchesState,
        bool isInitialize
    )
    {
        /// Initializing level for the first time with no loaded state data
        bool isEmptySwitchesState = false;
        Script_Switch[] switchesChildren = allSwitchesParent.GetComponentsInChildren<Script_Switch>(true);

        if  (switchesState == null || switchesState.Length == 0)
        {
            switchesState = new bool[switchesChildren.Length];
            isEmptySwitchesState = true;
        }
        
        for (int i = 0; i < switchesChildren.Length; i++)
        {
            Script_LightSwitch lightSwitch = null;
            Script_Switch switchObj = null;
            
            Script_Switch child = switchesChildren[i];
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

    public void SetupInteractableObjectsExit(
        Transform parent,
        List<Script_InteractableObject> interactableObjects,
        bool isInit
    )
    {
        exitCreator.SetupExits(parent, interactableObjects, isInit);
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

    public void CleanupTextDisabledFields()
    {
        Object[] allText = GameObject.FindObjectsOfType(typeof(Script_InteractableObjectText));
        foreach (Script_InteractableObjectText t in allText)
        {
            Debug.Log(t.name);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_InteractableObjectCreator))]
public class Script_InteractableObjectCreatorTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_InteractableObjectCreator t = (Script_InteractableObjectCreator)target;
        if (GUILayout.Button("Clean Up Text Objects"))
        {
            t.CleanupTextDisabledFields();
        }
    }
}
#endif