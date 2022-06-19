using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_TierComment : MonoBehaviour
{
    private enum TextAnimations
    {
        Emphasize = 0,
        Flashing = 1,
        None = 9
    }
    
    private static string EmphasizeTrigger = "emphasize";
    private static string PulseTrigger = "pulse";
    private static string Flash = "TMPro_Emphasize_Flashing";
    
    [SerializeField] private float emphasizeScale = 1.2f;
    [SerializeField] private FadeSpeeds fadeSpeed;
    [SerializeField] private float emphasizeTime = 0.167f;
    
    [SerializeField] private TextAnimations textAnimation;
    [SerializeField] private bool isFader;
    [SerializeField] private bool isEmphasizeNonAnimator;
    
    [SerializeField] private Animator TMProAnimator;
    [SerializeField] private Script_CanvasGroupController canvasGroupController;
    
    private IEnumerator co;
    private float activateTimeLength;
    private float emphasizeNonAnimatorTimer;


    void LateUpdate()
    {
        HandleEmphasizeNonAnimator();
    }
    
    public void Activate()
    {
        this.gameObject.SetActive(true);

        if (isFader)
        {
            canvasGroupController.Close();
            canvasGroupController.FadeIn(fadeSpeed.ToFadeTime());
        }

        switch (textAnimation)
        {
            case (TextAnimations.Emphasize):
                TMProAnimator.SetTrigger(EmphasizeTrigger);    
                break;
            case (TextAnimations.Flashing):
                TMProAnimator.Play(Flash);
                break;
        }

        if (isEmphasizeNonAnimator)
            EmphasizeNonAnimator();
        
        if (co != null)
        {
            StopCoroutine(co);
        }

        co = WaitToDeactivate();
        StartCoroutine(co);

        IEnumerator WaitToDeactivate()
        {
            yield return new WaitForSeconds(activateTimeLength);

            Deactivate(false);
        }
    }

    private void Deactivate(bool isInit = true)
    {
        if (isFader)
        {
            if (isInit)
                canvasGroupController.Close();
            else
                canvasGroupController.FadeOut(fadeSpeed.ToFadeTime(), canvasGroupController.Close);
        }
        else
            this.gameObject.SetActive(false);
    }

    public void Pulse()
    {
        TMProAnimator.SetTrigger(PulseTrigger);
    }

    /// <summary>
    /// Emphasize manually when we're playing another animation that needs the
    /// animator state (e.g. flashing)
    /// </summary>
    private void EmphasizeNonAnimator()
    {
        TMProAnimator.transform.localScale = new Vector3(emphasizeScale, emphasizeScale, 1);
        
        emphasizeNonAnimatorTimer = emphasizeTime;
    }

    /// <summary>
    /// Note: Must do in Late Update to override animator values.
    /// </summary>
    private void HandleEmphasizeNonAnimator()
    {
        if (emphasizeNonAnimatorTimer > 0f)
        {
            emphasizeNonAnimatorTimer -= Time.deltaTime;
            
            if (emphasizeNonAnimatorTimer <= 0)
                emphasizeNonAnimatorTimer = 0;

            float newScale = 1f + ((emphasizeNonAnimatorTimer / emphasizeTime) * (emphasizeScale - 1f));
            TMProAnimator.transform.localScale = new Vector3(newScale, newScale, 1);
        }
    }

    public void Setup(float t)
    {
        activateTimeLength = t;

        Deactivate();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_TierComment))]
public class Script_TierCommentTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_TierComment t = (Script_TierComment)target;
        if (GUILayout.Button("Activate"))
        {
            t.Activate();
        }

        if (GUILayout.Button("Pulse"))
        {
            t.Pulse();
        }
    }
}
#endif