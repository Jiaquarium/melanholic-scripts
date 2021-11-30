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
    [SerializeField] private WindFactors myMaskFactors;

    [SerializeField] private bool isFinalRound;


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

    public bool IsFinalRound
    {
        get => isFinalRound;
        set => isFinalRound = value;
    }

    public float Lateral(string id)
    {
        float wind;
        
        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.lateral,
                _ => defaultFactors.lateral,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.lateral,
                _ => defaultFactors.lateral,
            };
        }

        return wind;
    }

    public float Diagonal(string id)
    {
        float wind;

        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.diagonal,
                _ => defaultFactors.diagonal,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.diagonal,
                _ => defaultFactors.diagonal,
            };
        }
        
        return wind;
    }

    public float Headwind(string id)
    {
        float wind;

        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.headwind,
                _ => defaultFactors.headwind,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.headwind,
                _ => defaultFactors.headwind,
            };
        }

        return wind;
    }

    public float Tailwind(string id)
    {
        float wind;

        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.tailwind,
                _ => defaultFactors.tailwind,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.tailwind,
                _ => defaultFactors.tailwind,
            };
        }
        
        return wind;
    }

    public float Passive(string id)
    {
        float wind;

        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.passive,
                _ => defaultFactors.passive,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.passive,
                _ => defaultFactors.passive,
            };
        }

        return wind;        
    }

    public void InitialState()
    {
        IsFinalRound = false;
    }
    
    public void Setup()
    {
        if (Control == null)
            Control = this;
        else if (Control != this)
            Destroy(this.gameObject);
        
        InitialState();
    }
}