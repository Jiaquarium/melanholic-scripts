using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_IceMelt : MonoBehaviour
{
    private static float maxDistance = 25f;
    private static int MeltFrameSkip = 2;
    private static int DistanceFrameSkip = 4;
    
    private bool isMelting;
    private float meltRate;
    private List<Collider> myCols = new List<Collider>();
    private Rigidbody myRigidBody;
    
    private bool isDistanceHide;
    private float groundDistance;
    private Script_Game game;

    public Script_Game Game
    {
        get {
            if (game == null)
                game = Script_Game.Game;

            return game;
        }
        set => game = value;
    }
    
    void Awake()
    {
        GetComponents<Collider>(myCols);
        myRigidBody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // Only update scale every X frames
        if (isMelting)
        {
            if (Time.frameCount % MeltFrameSkip == 0)
                Melt();
            
            // For snowy maps, the ice melt rate is extremely slow, so hide the ice if the player gets too far.
            if (isDistanceHide && Time.frameCount % DistanceFrameSkip == 0)
                HandleDistanceHide();
        }
        
        if (transform.localScale == Vector3.zero)
        {
            isMelting = false;
            Disable();
        }
    }

    public void StartMelt(float rate, bool isDistanceMelt = false)
    {
        meltRate = rate;
        isDistanceHide = isDistanceMelt;
        isMelting = true;
    }

    // Note: when Player leaves map, CrackableStats will handle deactivating the parent GameObject
    public void Disable()
    {
        myCols.ForEach(myCol => myCol.enabled = false);
        myRigidBody.isKinematic = true;
        isMelting = false;
        transform.localScale = Vector3.zero;

        gameObject.SetActive(false);
    }

    private void Melt()
    {
        // Must adjust back for MeltFrameSkip for skipped frames
        var meltAmount = meltRate * Time.deltaTime * MeltFrameSkip;
        
        var newXScale = Mathf.Max(transform.localScale.x - meltAmount, 0f);
        var newYScale = Mathf.Max(transform.localScale.y - meltAmount, 0f);
        var newZScale = Mathf.Max(transform.localScale.z - meltAmount, 0f);

        transform.localScale = new Vector3(newXScale, newYScale, newZScale);
    }

    private void HandleDistanceHide()
    {
        if (Game == null || !Game.GetPlayerIsSpawned())
            return;

        Vector3 playerLocation = Game.GetPlayerLocation();
        
        // Calculate distance ignoring non-2D axis (gizmos are accurate this way)
        Vector2 playerLocationXZ = new Vector2(playerLocation.x, playerLocation.z);
        Vector2 myLocationXZ = new Vector2(transform.position.x, transform.position.z);
        groundDistance = Vector2.Distance(playerLocationXZ, myLocationXZ);

        bool isOutOfRange = groundDistance > maxDistance;
        
        if (isOutOfRange)
            Disable();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var position = transform.position;
        Handles.color = Color.green;
        
        Handles.DrawWireDisc(position, new Vector3(0, 1, 0), maxDistance);
    }
#endif
}
