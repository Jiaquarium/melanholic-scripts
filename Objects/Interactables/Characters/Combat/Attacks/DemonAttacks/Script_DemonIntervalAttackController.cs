using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Script_Demon))]
public class Script_DemonIntervalAttackController : MonoBehaviour
{
    [SerializeField] private float attackInterval;
    [SerializeField] private float timer;

    private void Start()
    {
    }
    
    private void Update()
    {
        AttackTimer();
    }

    private void AttackTimer()
    {
        timer -= Time.smoothDeltaTime;

        if (timer <= 0)
        {
            Attack();
            
            timer = 0;
            timer = attackInterval;
        }
    }

    private void Attack()
    {
        GetComponent<Script_Demon>().Attack();
    }
}
