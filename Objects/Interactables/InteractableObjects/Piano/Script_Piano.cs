using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_Piano : Script_InteractableObjectText
{
    private const string MapNameField = "MapName";
    
    [SerializeField] private Model_PianoData pianoData;
    
    public bool IsRemembered
    {
        get => pianoData.isRemembered;
        set => pianoData.isRemembered = value;
    }
    
    public int Level
    {
        get => pianoData.spawn.data.level;
    }

    public Vector3 PlayerSpawn
    {
        get => pianoData.spawn.data.playerSpawn;
    }

    public Directions FacingDirection
    {
        get => pianoData.spawn.data.facingDirection;
    }

    public string MapName
    {
        get
        {
            Debug.Log($"MapNameField {MapNameField} for {pianoData.levelBehavior}");
            
            if (pianoData.levelBehavior.HasField(MapNameField))
            {
                return pianoData.levelBehavior.GetField<string>(MapNameField);
            }
            
            return null;
        }
    }
    
    public override void ActionDefault()
    {
        if (CheckDisabled())
        {
            Debug.Log($"{name} is disabled");
            return;     
        }

        IsRemembered = true;
        
        base.ActionDefault();
    }       
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Piano))]
public class Script_PianoTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_Piano t = (Script_Piano)target;
        if (GUILayout.Button("Test MapFieldName"))
        {
            Debug.Log(t.MapName);
        }
    }
}
#endif
