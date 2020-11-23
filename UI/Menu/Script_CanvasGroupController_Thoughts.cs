using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Script_CanvasGroupController_Thoughts : Script_CanvasGroupController
{
    public Script_Game game;
    public GameObject thoughts;
    public GameObject emptyThoughts;
    public Button EmptyStateDefaultButton; 
    
    public override void Open()
    {
        HandleThoughtsState();
        base.Open();
    }

    void HandleThoughtsState()
    {
        Debug.Log($"Thoughts count on opening thoughts inventory: {game.GetThoughtsCount()}");
        
        if (game.GetThoughtsCount() > 0)    ShowThoughts();
        else                                ShowEmptyState();

        void ShowThoughts()
        {
            thoughts.SetActive(true);
            emptyThoughts.SetActive(false);
        }

        void ShowEmptyState()
        {
            /// If empty state always reset selected button to the top bar
            if (
                EventSystem.current != null &&
                EventSystem.current.currentSelectedGameObject != EmptyStateDefaultButton.gameObject
            )
            {
                EventSystem.current.SetSelectedGameObject(EmptyStateDefaultButton.gameObject);
                EmptyStateDefaultButton.GetComponent<Script_ButtonHighlighter>().Select();
            }
            
            thoughts.SetActive(false);
            emptyThoughts.SetActive(true);
        }
    }

    public override void Setup()
    {
        // TODO: we can update this in the game when we get a thought
        // there is still a bit of lag on first open
        HandleThoughtsState();
    }
}
