using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_PersistentDropsContainer : MonoBehaviour
{
    public Model_PersistentDrop[] persistentDropsDevDisplay;
    
    public Model_PersistentDrop[] GetPersistentDropModels()
    {
        Script_ItemObject[] myItemObjects = transform.GetChildren<Script_ItemObject>();
        Model_PersistentDrop[] persistentDrops = new Model_PersistentDrop[myItemObjects.Length];
        
        for (int i = 0; i < persistentDrops.Length; i++)
        {
            float[] location = new float[3];
            location[0] = myItemObjects[i].transform.position.x;
            location[1] = myItemObjects[i].transform.position.y;
            location[2] = myItemObjects[i].transform.position.z;
            
            persistentDrops[i] = new Model_PersistentDrop(
                location,
                myItemObjects[i].Item.id,
                myItemObjects[i].myLevelBehavior
            );
        }

        persistentDropsDevDisplay = persistentDrops;
        return persistentDrops;
    }

    public void ActivatePersistentDropsForLevel(int level)
    {
        Script_ItemObject[] myItemObjects = transform.GetChildren<Script_ItemObject>();
        
        foreach (Script_ItemObject itemObject in myItemObjects)
        {
            if (itemObject.myLevelBehavior == level)
                itemObject.gameObject.SetActive(true);
            else
                itemObject.gameObject.SetActive(false);
        }
        
        this.gameObject.SetActive(true);
    }

    public void DeactivatePersistentDrops()
    {
        this.gameObject.SetActive(false);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_PersistentDropsContainer))]
public class Script_PersistentDropsContainerTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_PersistentDropsContainer drops = (Script_PersistentDropsContainer)target;
        if (GUILayout.Button("GetPersistentDropModels()"))
        {
            drops.GetPersistentDropModels();
        }
    }
}
#endif