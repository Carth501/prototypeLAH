using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class action : MonoBehaviour
{
    public delegate void cooldownSetDelegate(float startTimestamp, float endTimestamp);
    public event cooldownSetDelegate CooldownEvent;

    [SerializeField]
    protected float activationDuration;
    [SerializeField]
    private RectTransform actionIcon;
    [SerializeField]
    protected float activationRange;
    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected float cooldownEndTimestamp = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void activate(actor target)
    {

    }
    public float getActivationDuration()
    {
        return activationDuration;
    }
    public float getActivationRange()
    {
        return activationRange;
    }
    public float getCooldown()
    {
        return cooldown;
    }

    public void resizeIcon(float length)
    {
        actionIcon.localScale = new Vector2(length, length);
    }

    public virtual void startCooldown()
    {
        if (CooldownEvent != null)
        {
            CooldownEvent(Time.time, Time.time + cooldown);
        }
        cooldownEndTimestamp = Time.time + cooldown;
    }

    public bool isCoolingDown()
    {
        return cooldownEndTimestamp >= Time.time;
    }
}
