using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class selectable : MonoBehaviour
{
    [SerializeField]
    private selectableType type;
    [SerializeField]
    private actor character;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) { 
            if (type == selectableType.terrain)
            {
                playerGhost.Instance.moveToPoint();
            }
            else if (type == selectableType.selectable)
            {
                playerGhost.Instance.setTarget(character);
            }
        }
    }
}
