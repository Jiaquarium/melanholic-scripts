using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_IceMeltController : MonoBehaviour
{
    public static float MeltRateDefault = 25f;
    public static float MeltRateSnow = 4f;
    
    [SerializeField] private List<Script_IceMelt> iceMelts;
    
    public bool DidStartMelt { get; set; }

    void OnValidate()
    {
        PopulateIceMelts();
    }

    public void StartMelt()
    {
        var game = Script_Game.Game;
        var isSnowyMap = game.levelBehavior == game.WellsWorldBehavior;
        var meltRate = isSnowyMap ? MeltRateSnow : MeltRateDefault;

        iceMelts.ForEach(iceMelt => iceMelt.StartMelt(meltRate, isSnowyMap));
        
        DidStartMelt = true;
    }
    
    private void PopulateIceMelts()
    {
        List<Script_IceMelt> myIceMelts = new List<Script_IceMelt>();
        myIceMelts = transform.GetComponentsInChildren<Script_IceMelt>(true).ToList();
        iceMelts = myIceMelts;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Script_IceMeltController))]
    public class Script_IceMeltControllerTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Script_IceMeltController t = (Script_IceMeltController)target;
            
            if (GUILayout.Button("Start Melt"))
            {
                t.StartMelt();
            }
        }
    }
#endif
}
