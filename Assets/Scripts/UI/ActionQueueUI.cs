using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueueUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] queueSlots;
    private List<action> actions = new List<action>();
    private actor character;

    // Start is called before the first frame update
    void Awake()
    {
        subscribeToPossessionChanges();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setCharacter(controller pc)
    {
        unsub();
        character = pc.getSelf();
        Queue<(action, actor)> queue = character.getActionQueue();
        createQueueUIFromQueue(character.getActionQueue());
        subscribe();
    }

    private void clearQueueUI()
    {
        foreach(action action in actions)
        {
            Destroy(action.gameObject);
        }
        actions.Clear();
    }

    private void addToQueueUI(action queueingAction, int index)
    {
        if(index < 4)
        {
            actions.Add(Instantiate(queueingAction, transform));
        }
    }
    private void removeFromQueueUI(int index)
    {
        action toBeDestroyed = actions[index];
        actions.Remove(toBeDestroyed);
        Destroy(toBeDestroyed.gameObject);
        drawQueueUI();
    }

    private void createQueueUIFromArray((action, actor)[] queue)
    {
        clearQueueUI();
        int i = 0;
        foreach ((action, actor) actionEntry in queue)
        {
            addToQueueUI(actionEntry.Item1, i);
            i++;
        }
        drawQueueUI();
    }

    private void drawQueueUI()
    {
        int i = 0;
        foreach(action action in actions)
        {
            action.transform.position = queueSlots[i].transform.position;
            if (i == 0)
            {
                action.resizeIcon(0.75f);
            }
            else
            {
                action.resizeIcon(0.5f);
            }
            i++;
        }
    }

    private void createQueueUIFromQueue(Queue<(action, actor)> actionQueue)
    {
        createQueueUIFromArray(actionQueue.ToArray());
    }
    private void enqueue(action enqueueing)
    {
        addToQueueUI(enqueueing, actions.Count);
    }
    private void dequeue()
    {
        removeFromQueueUI(0);
    }

    private void subscribe()
    {
        character.skillQueueChangeEvent += createQueueUIFromQueue;
    }

    private void unsub()
    {
        if(character != null)
        {
            character.skillQueueChangeEvent -= createQueueUIFromQueue;
        }
    }

    public void subscribeToPossessionChanges()
    {
        playerGhost.Instance.possessionChangeEvent += setCharacter;
    }
}
