using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WindManager : MonoBehaviour
{   
    public static Script_WindManager Control;

    [SerializeField] private float noEffectFactor = 1f;
    [SerializeField] private WindFactors defaultFactors;
    [SerializeField] private WindFactors defaultRunFactors;

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

    public float Lateral(string id, bool isRunning)
    {
        float wind;
        
        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.lateral,
                _ => isRunning ? defaultRunFactors.lateral : defaultFactors.lateral,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.lateral,
                _ => isRunning ? defaultRunFactors.lateral : defaultFactors.lateral,
            };
        }

        return wind;
    }

    public float Diagonal(string id, bool isRunning)
    {
        float wind;

        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.diagonal,
                _ => isRunning ? defaultRunFactors.diagonal : defaultFactors.diagonal,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.diagonal,
                _ => isRunning ? defaultRunFactors.diagonal : defaultFactors.diagonal,
            };
        }
        
        return wind;
    }

    public float Headwind(string id, bool isRunning)
    {
        float wind;

        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.headwind,
                _ => isRunning ? defaultRunFactors.headwind : defaultFactors.headwind,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.headwind,
                _ => isRunning ? defaultRunFactors.headwind : defaultFactors.headwind,
            };
        }

        return wind;
    }

    public float Tailwind(string id, bool isRunning)
    {
        float wind;

        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.tailwind,
                _ => isRunning ? defaultRunFactors.tailwind : defaultFactors.tailwind,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.tailwind,
                _ => isRunning ? defaultRunFactors.tailwind : defaultFactors.tailwind,
            };
        }
        
        return wind;
    }

    public float Passive(string id, bool isRunning)
    {
        float wind;

        if (IsFinalRound)
        {
            wind = id switch
            {
                Const_Items.MyMaskId => myMaskFactors.passive,
                _ => isRunning ? defaultRunFactors.passive : defaultFactors.passive,
            };
        }
        else
        {
            wind = id switch
            {
                Const_Items.IceSpikeId => snowWomanFactors.passive,
                _ => isRunning ? defaultRunFactors.passive : defaultFactors.passive,
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