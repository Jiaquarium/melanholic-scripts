using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TriggerStayEvent : UnityEvent<string>
{
}

public class Script_TriggerPlayerStay : Script_Trigger
{
    public Script_Game game;
    [SerializeField] private TriggerStayEvent triggerStayEvent;
    
    void OnTriggerStay(Collider other)
    {
       if (other.tag == Const_Tags.Player)
       {
            if (triggerStayEvent.CheckUnityEvent())     triggerStayEvent.Invoke(Id);
       }     
    }
}
