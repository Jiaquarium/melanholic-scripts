using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerMutation : MonoBehaviour
{
    public const int Layer = 0;

    [SerializeField] private Script_Game game;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Animator stepsAnimator;

    private Script_Player player;

    public Animator MyAnimator { get => myAnimator; }
    public Animator StepsAnimator { get => stepsAnimator; }

    void OnEnable()
    {
        player = game.GetPlayer();
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    public void HandleAnimatorState(
        RuntimeAnimatorController animatorController,
        AnimatorStateInfo animatorStateInfo,
        Directions facingDir,
        bool isMoving
    )
    {
        MyAnimator.runtimeAnimatorController = animatorController;
        MyAnimator.Play(animatorStateInfo.fullPathHash, Layer, animatorStateInfo.normalizedTime);
        MyAnimator.AnimatorSetDirection(facingDir);
        MyAnimator.SetBool(Script_PlayerMovement.PlayerMovingAnimatorParam, isMoving);

        StepsAnimator.AnimatorSetDirection(facingDir);
        StepsAnimator.SetBool(Script_PlayerMovement.PlayerMovingAnimatorParam, isMoving);
    }
}