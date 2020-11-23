using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For use in Timeline
/// Set isParent if want to specify a parent Transform of SpriteRenderers
/// </summary>
public class Script_SpriteFadeInOnEnable : MonoBehaviour
{
    [SerializeField] private Script_SpriteFadeOut spriteFader;
    [SerializeField] private Transform parent;
    [SerializeField] private float fadeTime;
    [SerializeField] private float alphaTarget; // in decimal
    [SerializeField] private bool isFadeIn = true;
    [SerializeField] private bool isParent;
    [SerializeField] private bool isPlayer;

    void OnEnable()
    {
        if (isPlayer)
        {
            FadePlayer();
            return;
        }
        
        if (isParent)   FadeChildrenRecursive(parent);
        else            Fade(spriteFader);
    }

    private void Fade(Script_SpriteFadeOut mySprite)
    {
        if (isFadeIn)
        {
            // mySprite.SetVisibility(false);
            StartCoroutine(mySprite.FadeInCo(null, fadeTime, alphaTarget));    
        }
        else
        {
            // mySprite.SetVisibility(true);
            StartCoroutine(mySprite.FadeOutCo(null, fadeTime, alphaTarget));    
        }
    }

    private void FadeChildrenRecursive(Transform parent)
    {
        if (parent == null) Debug.LogError("You need to set a parent to if you want to fade children.");

        Script_SpriteFadeOut[] children = parent.GetComponentsInChildren<Script_SpriteFadeOut>();
        if (isFadeIn)
        {
            foreach(var child in children)  Fade(child);  
        }
        else
        {
            foreach(var child in children)  Fade(child);
        }
    }

    private void FadePlayer()
    {
        Script_Game.Game.GetPlayer().isInvisible = !isFadeIn;
    }
}
