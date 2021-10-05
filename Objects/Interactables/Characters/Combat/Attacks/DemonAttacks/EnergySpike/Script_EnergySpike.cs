using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Script_EnergySpike : MonoBehaviour
{
    public Script_HitBox hitBox;

    [SerializeField] private bool isHitBoxDisabled;

    void OnValidate()
    {
        DisableHitbox(isHitBoxDisabled);
    }

    void Awake()
    {
        DisableHitbox(isHitBoxDisabled);
    }

    public void Play()
    {
        GetComponent<PlayableDirector>().Play();
    }

    private void DisableHitbox(bool isDisabled)
    {
        hitBox.IsDisabled = isDisabled;
    }
}
