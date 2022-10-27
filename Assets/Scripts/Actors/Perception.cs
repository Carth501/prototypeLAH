using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    public delegate void enterDelegate(controller other);
    public event enterDelegate enterEvent;
    public delegate void exitDelegate(controller other);
    public event exitDelegate exitEvent;

    [SerializeField]
    private controller self;

    private void Start()
    {
        if(self == null)
        {
            Debug.LogError("no controller set for " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        controller otherController = searchForActor(other);
        if(otherController != null)
        {
            if(enterEvent != null)
                enterEvent(otherController);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        controller otherController = searchForActor(other);
        if (otherController != null)
        {
            if (exitEvent != null)
                exitEvent(otherController);
        }
    }

    private controller searchForActor(Collider other)
    {
        controller otherController;
        if (other.TryGetComponent(out otherController))
        {
            if (otherController == self)
            {
                otherController = null;
            }
        }
        return otherController;
    }
}
