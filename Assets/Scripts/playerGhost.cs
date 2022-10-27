using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerGhost : Singleton<playerGhost>
{
    //The spirit of the player possesses the pcControllers
    //It is unseen, untargetable, unkillable
    public delegate void possessionChangeDelegate(controller possessee);
    public event possessionChangeDelegate possessionChangeEvent;

    [SerializeField]
    private controller[] characters;
    private int possessingIndex = 0;
    private controller activeCharacter;
    [SerializeField]
    private Camera cameraObject;
    [SerializeField]
    private targetDisplay targetDisplay;
    [SerializeField]
    private cameraController cameraController;
    [SerializeField]
    private ActionActivationDisplay pcActionActivationDisplay;
    [SerializeField]
    private ActionQueueUI actionQueueUI;
    [SerializeField]
    private Skillbar skillbar;

    // Start is called before the first frame update
    void Start()
    {
        activeCharacter = characters[possessingIndex];
        setNewPossesson();
    }

    public void setTarget(controller target)
    {
        activeCharacter.setTarget(target);
    }

    public void moveToPoint()
    {
        Vector3? point = tryToFindPoint();
        if (point == null)
        {
            Debug.LogWarning("destination point not found. How did you target something without hitting it?");
        }
        else
        {
            Vector3 guarenteedToBeNotNullPoint = (Vector3)point;
            activeCharacter.tryToMoveToTerrainPoint(guarenteedToBeNotNullPoint);
        }
    }

    private Vector3? tryToFindPoint()
    {
        RaycastHit hit;
        Ray ray = cameraObject.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }

    public void manualMove(Vector3 move)
    {
    }

    public void manualMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        Vector3 move3D = new Vector3(value.x, 0, value.y);
        manualMove(move3D);
    }

    public void cyclePossession(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            changePossessingIndex(1);
        }
    }

    public void setPossessingIndex(int newIndex)
    {
        if (newIndex != possessingIndex)
        {
            possessingIndex = newIndex;
            setNewPossesson();
        }
    }

    public void setPossessingIndex(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            setPossessingIndex(context.ReadValue<int>());
        }
    }

    public void changePossessingIndex(int delta)
    {
        possessingIndex = (possessingIndex += delta) % 4;
        setNewPossesson();
    }

    public void setNewPossesson()
    {
        activeCharacter = characters[possessingIndex];
        possessionChangeEvent(activeCharacter);
    }
}
