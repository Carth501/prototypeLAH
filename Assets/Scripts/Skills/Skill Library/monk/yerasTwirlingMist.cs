using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yerasTwirlingMist : skill
{
    [SerializeField]
    private float damage = -14;
    // Start is called before the first frame update
    void Start()
    {
        activationDuration = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void activate(actor target)
    {
        if (target)
        {
            target.changeCurrentHealth(damage);
        }
        else
        {
            Debug.Log("no target");
        }
    }
}
