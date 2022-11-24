using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_QuestPaintingsManager : MonoBehaviour
{
    public enum Painting {
        Ellenia = 0,
        Ids = 1,
        Eileen = 2,
        WellsWorld = 3,
        GardenLabyrinth = 4,
        KTVRoom2 = 5
    }

    [SerializeField] private Script_LevelBehavior_25 Ellenia;
    [SerializeField] private Script_LevelBehavior_10 Ids;
    [SerializeField] private Script_LevelBehavior_26 Eileen;
    [SerializeField] private Script_LevelBehavior_42 WellsWorld;
    [SerializeField] private Script_LevelBehavior_46 GardenLabyrinth;
    [SerializeField] private Script_LevelBehavior_24 KTVRoom2;

    
    public bool GetQuestPaintingIsDone(Painting quest) => quest switch
    {
        Painting.Ellenia => Ellenia.isCurrentPuzzleComplete,
        Painting.Ids => Ids.isCurrentPuzzleComplete,
        Painting.Eileen => Eileen.isCurrentPuzzleComplete,
        Painting.WellsWorld => WellsWorld.isCurrentMooseQuestComplete,
        Painting.GardenLabyrinth => GardenLabyrinth.isCurrentPuzzleComplete,
        Painting.KTVRoom2 => KTVRoom2.IsCurrentPuzzleComplete,
        _ => false
    };

    public bool GetIsAllQuestsDoneToday() =>
        Ellenia.isCurrentPuzzleComplete
            && Ids.isCurrentPuzzleComplete
            && Eileen.isCurrentPuzzleComplete
            && WellsWorld.isCurrentMooseQuestComplete
            && GardenLabyrinth.isCurrentPuzzleComplete
            && KTVRoom2.IsCurrentPuzzleComplete;
    
    public void Setup()
    {
        
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_QuestPaintingsManager))]
    public class Script_QuestPaintingsManagerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_QuestPaintingsManager t = (Script_QuestPaintingsManager)target;
            if (GUILayout.Button("Print Quest Status"))
            {
                Dev_Logger.Debug($"Ellenia Done: {t.GetQuestPaintingIsDone(Painting.Ellenia)}");
                Dev_Logger.Debug($"Ids Done: {t.GetQuestPaintingIsDone(Painting.Ids)}");
                Dev_Logger.Debug($"Eileen Done: {t.GetQuestPaintingIsDone(Painting.Eileen)}");
                Dev_Logger.Debug($"Moose Done: {t.GetQuestPaintingIsDone(Painting.WellsWorld)}");
                Dev_Logger.Debug($"Kaffe Latte Done: {t.GetQuestPaintingIsDone(Painting.GardenLabyrinth)}");
                Dev_Logger.Debug($"Ursie Done: {t.GetQuestPaintingIsDone(Painting.KTVRoom2)}");

                Dev_Logger.Debug($"ALL DONE: {t.GetIsAllQuestsDoneToday()}");
            }
        }
    }
#endif
}
