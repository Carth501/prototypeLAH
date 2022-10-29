using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBars : MonoBehaviour
{
    [SerializeField]
    private healthbar healthBar;
    [SerializeField]
    private EnergyBar energyBar;


    // Start is called before the first frame update
    void Start()
    {
        subscribeToPossessionChanges();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void setPossessee(actor newPossession)
    {
        healthBar.setCharacter(newPossession);
        energyBar.setCharacter(newPossession);
    }

    public void subscribeToPossessionChanges()
    {
        playerGhost.Instance.possessionChangeEvent += setPossessee;
    }
}
