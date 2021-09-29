using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_InteractionBoxController : MonoBehaviour
{
    [SerializeField] private Script_InteractionBox InteractionBoxN;
    [SerializeField] private Script_InteractionBox InteractionBoxE;
    [SerializeField] private Script_InteractionBox InteractionBoxS;
    [SerializeField] private Script_InteractionBox InteractionBoxW;
    public Script_InteractionBox activeBox;
    
    [SerializeField] List<Const_Tags.Tags> ignoreTags;

    /// <returns>ordered N, E, S, W</returns>
    public Script_InteractionBox[] InteractionBoxes
    {
        get => new Script_InteractionBox[]{
            InteractionBoxN,
            InteractionBoxE,
            InteractionBoxS,
            InteractionBoxW
        };
    }

    public void HandleActiveInteractionBox(Directions dir)
    {
        if (dir == Directions.Up)         SetActiveInteractionBox(InteractionBoxN);
        else if (dir == Directions.Right) SetActiveInteractionBox(InteractionBoxE);
        else if (dir == Directions.Down)  SetActiveInteractionBox(InteractionBoxS);
        else if (dir == Directions.Left)  SetActiveInteractionBox(InteractionBoxW);
    }

    void SetActiveInteractionBox(Script_InteractionBox box)
    {
        InteractionBoxN.isExposed = false;
        InteractionBoxE.isExposed = false;
        InteractionBoxS.isExposed = false;
        InteractionBoxW.isExposed = false;
        
        activeBox = box;
        box.isExposed = true;
    }

    public Script_StaticNPC GetNPC(Directions dir)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetNPC();
    }

    public Script_SavePoint GetSavePoint(Directions dir)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetSavePoint();
    }

    public Script_InteractableObject[] GetInteractableObject(Directions dir)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetInteractableObjects();
    }

    /// <param name="ignoreTags">Specify tags to ignore. (e.g. NPCs should ignore player interaction boxes.)</param>
    public List<Script_Interactable> GetInteractablesBlocking(Directions dir)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetInteractablesBlocking(ignoreTags);
    }

    public List<Script_Pushable> GetPushables(Directions dir)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetPushables();
    }

    public Script_ItemObject GetItem(Directions dir)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetItem();
    }

    public Script_UsableTarget GetUsableTarget(Directions dir)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetUsableTarget();
    }

    public List<Transform> GetUniqueBlocking(Directions dir, string tag)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetUniqueBlocking(tag);
    }

    public Script_DoorExit GetDoorExit(Directions dir)
    {
        HandleActiveInteractionBox(dir);
        return activeBox.GetDoorExit();
    }
}
