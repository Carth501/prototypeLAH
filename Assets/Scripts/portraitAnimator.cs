using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portraitAnimator : MonoBehaviour
{
    private bool alive;
    [SerializeField]
    private SpriteRenderer portrait;
    [SerializeField]
    private controller controller;
    [SerializeField]
    private double attackPulseDuration;
    private double? attackPulseEndTime;
    [SerializeField]
    private double damagePulseDuration;
    private double? damagePulseEndTime;

    // Start is called before the first frame update
    void Start()
    {
        alive = controller.getSelf().getAliveState();
        controller.getSelf().aliveStateChangeEvent += changeAliveDisplay;
        controller.getSelf().takeDamageEvent += takeDamagePulse;
        controller.getSelf().basicAttackEvent += attackPulse;
    }

    // Update is called once per frame
    void Update()
    {
        if(attackPulseEndTime < Time.time)
        {
            if(alive)
            {
                portrait.color = Color.white;
            }
            attackPulseEndTime = null;
        }
        if (damagePulseEndTime < Time.time)
        {
            if (alive)
            {
                portrait.color = Color.white;
            }
            damagePulseEndTime = null;
        }
    }

    private void changeAliveDisplay(bool alive)
    {
        this.alive = alive;
        if(alive)
        {
            portrait.color = Color.white;
        } else {
            portrait.color = Color.gray;
        }
    }

    private void attackPulse()
    {
        portrait.color = new Color(0.2f, 0.2f, 0.9f, 0.5f);
        attackPulseEndTime = attackPulseDuration + Time.time;
    }

    private void takeDamagePulse()
    {
        portrait.color = Color.red;
        damagePulseEndTime = damagePulseDuration + Time.time;
    }
}
