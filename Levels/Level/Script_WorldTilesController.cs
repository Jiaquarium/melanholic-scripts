using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_WorldTilesController : MonoBehaviour
{
    [SerializeField] private Vector2Int Center = new Vector2Int(1, 1);
    [SerializeField] private int xLength;
    [SerializeField] private int zLength;
    [SerializeField] private Script_WorldTile defaultOriginWorldTile;
    [SerializeField] private Model_WorldTilesMap worldTilesMap;
    [SerializeField] private List<Script_WorldTile> movedTiles;
    
    private Script_WorldTile originWorldTile;
    
    public int MapLength
    {
        get => worldTilesMap.Length;
    }
    
    public int XLength
    {
        get => xLength;
    }

    public int ZLength
    {
        get => zLength;
    }

    void Start()
    {
        InitialState();
    }

    public void SetNewOriginWorldTile(Script_WorldTile worldTile)
    {
        if (originWorldTile == worldTile)   return;

        // Calculate new origin Coord.
        Vector2Int newOriginCoordinates = worldTilesMap.GetWorldTileCoordinates(worldTile);

        // Get diff from center.
        Vector2Int diffFromCenter = newOriginCoordinates - Center;

        // Shift depending on diff.
        ShiftMap(diffFromCenter);

        originWorldTile = worldTile;
        
        void ShiftMap(Vector2Int shift)
        {
            if          (shift.y > 0)    ShiftTilesDown();
            else if     (shift.y < 0)    ShiftTilesUp();

            if          (shift.x > 0)    ShiftTilesRight();
            else if     (shift.x < 0)    ShiftTilesLeft();
        }
    }

    public void ShiftTilesDown()
    {
        worldTilesMap.ShiftTilesDown();
    }

    public void ShiftTilesUp()
    {
        worldTilesMap.ShiftTilesUp();
    }

    public void ShiftTilesLeft()
    {
        worldTilesMap.ShiftTilesLeft();
    }

    public void ShiftTilesRight()
    {
        worldTilesMap.ShiftTilesRight();
    }

    private Vector2Int GetNewOriginShift()
    {
        return new Vector2Int(0, 0);
    }

    public void InitialState()
    {
        worldTilesMap.InitialState();
        originWorldTile = defaultOriginWorldTile;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_WorldTilesController))]
public class Script_WorldTilesControllerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_WorldTilesController t = (Script_WorldTilesController)target;
        if (GUILayout.Button("Print Map Length"))
        {
            Debug.Log(t.MapLength);
        }

        if (GUILayout.Button("ShiftTilesDown()"))
        {
            t.ShiftTilesDown();
        }

        if (GUILayout.Button("ShiftTilesUp()"))
        {
            t.ShiftTilesUp();
        }

        if (GUILayout.Button("ShiftTilesLeft()"))
        {
            t.ShiftTilesLeft();
        }

        if (GUILayout.Button("ShiftTilesRight()"))
        {
            t.ShiftTilesRight();
        }
    }
}
#endif