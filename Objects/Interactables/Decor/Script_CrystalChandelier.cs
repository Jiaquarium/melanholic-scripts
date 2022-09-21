using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_CrystalChandelier : MonoBehaviour
{
    private static readonly int IsSpinning = Animator.StringToHash("isSpinning");
    
    [SerializeField] private Animator a; 

    void Awake() {
        a = GetComponent<Animator>();
    }

    public void StartSpinning()
    {
        a.SetBool(IsSpinning, true);
    }

    public void StopSpinning()
    {
        a.SetBool(IsSpinning, false);
    }
}
