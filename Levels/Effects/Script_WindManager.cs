using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WindManager : MonoBehaviour
{   
    public static Script_WindManager Control;

    [SerializeField] private float noEffectFactor = 1f;
    [SerializeField] private WindFactors defaultFactors;

    [Header("Mask Specific Wind Adjustment Factors")]
    [SerializeField] private WindFactors snowWomanFactors;


    [System.Serializable]
    private class WindFactors
    {
        // Moving laterally e.g. wind from N, moving R or L
        public float lateral;
        
        // Trying to move laterally but pushed back 1 space as well. 
        public float diagonal;
        
        public float headwind;
        
        public float tailwind;
        
        public float passive;
    }

    public float NoEffectFactor
    {
        get => noEffectFactor;
    }

    public float Lateral(string id) => id switch
    {
        Const_Items.IceSpikeId => snowWomanFactors.lateral,
        _ => defaultFactors.lateral,
    };

    public float Diagonal(string id) => id switch
    {
        Const_Items.IceSpikeId => snowWomanFactors.diagonal,
        _ => defaultFactors.diagonal,
    };

    public float Headwind(string id) => id switch
    {
        Const_Items.IceSpikeId => snowWomanFactors.headwind,
        _ => defaultFactors.headwind,
    };

    public float Tailwind(string id) => id switch
    {
        Const_Items.IceSpikeId => snowWomanFactors.tailwind,
        _ => defaultFactors.tailwind,
    };

    public float Passive(string id) => id switch
    {
        Const_Items.IceSpikeId => snowWomanFactors.passive,
        _ => defaultFactors.passive,
    };

    public void Setup()
    {
        if (Control == null)
            Control = this;
        else if (Control != this)
            Destroy(this.gameObject);
    }
}