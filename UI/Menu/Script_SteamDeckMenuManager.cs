using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_SteamDeckMenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform stickerDescriptionRect;
    [SerializeField] private RectTransform itemDescriptionRect;
    [SerializeField] private Vector2 descriptionRectNewSize;

    [SerializeField] private RectTransform stickerDescriptionTextRect;
    [SerializeField] private RectTransform itemDescriptionTextRect;
    [SerializeField] private Vector2 descriptionTextRectNewSize;

    [SerializeField] private TextMeshProUGUI stickerName;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private FontStyles nameNewFontStyles;
    
    void OnEnable()
    {
        if (Script_Game.IsSteamRunningOnSteamDeck)
        {
            SetNewWidths();
            SetNewTextWidths();
            SetNewStyles();
        }
    }

    private void SetNewWidths()
    {
        Vector2 newDescriptionSize = new Vector2(descriptionRectNewSize.x, descriptionRectNewSize.y);

        itemDescriptionRect.sizeDelta = newDescriptionSize;
        stickerDescriptionRect.sizeDelta = newDescriptionSize;
    }

    private void SetNewTextWidths()
    {
        Vector2 newDescriptionSize = new Vector2(descriptionTextRectNewSize.x, descriptionTextRectNewSize.y);

        itemDescriptionTextRect.sizeDelta = newDescriptionSize;
        stickerDescriptionTextRect.sizeDelta = newDescriptionSize;
    }

    private void SetNewStyles()
    {
        itemName.fontStyle = nameNewFontStyles;
        stickerName.fontStyle = nameNewFontStyles;
    }
}
