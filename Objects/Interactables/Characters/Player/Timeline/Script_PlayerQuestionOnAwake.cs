using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerQuestionOnAwake : MonoBehaviour
{
    [SerializeField] private float t = 1.5f;
    [SerializeField] private bool isOnEnable; // do the action 1+ per game
    
    /// In the case this object is set Inactive before coroutine finishes, cancel
    /// the QuestionMark Effect (since coroutines are stopped when going inactive)
    void OnDisable()
    {
        Script_Game.Game.PlayerEffectQuestion(false);
    }

    void OnEnable()
    {
        if (isOnEnable)     PlayerEffectQuestion();
    }
    
    void Awake()
    {
        if (!isOnEnable)    PlayerEffectQuestion();
    }

    private void PlayerEffectQuestion()
    {
        StartCoroutine(PlayerEffectQuestionCo());
        
        IEnumerator PlayerEffectQuestionCo()
        {
            Script_Game.Game.PlayerEffectQuestion(true);
            yield return new WaitForSeconds(t);
            Script_Game.Game.PlayerEffectQuestion(false);
        }
    }
}
