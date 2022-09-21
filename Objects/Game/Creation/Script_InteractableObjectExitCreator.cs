using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InteractableObjectExitCreator : MonoBehaviour
{
    public void SetupExits(
        Transform exitObjectParent,
        List<Script_InteractableObject> interactableObjects,
        bool isInitialize
    )
    {
        Script_InteractableObjectExit[] exits = exitObjectParent
            .GetComponentsInChildren<Script_InteractableObjectExit>(true);
        
        foreach (var exit in exits)
        {
            if (isInitialize)   InitializeExitObject(exit);
        }

        if (Debug.isDebugBuild && Const_Dev.IsDevMode)  Dev_Logger.Debug("interactable objects count: " + interactableObjects.Count);

        void InitializeExitObject(Script_InteractableObjectExit iObj)
        {
            interactableObjects.Add(iObj);
            iObj.Id = interactableObjects.Count - 1;
            
            if (iObj.GetRendererChild() != null)
            {
                Script_SortingOrder so = iObj.GetRendererChild().GetComponent<Script_SortingOrder>();
                iObj.Setup(so.enabled, so.sortingOrderIsAxisZ, so.offset);
            }
            else
            {
                iObj.Setup(false, false, 0);
            }
        }
    }
}
