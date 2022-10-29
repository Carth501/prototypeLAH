using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownCurtain : MonoBehaviour
{
    private float endTimestamp = 0;
    private float startTimestamp = 0;
    private skill skill;
    [SerializeField]
    private RectTransform rect;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < endTimestamp)
        {
            rect.sizeDelta = new Vector2(64, percentCooldownRemaining() * 64);
        }
    }

    public void setSkill(skill skill)
    {
        this.skill = skill;
        subscribe();
    }

    public void startCooldown(float startTimestamp, float endTimestamp)
    {
        this.endTimestamp = endTimestamp;
        this.startTimestamp = startTimestamp;
    }

    public float percentCooldownRemaining()
    {
        float cooldownRemaining = 0;
        if (Time.time < endTimestamp && endTimestamp != startTimestamp)
        {
            float duration = endTimestamp - startTimestamp;
            float remaining = duration - (Time.time - startTimestamp);
            cooldownRemaining = remaining / duration;
        }
        return cooldownRemaining;
    }

    public void subscribe()
    {
        if(skill != null)
        {
            skill.CooldownEvent -= startCooldown;
        }
        skill.CooldownEvent += startCooldown;
    }
}
