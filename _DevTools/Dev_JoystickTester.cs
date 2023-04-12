using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_JoystickTester : MonoBehaviour
{
    private Player MyPlayer { get; set; }
    
    void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            gameObject.SetActive(false);
            return;
        }
        
        MyPlayer = Rewired.ReInput.players.GetPlayer(Script_Player.PlayerId);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!Debug.isDebugBuild)
            return;
        
        Vector2 dirVector = new Vector2(
            MyPlayer.GetAxis(Const_KeyCodes.RWHorizontal),
            MyPlayer.GetAxis(Const_KeyCodes.RWVertical)
        );

        // Equally weight Get Button Down listener
        // if (
        //     MyPlayer.GetButtonDown(Const_KeyCodes.RWVertical)
        //     || MyPlayer.GetNegativeButtonDown(Const_KeyCodes.RWVertical)
        //     || MyPlayer.GetButtonDown(Const_KeyCodes.RWHorizontal)
        //     || MyPlayer.GetNegativeButton(Const_KeyCodes.RWHorizontal)
        // )
        //     Debug.Log($"dirVector ({dirVector.x}, {dirVector.y})");

        // if (MyPlayer.GetButtonDown(Const_KeyCodes.RWVertical))
        //     Debug.Log($"{Time.frameCount} UP dirVector ({dirVector.x}, {dirVector.y})");
        // else if (MyPlayer.GetNegativeButtonDown(Const_KeyCodes.RWVertical))
        //     Debug.Log($"{Time.frameCount} DOWN dirVector ({dirVector.x}, {dirVector.y})");
        // else if (MyPlayer.GetButtonDown(Const_KeyCodes.RWHorizontal))
        //     Debug.Log($"{Time.frameCount} RIGHT dirVector ({dirVector.x}, {dirVector.y})");
        // else if (MyPlayer.GetNegativeButtonDown(Const_KeyCodes.RWHorizontal))
        //     Debug.Log($"{Time.frameCount} LEFT dirVector ({dirVector.x}, {dirVector.y})");
        
        // if (MyPlayer.GetButton(Const_KeyCodes.RWVertical))
        //     Debug.Log($"UP dirVector ({dirVector.x}, {dirVector.y})");
        // else if (MyPlayer.GetNegativeButton(Const_KeyCodes.RWVertical))
        //     Debug.Log($"DOWN dirVector ({dirVector.x}, {dirVector.y})");
        // else if (MyPlayer.GetButton(Const_KeyCodes.RWHorizontal))
        //     Debug.Log($"RIGHT dirVector ({dirVector.x}, {dirVector.y})");
        // else if (MyPlayer.GetNegativeButton(Const_KeyCodes.RWHorizontal))
        //     Debug.Log($"LEFT dirVector ({dirVector.x}, {dirVector.y})");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Dev_JoystickTester))]
public class Dev_JoystickTesterTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Dev_JoystickTester player = (Dev_JoystickTester)target;
        if (GUILayout.Button("Change Game State: Cut Scene"))
        {
            Script_Game.Game.ChangeStateCutScene();
        }

        if (GUILayout.Button("Change Game State: Interact"))
        {
            Script_Game.Game.ChangeStateInteract();
        }
    }
}
#endif
