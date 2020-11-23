using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class to trigger collisions
/// </summary>
public class Script_Interactable : MonoBehaviour
{
    private static float dialogueCoolDownTime = 0.6f;
    [SerializeField] protected bool isDialogueCoolDown = false;
    private float timer;

    /// <summary>
    /// Parent classes reference isDialogueCoolDown in ActionDefault() 
    /// to cool down after a dialogue end
    /// 
    /// This must be in a coroutine since Update() is frequently overriden
    /// </summary>
    public void StartDialogueCoolDown()
    {
        isDialogueCoolDown = true;
        timer = dialogueCoolDownTime;
        StartCoroutine(DialogueCoolDownCo());

        IEnumerator DialogueCoolDownCo()
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            timer = 0f;
            isDialogueCoolDown = false;
        }
    }
}
