using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Script_TMProPopulator))]
public class Script_TMProRandomizer : MonoBehaviour
{
    [SerializeField] private List<string> ids;
    [SerializeField] private float timerMax;
    [SerializeField] private Script_TMProZalgofy tmproZalgofy;
    
    private Script_TMProPopulator TMProPopulator;
    private float timer;
    private string defaultId;

    public string DefaultId
    {
        get => defaultId;
        set => defaultId = value;
    }
    
    void Awake()
    {
        TMProPopulator = GetComponent<Script_TMProPopulator>();
        
        if (String.IsNullOrEmpty(DefaultId))
            DefaultId = TMProPopulator.Id;
    }

    void OnEnable()
    {
        InitialState();
    }

    void OnDisable()
    {
        TMProPopulator.UpdateTextId(DefaultId);
    }
    
    void Update()
    {
        timer -= Time.unscaledDeltaTime;

        if (timer <= 0f)
        {
            if (tmproZalgofy != null)
                tmproZalgofy.Zalgofy();
            else
                HandleRandomIdSwitch();

            timer = timerMax;
        }
    }

    private void HandleRandomIdSwitch()
    {
        int randomIdx = UnityEngine.Random.Range(0, ids.Count);
        string newId = ids[randomIdx];
        TMProPopulator.UpdateTextId(newId);
    }

    private void InitialState()
    {
        timer = 0f;
    }
}
