using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Usable", menuName = "Inventory/Usables/Usable")]
/// <summary>
/// Can be used
/// Actions:
/// - use
/// - drop
/// - cancel
/// </summary>
public class Script_Usable : Script_Item
{
    public FadeSpeeds fadeInSpeed;
    public FadeSpeeds fadeOutSpeed;
    public Script_DialogueNode useSuccessMessage;
    public Script_DialogueNode useFailureMessage;
    public Script_Item node;
}
