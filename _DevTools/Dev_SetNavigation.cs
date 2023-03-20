using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Dev_SetNavigation : MonoBehaviour
{
    [SerializeField] private List<Script_ListOfLists<Button>> buttonsGrid;

#if UNITY_EDITOR
    private void SetGridNavigation(
        bool ignoreFirstRowUp = false,
        bool ignoreLastRowDown = false
    )
    {
        // loop through rows
        for (var i = 0; i < buttonsGrid.Count; i++)
        {
            // loop through columns
            for (var j = 0; j < buttonsGrid[i].Count; j++)
            {
                Button currentButton = buttonsGrid[i][j];
                Debug.Log($"{currentButton.name} {currentButton}");

                Button firstButtonInRow = buttonsGrid[i][0];
                Button lastButtonInRow = buttonsGrid[i][buttonsGrid[i].Count - 1];
                Button lastRowButtonSameCol = buttonsGrid[buttonsGrid.Count - 1][j];
                Button firstRowButtonSameCol = buttonsGrid[0][j];
                
                // if col is 0, then button selectOnLeft should be last element in row
                // otherwise, selectOnLeft should be prev element
                Button newSelectOnLeft = j == 0 ? lastButtonInRow : buttonsGrid[i][j - 1];
                currentButton.SetNavigation(newSelectOnLeft: newSelectOnLeft);

                // if col is last, then button selectOnRight should be first element in row
                // otherwise, selectOnRight should be next element
                Button newSelectOnRight = j == buttonsGrid[i].Count - 1 ? firstButtonInRow : buttonsGrid[i][j + 1];
                currentButton.SetNavigation(newSelectOnRight: newSelectOnRight);

                // if row is 0, then button selectOnUp should be last row, same column
                // otherwise, selectOnUp should be next row, same column
                Button newSelectOnUp = i == 0
                    ? (ignoreFirstRowUp ? null : lastRowButtonSameCol)
                    : buttonsGrid[i - 1][j];
                currentButton.SetNavigation(newSelectOnUp: newSelectOnUp);

                // if row is last, then button selectOnDown should be first row, same column
                // otherwise, selectOnDown should be next row, same column
                Button newSelectOnDown = i == buttonsGrid.Count - 1
                    ? (ignoreLastRowDown ? null : firstRowButtonSameCol)
                    : buttonsGrid[i + 1][j];
                currentButton.SetNavigation(newSelectOnDown: newSelectOnDown);

                if (PrefabUtility.IsPartOfPrefabInstance(currentButton))
                    PrefabUtility.RecordPrefabInstancePropertyModifications(currentButton);
            }
        }
    }

    [CustomEditor(typeof(Dev_SetNavigation))]
    public class Dev_SetNavigationTester : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Dev_SetNavigation t = (Dev_SetNavigation)target;
            
            if (GUILayout.Button("Set Grid Nav"))
            {
                t.SetGridNavigation();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(t);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                }
            }

            if (GUILayout.Button("Set Grid Nav (No vert connection)"))
            {
                t.SetGridNavigation(true, true);

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(t);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                }
            }
        }
    }
#endif
}
