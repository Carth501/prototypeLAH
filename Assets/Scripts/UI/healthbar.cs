using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    [SerializeField]
    private barMode mode = barMode.none;
    private actor character;
    [SerializeField]
    private Slider bar;
    [SerializeField]
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(character != null)
        {
            bar.value = character.getPercentHealth();
        }
        if (mode == barMode.number)
        {
            text.text = character.getCurrentHealth().ToString();
        }
    }

    public void setCharacter(actor character)
    {
        this.character = character;
        if (mode == barMode.text)
        {
            text.text = character.getDiegeticName();
        }
    }
}
