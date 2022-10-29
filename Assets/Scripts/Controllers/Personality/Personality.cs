using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personality : MonoBehaviour
{
    // ai hostility
    public delegate void inimicalActorEnteredDelegate(actor inimicalActor);
    public event inimicalActorEnteredDelegate inimicalActorEnteredEvent;

    [SerializeField]
    private float focus; // The amount of time this will chase a creature before it gets bored. ???
    [SerializeField]
    private coteriesEnum initialCoterieSetting;
    private coterie coterie;
    [SerializeField]
    private bool aiEnabled = true;
    [SerializeField]
    private bool pcControlled = false;
    [SerializeField]
    private actor body;
    [SerializeField]
    private Perception awareness;
    [SerializeField]
    protected List<actor> hostiles = new List<actor>(); // things hostile to this, should be limited to living/active

    // Start is called before the first frame update
    void Start()
    {
        coterie = coterie.GetCoterie(initialCoterieSetting);
        body.aliveStateChangeEvent += setAI;
        awareness.enterEvent += actorEntersArea;
        //awareness.exitEvent += actorExitsArea; // disabled so the character pursues enemies
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.value > 0.9)
        {
            useRandomSkill();
        }
    }

    public bool isHostile(coteriesEnum targetCoterie)
    {
        if (coterie != null)
        {
            float relations = coterie.getCoterieRelationsNumber(targetCoterie);
            return relations < 0;
        }
        return false;
    }

    public coteriesEnum getCoterieName()
    {
        return coterie.getCoterieName();
    }

    private void startFighting(actor inimicalActor)
    { 
        if(enabled && !body.getTarget() && aiEnabled)
        {
            changeTargets(inimicalActor);
        }
    }

    private void switchToRandomTarget(bool alive)
    {
        List<actor> hostiles = getHostileList();
        if (hostiles.Count > 0 && aiEnabled)
        {
            int targetIndex;
            if(hostiles.Count == 1)
            {
                targetIndex = 0;
            }
            else
            {
                targetIndex = Random.Range(0, hostiles.Count);
            }
            changeTargets(hostiles[targetIndex]);
        }
        else
        {
            body.setTarget(null);
        }
    }

    private void changeTargets(actor target)
    {
        if(body.getTarget() != null)
        {
            target.aliveStateChangeEvent -= switchToRandomTarget;
        }
        body.setTarget(target);
        body.setBasicAttackCycle(target != null);
        if(!pcControlled)
        {
            target.aliveStateChangeEvent += switchToRandomTarget;
        }
    }

    public void setPlayerControlled(bool control)
    {
        pcControlled = control;
        actor target = body.getTarget();
        if (!control && target != null)
        {
            target.aliveStateChangeEvent += switchToRandomTarget;
        }
    }

    private void setAI(bool state)
    {
        aiEnabled = state;
    }

    private void useRandomSkill()
    {
        if (!pcControlled && aiEnabled)
        { 
            actor target = body.getTarget();
            if (target != null)
            {
                skill skill = body.getRandomSkill();
                if (skill != null)
                {
                    body.enqueueAction(skill, target);
                }
            }
        }
    }

    #region hostility
    public void actorEntersArea(actor newEntry)
    {
        if (newEntry.getPersonality().isHostile(getCoterieName()))
        {
            addHostile(newEntry);
        }
    }

    public void actorExitsArea(actor leavingActor)
    {
        if (leavingActor.getPersonality().isHostile(getCoterieName()))
        {
            removeHostile(leavingActor);
        }
    }

    public void addHostile(actor newHostile)
    {
        if (!hostiles.Contains(newHostile))
        {
            hostiles.Add(newHostile);
            newHostile.characterAliveChangeEvent += removeHostile;
            startFighting(newHostile);
        }
    }

    public void removeHostile(actor hostileToBeRemoved)
    {
        if (hostiles.Contains(hostileToBeRemoved))
        {
            hostiles.Remove(hostileToBeRemoved);
            hostileToBeRemoved.characterAliveChangeEvent -= removeHostile;
        }
    }

    public List<actor> getHostileList()
    {
        return hostiles;
    }
    #endregion
}
