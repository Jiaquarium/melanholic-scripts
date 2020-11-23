using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
/// <summary>
/// Used as quest items and interesting things player can view
/// to reveal more about characters and the story
/// Can be:
/// - examined
/// - dropped
/// </summary>
public class Script_Collectible : Script_Item
{
    // fullArt
    public string fullArtId;
    public FadeSpeeds fadeInSpeed;
    public FadeSpeeds fadeOutSpeed;
}
