using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillbar : MonoBehaviour
{
    [SerializeField]
    private skillSlot[] skillSlots;

    // Start is called before the first frame update
    void Awake()
    {
        subscribeToPossessionChanges();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void changePossessee(controller pc)
    {
        clearSkillbar();
        setUISkillbar(pc);
    }

    public void setUISkillbar(controller pc)
    {
        skill[] skills = pc.getSkillbar();
        int i = 0;
        foreach (skill skill in skills)
        {
            if (i < 8 && skill != null)
            {
                skillSlots[i].setSkill(skill, pc);
                i++;
            }
        }
    }
    public void clearSkillbar()
    {
        foreach (skillSlot slot in skillSlots)
        {
            slot.removeSkill();
        }
    }

    public void subscribeToPossessionChanges()
    {
        playerGhost.Instance.possessionChangeEvent += changePossessee;
    }
}
