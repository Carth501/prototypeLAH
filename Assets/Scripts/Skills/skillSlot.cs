using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. UI;


public class skillSlot : MonoBehaviour
{
    private skill skillInstance;
    private actor pc;
    [SerializeField]
    private CooldownCurtain curtain;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enqueue()
    {
        if(skillInstance)
        {
            pc.enqueueAction(skillInstance, pc.getTarget());
        }
    }

    public void setSkill(skill skill, actor pc)
    {
        if (skill && pc)
        {
            skillInstance = Instantiate(skill, transform);
            skillInstance.transform.SetSiblingIndex(0);
            curtain.setSkill(skillInstance);
            this.pc = pc;
        }
    }

    public void removeSkill()
    {
        if(skillInstance != null)
        {
            Destroy(skillInstance.gameObject);
            skillInstance = null;
            pc = null;
        }
    }
}
