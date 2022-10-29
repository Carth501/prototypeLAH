using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    public delegate void enterDelegate(actor other);
    public event enterDelegate enterEvent;
    public delegate void exitDelegate(actor other);
    public event exitDelegate exitEvent;

    [SerializeField]
    private actor self;
    private List<actor> actorsInArea = new List<actor>();

    private void Start()
    {
        if(self == null)
        {
            Debug.LogError("no controller set for " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        actor character = searchForActor(other);
        if(character != null)
        {
            addActorsInArea(character);
            if (enterEvent != null)
                enterEvent(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        actor otherController = searchForActor(other);
        if (otherController != null)
        {
            if (exitEvent != null)
                exitEvent(otherController);
        }
    }

    private actor searchForActor(Collider other)
    {
        actor otherController;
        if (other.TryGetComponent(out otherController))
        {
            if (otherController == self)
            {
                otherController = null;
            }
        }
        return otherController;
    }
    #region actors in area
    public List<actor> getListActorsInArea()
    {
        return actorsInArea;
    }

    public void addActorsInArea(actor newEntry)
    {
        if (!actorsInArea.Contains(newEntry))
        {
            actorsInArea.Add(newEntry);
        }
    }

    public void removeActorsInArea(actor entryToBeRemoved)
    {
        if (actorsInArea.Contains(entryToBeRemoved))
        {
            actorsInArea.Remove(entryToBeRemoved);
        }
    }

    public bool actorsInAreaContains(actor entry)
    {
        return actorsInArea.Contains(entry);
    }

    public void clearActorsInArea()
    {
        actorsInArea.Clear();
    }
    #endregion
}
