using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemies, able to attack and EAT
/// </summary>
[RequireComponent(typeof(Script_DemonStats))]
public class Script_Demon : Script_Character
{
    public static string SwallowedTrigger = "SwallowedTrigger";
    public int Id;
    public Model_Thought thought;
    public int swallowedFillCount;
    

    public AudioClip deathCrySFX;
    public Animator animator;
    [SerializeField] private Renderer spriteRenderer;


    private AudioSource audioSource;
    private Script_AudioOneShotSource audioOneShotSource;
    

    private IEnumerator co;
    
    [SerializeField]
    private bool isInvincible;


    // Update is called once per frame
    void Update()
    {
        AdjustRotation();    
    }
    
    /// <summary>
    /// Start demon dying sequence
    /// </summary>
    public void Die()
    {
        if (isInvincible)   return;
        isInvincible = true;

        /// Only apply damage as time deductions
        // Script_Game.Game.PlayerHurtFromThought(swallowedFillCount, thought);
        Swallowed();
    }
    
    public void FinishSwallowed()
    {
        isInvincible = false;
        Script_Game.Game.EatDemon(Id);
        gameObject.SetActive(false);
    }
    

    private void Swallowed()
    {
        animator.SetTrigger(SwallowedTrigger);
        audioOneShotSource = Script_Game.Game.CreateAudioOneShotSource(transform.position);
        audioOneShotSource.Setup(deathCrySFX);
        audioOneShotSource.PlayOneShot();
    }

    /// <summary>
    /// meant to be overriden by specific Demon subclass
    /// </summary>
    public virtual void Attack() { }

    public void AdjustRotation()
    {
        spriteRenderer.transform.forward = Camera.main.transform.forward;
    }

    public virtual void Setup(
        Model_Thought _thought,
        AudioClip _deathCry
    )
    {
        audioSource = GetComponent<AudioSource>();
        
        thought = _thought;
        deathCrySFX = _deathCry;

        /// Setup character stats
        base.Setup();

        AdjustRotation();
    }
}
