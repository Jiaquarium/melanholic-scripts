using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_Demon))]
public class Script_DemonIntervalAttackController : MonoBehaviour
{
    [SerializeField] private float attackInterval;
    [SerializeField] private float initialTimeInterval;

    [SerializeField] private bool isDisabled;

    private float timer;

    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }

    void OnEnable()
    {
        timer = initialTimeInterval;
    }
    
    private void Update()
    {
        if (!IsDisabled)
            AttackTimer();
    }

    private void AttackTimer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Attack();
            
            timer = attackInterval;
        }
    }

    private void Attack()
    {
        GetComponent<Script_Demon>().Attack();
    }
}
