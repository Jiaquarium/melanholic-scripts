using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class Script_TileMapExitEntrance : MonoBehaviour
{
    // option to pass an ExitMetadata option instead of explicitly defining
    public Script_ExitMetadata exitEntranceMetadata;
    
    [SerializeField] private int level;
    [SerializeField] private Vector3 playerNextSpawnPosition;
    [SerializeField] private Directions playerFacingDirection = Directions.Up;
    /// Used to specify special exit behaviors
    [SerializeField] private Script_Exits.ExitType _type;
    [SerializeField] private bool isDisabled;
    [SerializeField] private bool _isSilent;

    public int Level
    {
        get
        {
            if (exitEntranceMetadata != null)
                return exitEntranceMetadata.data.level;

            return level;
        }
        set => level = value;
    }

    public Vector3 PlayerNextSpawnPosition
    {
        get
        {
            if (exitEntranceMetadata != null)
                return exitEntranceMetadata.data.playerSpawn;
            
            return playerNextSpawnPosition;
        }
        set => playerNextSpawnPosition = value;
    }

    public Directions PlayerFacingDirection
    {
        get
        {
            if (exitEntranceMetadata != null)
                return exitEntranceMetadata.data.facingDirection;
            
            return playerFacingDirection;
        }
        set => playerFacingDirection = value;
    }

    public Script_Exits.ExitType Type
    {
        get => _type;
    }

    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }

    public bool IsSilent
    {
        get => _isSilent;
    }

    void OnValidate() {
        if (exitEntranceMetadata != null)
        {
            level                   = exitEntranceMetadata.data.level;
            playerNextSpawnPosition = exitEntranceMetadata.data.playerSpawn;
            playerFacingDirection   = exitEntranceMetadata.data.facingDirection;
        }    
    }
    
    void Awake()
    {
        if (!Debug.isDebugBuild || !Const_Dev.IsDevMode)
            GetComponent<TilemapRenderer>().enabled = false;
    }
    
}
