using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class skillbarHotkeyInterpreter : MonoBehaviour
{
    [SerializeField]
    private skillSlot[] skillSlotButton;

#region skillbar translations
    public void triggerSkill1(InputAction.CallbackContext context)
    {
        triggerSkillbarButton(context, 0);
    }
    public void triggerSkill2(InputAction.CallbackContext context)
    {
        triggerSkillbarButton(context, 1);
    }
    public void triggerSkill3(InputAction.CallbackContext context)
    {
        triggerSkillbarButton(context, 2);
    }
    public void triggerSkill4(InputAction.CallbackContext context)
    {
        triggerSkillbarButton(context, 3);
    }
    public void triggerSkill5(InputAction.CallbackContext context)
    {
        triggerSkillbarButton(context, 4);
    }
    public void triggerSkill6(InputAction.CallbackContext context)
    {
        triggerSkillbarButton(context, 5);
    }
    public void triggerSkill7(InputAction.CallbackContext context)
    {
        triggerSkillbarButton(context, 6);
    }
    public void triggerSkill8(InputAction.CallbackContext context)
    {
        triggerSkillbarButton(context, 7);
    }
#endregion 

    private void triggerSkillbarButton(InputAction.CallbackContext context, int number)
    {
        if(context.started)
        {
            skillSlotButton[number].enqueue();
        }
    }
}
