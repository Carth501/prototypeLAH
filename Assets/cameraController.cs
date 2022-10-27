using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{
    [SerializeField]
    private float cameraYSensitivity;
    [SerializeField]
    private float cameraXSensitivity;
    [SerializeField]
    private float yMinAngle;
    [SerializeField]
    private float yMaxAngle;
    [SerializeField]
    private float cameraDistance;
    private float cameraX = 0;
    private float cameraY = 45;
    private Vector3 cameraOffset = new Vector3(0, 0, 0);
    private RaycastHit obstructionDetector;
    private controller activeCharacter;
    private bool cameraControl;

    public void Awake()
    {
        subscribeToPossessionChanges();
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        recalculateCameraOffset();
    }

    public void cameraChangeAngle(InputAction.CallbackContext context)
    {
        if (cameraControl)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            cameraX += delta.x * cameraXSensitivity * 0.05f;
            cameraX %= 360;

            cameraY -= delta.y * cameraYSensitivity * 0.05f;
            cameraY = Mathf.Clamp(cameraY, yMinAngle, yMaxAngle);
        }
    }

    public void cameraDolly(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<Vector2>().y * -0.002f;
        cameraDistance = Math.Clamp(scroll + cameraDistance, 3, 20);
    }

    public void recalculateCameraOffset()
    {
        Quaternion rotation = Quaternion.Euler(cameraY, cameraX, 0);

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -cameraDistance);
        Vector3 position = rotation * negDistance + activeCharacter.transform.position;

        int ignoredLayers = 9; // 9 is the actors layer, since actors should never obstruct the view, since they will be moving too much, and one should probably be paying attention to them anyway.
        if (Physics.Linecast(activeCharacter.transform.position, position, out obstructionDetector, ignoredLayers))
        {
            Debug.Log("Hitting something?");
            float adjustedDistance = Vector3.Distance(obstructionDetector.point, activeCharacter.transform.position) - 1;
            negDistance = new Vector3(0.0f, 0.0f, -adjustedDistance);
            position = rotation * negDistance + activeCharacter.transform.position;
        }

        transform.rotation = rotation;
        transform.position = position;
    }

    public void setActivateCameraControl(InputAction.CallbackContext context)
    {
        cameraControl = !(context.phase == InputActionPhase.Canceled);
    }

    public void setActiveCharacter(controller pc)
    {
        activeCharacter = pc;
    }

    public void subscribeToPossessionChanges()
    {
        playerGhost.Instance.possessionChangeEvent += setActiveCharacter;
    }
}
