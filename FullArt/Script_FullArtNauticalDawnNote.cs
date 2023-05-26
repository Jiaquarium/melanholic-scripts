using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_FullArtNauticalDawnNote : MonoBehaviour
{
    [SerializeField] private Image doodle;
    [SerializeField] private Script_Game game;
    
    void OnEnable()
    {
        bool isNauticalDawn = game.Run.dayId == Script_Run.DayId.sun;
        doodle.gameObject.SetActive(isNauticalDawn);
    }
}
