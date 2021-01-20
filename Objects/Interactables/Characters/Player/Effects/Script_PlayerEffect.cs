using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_PlayerEffect : MonoBehaviour
{
    [SerializeField] private Script_PlayerEffectQuestionMarkAnimate questionMark;
    [SerializeField] private Script_EffectItemPickUp itemDisplay;
    [SerializeField] private AudioSource dropSFXsource;
    [SerializeField] private Script_GiantBoarNeedleEffect giantBoarNeedle;

    public void QuestionMark(bool isShow)
    {
        if (isShow)     questionMark.QuestionMark();
        else            questionMark.HideQuestionMark();
    }

    public void ItemPickUp(bool isShow, Script_Item item)
    {
        if (isShow)     itemDisplay.ShowItem(item);
        else            itemDisplay.HideItem();
    }

    /// <summary>
    /// Fade to desired alpha
    /// </summary>
    public void SetVisibility(
        float alpha,
        float fadeTime,
        Renderer playerRenderer,
        Script_PlayerGhost playerGhost,
        Action cb
    )
    {
        SpriteRenderer sprite = playerRenderer as SpriteRenderer; 
        Debug.Log($"alpha: {alpha}, sprite.color.a: {sprite.color.a}, fadeTime: {fadeTime}");

        if (alpha > sprite.color.a)
        {
            StartCoroutine(playerGhost.spriteRenderer.GetComponent<Script_SpriteFadeOut>().FadeInCo(null, fadeTime, alpha));
            StartCoroutine(playerRenderer.GetComponent<Script_SpriteFadeOut>().FadeInCo(
                () => { if (cb != null) cb(); }, fadeTime, alpha
            ));
        }
        else if (alpha < sprite.color.a)
        {
            StartCoroutine(playerGhost.spriteRenderer.GetComponent<Script_SpriteFadeOut>().FadeOutCo(null, fadeTime, alpha));
            StartCoroutine(playerRenderer.GetComponent<Script_SpriteFadeOut>().FadeOutCo(
                () => { if (cb != null) cb(); }, fadeTime, alpha
            ));
        }
        else
        {
            if (cb != null) cb();
        }
    }

    public void DropSFX()
    {
        dropSFXsource.PlayOneShot(Script_SFXManager.SFX.playerDropFinishSFX, Script_SFXManager.SFX.playerDropFinishSFXVol);
    }

    public void GiantBoarNeedle()
    {
        giantBoarNeedle.Effect();
    }

    public void Setup()
    {
        questionMark.Setup();
        itemDisplay.Setup();
    }
}
