using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitchReader : MonoBehaviour
{
    [SerializeField]
    private playerGhost playerGhost;
    // wow, this is dumb
    public void jumpToCharacter1(InputAction.CallbackContext context)
    {
        playerGhost.setPossessingIndex(0);
    }
    public void jumpToCharacter2(InputAction.CallbackContext context)
    {
        playerGhost.setPossessingIndex(1);
    }
    public void jumpToCharacter3(InputAction.CallbackContext context)
    {
        playerGhost.setPossessingIndex(2);
    }
    public void jumpToCharacter4(InputAction.CallbackContext context)
    {
        playerGhost.setPossessingIndex(3);
    }
}
