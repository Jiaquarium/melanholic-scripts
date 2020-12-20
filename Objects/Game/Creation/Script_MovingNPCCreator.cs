using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MovingNPCCreator : MonoBehaviour
{
    public void SetupMovingNPC(
        Script_MovingNPC movingNPC,
        List<Script_StaticNPC> NPCs,
        List<Script_MovingNPC> movingNPCs,
        bool isInitialize
    )
    {
        NPCs.Add(movingNPC);
        movingNPCs.Add(movingNPC);

        movingNPC.StaticNPCId = NPCs.Count - 1;
        movingNPC.MovingNPCId = movingNPCs.Count - 1;
        
        if (Debug.isDebugBuild && Const_Dev.IsDevMode)
        {
            Debug.Log("NPCs Count: " + NPCs.Count + ", MovingNPCs Count: " + movingNPCs.Count);
        }

        if (isInitialize)
        {
            movingNPC.Setup();
        }
    }

    public void AutoSetup(
        Script_MovingNPC npc,
        List<Script_StaticNPC> NPCs,
        List<Script_MovingNPC> movingNPCs
    )
    {
        NPCs.Add(npc);
        movingNPCs.Add(npc);

        npc.StaticNPCId = NPCs.Count - 1;
        npc.MovingNPCId = movingNPCs.Count - 1;
    }
}
