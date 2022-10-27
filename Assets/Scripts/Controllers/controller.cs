using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public delegate void targetChangeDelegate(actor target);
    public event targetChangeDelegate targetChangeEvent;
    public delegate void followingChangeDelegate(bool f);
    public event followingChangeDelegate followingChangeEvent;

    [SerializeField]
    private actor self;
    protected actor following;
    [SerializeField]
    private skill[] skillbar;
    [SerializeField]
    private Perception awareness;
    private List<controller> controllersInArea = new List<controller>();
    protected List<actor> hostiles = new List<actor>(); // things hostile to this
    protected actor target; // targetting should be handled by the controller, as it's more of a mind thing
    [SerializeField]
    private Personality identity;

    // Start is called before the first frame update
    void Start()
    {
        awareness.enterEvent += addControllerInArea;
        awareness.exitEvent += removeControllerInArea;
        self.skillActivationFinalizeEvent += keepAttackingIfHostile;
    }

    // Update is called once per frame
    void Update()
    {
        self.combatUpdate(target);
    }

    public void setTarget(controller newTarget)
    {
        actor newActor = newTarget.getSelf();
        if (newActor == target)
        {
            tryToInteractWith(newTarget);
        }
        else
        {
            setNewTarget(newActor);
        }
    }
    protected void setNewTarget(actor newActor)
    {
        if (following)
        {
            self.resetPath();
        }
        if (target != null)
        {
            stopWatchingTargetForDeath();
        }
        setTarget(newActor);
        targetChangeEvent(newActor);
        if(newActor != null)
        {
            watchForTargetDeath();
        }
    }

    private void stopWatchingTargetForDeath()
    {
        target.aliveStateChangeEvent -= targetLifeStateChange;
    }

    public void watchForTargetDeath()
    {
        target.aliveStateChangeEvent += targetLifeStateChange;
    }
    private void targetLifeStateChange(bool alive)
    {
        // it shouldn't stop watching for life state change. It should maintain target even after death.
        if (alive)
        {

        } else {
            setFollowActor(null);
            self.setBasicAttackCycle(false);
        }
    }

    public void tryToMoveToTerrainPoint(Vector3 point)
    {
        setFollowActor(null);
        self.setDestinationClear(point, constantDictionary.terrainMove);
        self.setBasicAttackCycle(false);
    }

    public void tryToInteractWith(controller target)
    {
        // don't just move to their position, follow them!
        self.setStoppingDistance(constantDictionary.interactionRadius);
        setFollowActor(target.getSelf());
        if (target.self.getAliveState() && target.getPersonality().isHostile(identity.getCoterieName())) 
            // are they alive, and are they hostile to me?
        {
            self.setBasicAttackCycle(true);
            self.stopInBasicAttackRange();
        }
    }

    public actor getSelf()
    {
        return self;
    }

    public virtual void setFollowActor(actor actor)
    {
        following = actor;
        if (following)
        {
            self.setFollowDestination(following.transform.position, following);
            StartCoroutine(follow());
        }
        if(followingChangeEvent != null)
            followingChangeEvent(actor != null);
    }

    private IEnumerator follow()
    {
        while (following != null)
        {
            yield return new WaitForSeconds(0.4f);
            if (following != null)
            {
                bool arrived = self.setFollowDestination(following.transform.position, following);
                if (arrived)
                {
                    self.resetPath();
                    following = null;
                }
            }
        }
    }

    public void clearTarget()
    {
        target = null;
    }

    public actor getTargetActor()
    {
        return target;
    }

    public bool getFollowing()
    {
        return following != null;
    }

    public skill[] getSkillbar()
    {
        return skillbar;
    }

    #region actors in area

    public List<controller> getListControllersInArea()
    {
        return controllersInArea;
    }

    public void addControllerInArea(controller newEntry)
    {
        if(!controllersInArea.Contains(newEntry))
        {
            controllersInArea.Add(newEntry);
            if (newEntry.getPersonality().isHostile(identity.getCoterieName()))
            {
                addHostile(newEntry.getSelf());
            }
        }
    }

    public void removeControllerInArea(controller entryToBeRemoved)
    {
        if (controllersInArea.Contains(entryToBeRemoved))
        {
            controllersInArea.Remove(entryToBeRemoved);
        }
    }

    public bool controllerInAreaContains(controller entry)
    {
        return controllersInArea.Contains(entry);
    }

    public void clearActorsInArea()
    {
        controllersInArea.Clear();
    }
    #endregion

    public void addHostile(actor newHostile)
    {
        if (!hostiles.Contains(newHostile))
        {
            hostiles.Add(newHostile);
            newHostile.characterAliveChangeEvent += removeHostile;
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

    #region targetting
    public actor getTarget()
    {
        return target;
    }
    public void setTarget(actor newTarget)
    {
        target = newTarget;
    }
    #endregion

    public Personality getPersonality()
    {
        return identity;
    }

    private void keepAttackingIfHostile((action, actor, float, float) finalizedAction)
    {
        if(finalizedAction.Item2)
    }
}
