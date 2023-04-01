using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_NotesHint : MonoBehaviour
{
    [SerializeField] private Script_CanvasGroupController canvasGroupController;

    public void Open() => canvasGroupController.Open();

    public void Close() => canvasGroupController.Close();
}
