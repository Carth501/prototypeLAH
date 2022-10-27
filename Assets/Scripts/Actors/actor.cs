using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actor : MonoBehaviour
{
    public delegate void aliveStateChangeDelegate(bool alive);
    public event aliveStateChangeDelegate aliveStateChangeEvent;
    public delegate void characterAliveChangeDelegate(actor character);
    public event characterAliveChangeDelegate characterAliveChangeEvent;
    public delegate void takeDamageDelegate();
    public event takeDamageDelegate takeDamageEvent;
    public delegate void skillEnqueueDelegate(action enqueuedAction);
    public event skillEnqueueDelegate skillEnqueueEvent;
    public delegate void skillDequeueDelegate();
    public event skillDequeueDelegate skillDequeueEvent;
    public delegate void skillQueueChangeDelegate(Queue<(action, actor)> actionQueue);
    public event skillQueueChangeDelegate skillQueueChangeEvent;
    public delegate void skillActivationStartDelegate((action, actor, float, float) beginningActionActivation);
    public event skillActivationStartDelegate skillActivationStartEvent;
    public delegate void skillActivationFinishDelegate();
    public event skillActivationFinishDelegate skillActivationFinishEvent;
    public delegate void skillActivationCanceledDelegate();
    public event skillActivationCanceledDelegate skillActivationCanceledEvent;
    public delegate void skillActivationFinalizeDelegate((action, actor, float, float) finalizedAction);
    public event skillActivationFinalizeDelegate skillActivationFinalizeEvent;
    public delegate void basicAttackingChangeDelegate(bool ba);
    public event basicAttackingChangeDelegate basicAttackingChangeEvent;
    public delegate void basicAttackDelegate();
    public event basicAttackDelegate basicAttackEvent;

    [SerializeField]
    private string diegeticName;
    [SerializeField]
    private bool alive;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private int maxEnergy;
    [SerializeField]
    private float currentEnergy;
    [SerializeField]
    private float basicAttackRange;
    [SerializeField]
    private float basicAttackDamage;
    [SerializeField]
    private float basicAttackFrequency;
    private Queue<(action, actor)> actionQueue = new Queue<(action, actor)>();
    [SerializeField]
    private double nextActionDequeueTime;
    private (action, actor, float, float) activatingAction; // action being activated, target actor's controller, start timestamp, completion timestamp
    [SerializeField]
    protected UnityEngine.AI.NavMeshAgent navAgent;
    private bool basicAttackCycle = false;
    private double? nextBasicAttackTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float getCharacterRadius()
    {
        return navAgent.radius;
    }

    public static float getDistanceBetweenActors(actor actor1, actor actor2)
    {
        float absoluteDistance = Vector3.Distance(actor1.transform.position, actor2.transform.position);
        return absoluteDistance - actor1.getCharacterRadius() - actor2.getCharacterRadius();

    }

    public string getDiegeticName()
    {
        return diegeticName;
    }

    #region health functions
    public float getPercentHealth()
    {
        return currentHealth / maxHealth;
    }

    public (float, int) getHealth()
    {
        return (currentHealth, maxHealth);
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void changeMaxHealth(int delta)
    {
        maxHealth += delta;
        changeCurrentHealth(delta);
    }

    public void changeCurrentHealth(float delta)
    {
        currentHealth += delta;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);
        if (delta < 0)
        {
            takeDamageEvent();
        }
        if (currentHealth <= 0 && alive)
        {
            alive = false;
            aliveStateChangeEvent(alive);
            characterAliveChangeEvent(this);
        }
    }
    #endregion

    #region energy functions
    public float getPercentEnergy()
    {
        return currentEnergy / maxEnergy;
    }

    public (float, int) getEnergy()
    {
        return (currentEnergy, maxEnergy);
    }

    public float getCurrentEnergy()
    {
        return currentEnergy;
    }

    public void changeMaxEnergy(int delta)
    {
        maxEnergy += delta;
        changeCurrentEnergy(delta);
    }

    public void changeCurrentEnergy(float delta)
    {
        currentEnergy += delta;
        currentEnergy = Math.Min(currentEnergy, maxEnergy);
    }
    #endregion

    #region basic attack functions
    public float getBasicAttackRange()
    {
        return basicAttackRange;
    }
    public float getBasicAttackDamage()
    {
        return basicAttackDamage;
    }
    public float getBasicAttackFrequency()
    {
        return basicAttackFrequency;
    }
    #endregion

    #region life state functions
    public void aliveState(bool alive)
    {
        this.alive = alive;
        aliveStateChangeEvent(alive);
        characterAliveChangeEvent(this);
    }

    public bool getAliveState()
    {
        return alive;
    }

    public void returnToLife(float health)
    {
        aliveState(true);
        changeCurrentHealth(health);
    }
    #endregion

    #region combat functions
    #region queue functions
    public Queue<(action, actor)> getActionQueue()
    {
        return actionQueue;
    }

    public (action, actor) peekSkillQueue()
    {
        return actionQueue.Peek();
    }

    public (action, actor) dequeueSkill()
    {
        (action, actor) nextAction = actionQueue.Dequeue();
        if(skillQueueChangeEvent != null)
        {
            skillQueueChangeEvent(actionQueue);
        }
        return nextAction;
    }

    public void enqueueAction(action enqueueingAction, actor target)
    {
        if(validEnqueue(enqueueingAction))
        {
            actionQueue.Enqueue((enqueueingAction, target));
            skillQueueChangeEvent(actionQueue);
        }
    }

    public void clearActionQueue()
    {
        actionQueue.Clear();
        if (skillQueueChangeEvent != null)
        {
            skillQueueChangeEvent(actionQueue);
        }
    }

    private bool validEnqueue(action enqueueingCandidate)
    {
        bool validAction = !alreadyInQueue(enqueueingCandidate) && !isTheActivatingAction(enqueueingCandidate);
        return actionQueue.Count < 4 && !enqueueingCandidate.isCoolingDown() && validAction;
    }

    private bool alreadyInQueue(action enqueueingCandidate)
    {
        bool inQueue = false;
        (action, actor)[] actionList = actionQueue.ToArray();
        foreach ((action, actor) queuedAction in actionList)
        {
            if(queuedAction.Item1 == enqueueingCandidate)
            {
                inQueue = true;
            }
        }
        return inQueue;
    }
    private bool isTheActivatingAction(action enqueueingCandidate)
    {
        return enqueueingCandidate == activatingAction.Item1;
    }
    #endregion
    #region combat cycle functions
    public void combatUpdate(actor target)
    {
        actionUpdate();
        if (basicAttackCycle)
        {
            if (nextBasicAttackTime < Time.time && !checkIfActionIsActivating())
            {
                if (checkAttackRange(target))
                {
                    basicAttack(target);
                }
                else
                {
                    stopInBasicAttackRange();
                    setDestination(target.transform.position); // This is going to recalculate path every frame, way too much
                }
            }
        }
    }
    
    private void actionUpdate()
    {
        if (checkIfActionIsActivating())
        {
            if (compareActionCompletionTime())
            {
                completeActionActivation();
            }
        } else if (actionQueue.Count > 0 && Time.time >= nextActionDequeueTime)
        {
            (action, actor) act = peekSkillQueue();
            if (checkNextActionInRange(act))
            {
                resetPath();
                beginNextAction();
            } else
            {
                setDestination(act.Item2.transform.position);
            }
        }
    }

    private void completeActionActivation()
    {
        if (skillActivationFinishEvent != null)
        {
            skillActivationFinishEvent();
        }
        activatingAction.Item1.activate(activatingAction.Item2);
        activatingAction.Item1.startCooldown();

        finalizeActionActivation();
    }
    private void cancelActionActivation()
    {
        if (skillActivationCanceledEvent != null)
        {
            skillActivationCanceledEvent();
        }
        finalizeActionActivation();
    }
    private void finalizeActionActivation()
    {
        setNextBasicAttackTime();
        if (skillActivationCanceledEvent != null)
        {
            skillActivationFinalizeEvent(activatingAction);
        }
        activatingAction = (null, null, Time.time, Time.time);
        nextActionDequeueTime = Time.time + constantDictionary.actionGap;
    }

    private bool checkIfActionIsActivating()
    {
        if (activatingAction.Item1 == null || activatingAction.Item2 == null)
        {
            return false;
        }
        return true;
    }
    private bool compareActionCompletionTime()
    {
        return activatingAction.Item4 <= Time.time;
        
    }
    private void beginNextAction()
    {
        (action, actor) nextAction = dequeueSkill();
        activatingAction = (nextAction.Item1, nextAction.Item2, Time.time, nextAction.Item1.getActivationDuration() + Time.time);
        if (skillQueueChangeEvent != null)
        {
            skillQueueChangeEvent(actionQueue);
        }
        if (skillActivationStartEvent != null)
        {
            skillActivationStartEvent(activatingAction);
        }
    }

    public (action, actor, float, float) getActivatingAction()
    {
        return activatingAction;
    }

    private bool checkNextActionInRange((action, actor) act)
    {
        return act.Item1.getActivationRange() >= getDistanceBetweenActors(this, act.Item2);
    }

    #endregion
    #region basic attack
    public bool getBasicAttacking()
    {
        return nextBasicAttackTime != null;
    }

    public virtual void setBasicAttackCycle(bool active)
    {
        if (active)
        {
            basicAttackCycle = true;
            if (nextBasicAttackTime == null)
            {
                nextBasicAttackTime = Time.time;
            }
        }
        else
        {
            basicAttackCycle = false;
            nextBasicAttackTime = null;
        }
        if (basicAttackingChangeEvent != null)
        { 
            basicAttackingChangeEvent(active);
        }
    }

    public void setNextBasicAttackTime()
    {
        nextBasicAttackTime = 1 / getBasicAttackFrequency() + Time.time;
    }

    public void basicAttack(actor target)
    {
        target.changeCurrentHealth(-getBasicAttackDamage());
        basicAttackEvent();
        if (basicAttackCycle)
        {
            setNextBasicAttackTime();
        }
    }

    private bool checkAttackRange(actor target)
    {
        if (target)
        {
            return getBasicAttackRange() >= getDistanceBetweenActors(this, target);
        }
        return false;
    }
    #endregion
    #endregion

    #region movement

    private void setDestination(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }

    private bool setDestination(Vector3 destination, float stopDistance)
    {
        navAgent.stoppingDistance = stopDistance;
        navAgent.SetDestination(destination);
        return stopDistance <= Vector3.Distance(transform.position, destination);
    }

    public bool setFollowDestination(Vector3 destination, actor following)
    {
        cancelActionActivation();
        clearActionQueue();
        setDestination(destination);
        float stopDist = navAgent.stoppingDistance;
        return stopDist >= getDistanceBetweenActors(this, following);
    }

    public bool setDestinationClear(Vector3 destination, float stopDistance)
    {
        cancelActionActivation();
        clearActionQueue();
        setDestination(destination, stopDistance);
        return stopDistance <= Vector3.Distance(transform.position, destination);
    }

    public void setStoppingDistance(float stopDistance)
    {
        navAgent.stoppingDistance = stopDistance;
    }

    public void resetPath()
    {
        navAgent.ResetPath();
    }

    public void stopInBasicAttackRange()
    {
        navAgent.stoppingDistance = basicAttackRange;
    }
    #endregion
}
