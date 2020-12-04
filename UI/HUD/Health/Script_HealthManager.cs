using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the Health View; actual death functionality in PlayerStats
/// 
/// NOTE: Should only be called from Stats
/// (is what actually handles health)
/// This only handles the view
/// </summary>
public class Script_HealthManager : MonoBehaviour
{
    public Vector3 heartSpawnLocation;
    public Script_HealthHeart heartPrefab;
    public Script_HealthHeartsHolder heartContainer;
    public CanvasGroup healthCanvas;
    

    [SerializeField] private List<Script_HealthHeart> hearts;
    // if this goes above slots - 1, then player loses
    [SerializeField] private int heartIndex;
    [SerializeField] private Script_Game game;
    

    public void FillHearts(int count)
    {
        for (int i = 0; i < count; i++)
        {
            hearts[heartIndex].Fill();
            heartIndex++;

            // actual death handled in PlayerStats
            if (heartIndex == hearts.Count)
            {
                print("DEATH!!!!!!!!!!!!!!!!!!");
            }
        }
    }

    public void RemoveHearts(int count)
    {
        Debug.Log($"Removing hearts: {count}");

        for (int i = 0; i < count; i++)
        {
            if (heartIndex == 0)    return;
            heartIndex--;
            hearts[heartIndex].Empty();
        }
    }


    public void Close()
    {
        healthCanvas.GetComponent<Script_CanvasGroupController_Health>()
            .Close();
    }

    public void Open()
    {
        healthCanvas.GetComponent<Script_CanvasGroupController_Health>()
            .Open();
    }

    public void Setup(Script_Player player)
    {
        // setup # of slots equal to thoughtSlots, start as empty state
        game = transform.parent.GetComponent<Script_Game>();
        InitializeHearts();

        healthCanvas.gameObject.SetActive(true);        

        void InitializeHearts()
        {
            // match with maxHp
            int playerHp = player.GetComponent<Script_PlayerStats>().stats.maxHp.GetVal();
            for (int i = 0; i < playerHp; i++)
            {
                Script_HealthHeart heart = Instantiate(heartPrefab, heartSpawnLocation, Quaternion.identity);
                heart.transform.SetParent(heartContainer.transform, false);
                hearts.Add(heart);
                heart.Setup(false);
            }

            FillHearts(playerHp);
        }
    }
}
