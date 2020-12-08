using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Must be the parent of Scarlet Cipher Slots
/// </summary>
public class Script_ScarletCipherController : MonoBehaviour
{
    [SerializeField] private Script_ScarletCipher scarletCipher;
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
        
        for (int i = 0; i < Script_ScarletCipher.QuestionCount; i++)
        {
            slots[i].Id = i;
            
            bool isVisible = scarletCipher.ScarletCipherVisibility[i];
            int? cipherCode = null;
            if (isVisible)  cipherCode = scarletCipher.ScarletCipher[i];
            slots[i].Setup(cipherCode);
        }
    }
}
