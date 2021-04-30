using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadMynesMirror : MonoBehaviour
{
    [SerializeField] private Script_MynesMirrorManager mynesMirrorManager;
    
    public void SaveMynesMirror(Model_SaveData data)
    {
        data.mynesMirrorData = new Model_MynesMirror(
            mynesMirrorManager.InteractionCount
        );
    }

    public void LoadMynesMirror(Model_SaveData data)
    {
        Model_MynesMirror m = new Model_MynesMirror(
            data.mynesMirrorData.interactionCount
        );

        mynesMirrorManager.InteractionCount = m.interactionCount;
    }
}
