using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerMutation : MonoBehaviour
{
    public const int Layer = 0;

    [SerializeField] private Script_Game game;
    [SerializeField] private Animator myAnimator;

    private Script_Player player;

    public Animator MyAnimator { get => myAnimator; }

    void OnEnable()
    {
        player = game.GetPlayer();
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    public void SyncAnimatorState(AnimatorStateInfo animatorStateInfo)
    {
        MyAnimator.Play(animatorStateInfo.fullPathHash, Layer, animatorStateInfo.normalizedTime);
    }
}
