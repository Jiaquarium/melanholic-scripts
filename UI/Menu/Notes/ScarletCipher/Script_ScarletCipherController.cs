using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Must be the parent of Scarlet Cipher Slots
/// </summary>
public class Script_ScarletCipherController : MonoBehaviour
{
    [SerializeField] private Script_ScarletCipherManager scarletCipherManager;
    [SerializeField] private Script_ScarletCipherSlot[] slots;
    
    void OnValidate()
    {
        SetupSlots();    
    }
    
    void OnEnable()
    {
        SetupSlots();
    }

    private void SetupSlots()
    {
        slots = GetComponentsInChildren<Script_ScarletCipherSlot>(true);
        
        for (int i = 0; i < Script_ScarletCipherManager.QuestionCount; i++)
        {
            slots[i].Id = i;
            
            bool isVisible = scarletCipherManager.ScarletCipherVisibility[i];
            int? cipherCode = null;
            if (isVisible)  cipherCode = scarletCipherManager.ScarletCipher[i];
            slots[i].Setup(cipherCode);
        }
    }
}
