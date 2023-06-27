using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CutSceneNPCCreator : MonoBehaviour
{
    public void SetupCutSceneNPC(
        Script_CutSceneNPC cutSceneNPC,
        List<Script_StaticNPC> NPCs,
        List<Script_CutSceneNPC> cutSceneNPCs
    )
    {
        cutSceneNPC.Setup();
        
        NPCs.Add(cutSceneNPC);
        cutSceneNPCs.Add(cutSceneNPC);

        cutSceneNPC.StaticNPCId = NPCs.Count - 1;
        cutSceneNPC.CutSceneNPCId = cutSceneNPCs.Count - 1;
    }
}
