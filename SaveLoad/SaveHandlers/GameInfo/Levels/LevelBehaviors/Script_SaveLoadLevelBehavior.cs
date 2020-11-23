using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SaveLoadLevelBehavior : MonoBehaviour
{
    public virtual void Save(Model_SaveData data) { }
    public virtual void Load(Model_SaveData data) { }
}
