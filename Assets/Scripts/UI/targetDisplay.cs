using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class targetDisplay : MonoBehaviour
{
    [SerializeField]
    private healthbar healthbar;
    [SerializeField]
    private TMP_Text targetName;
    [SerializeField]
    private GameObject basicAttacking;
    [SerializeField]
    private GameObject following;
    private actor currentTarget;
    private actor pc;

    // Start is called before the first frame update
    void Awake()
    {
        subscribeToPossessionChanges();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setPossesseeTarget(actor newTarget)
    {
        currentTarget = newTarget;
        if (currentTarget)
        {
            healthbar.setCharacter(newTarget);
            healthbar.gameObject.SetActive(true);
        } else
        {
            healthbar.gameObject.SetActive(false);
        }
    }

    private void setFollowing(bool f)
    {
        following.SetActive(f);
    }

    private void setBasicAttacking(bool ba)
    {
        basicAttacking.SetActive(ba);
    }

    public void setPossessee(actor newPossession)
    {
        if (pc)
        {
            pc.targetChangeEvent -= setPossesseeTarget;
            pc.followingChangeEvent -= setFollowing;
            pc.basicAttackingChangeEvent -= setBasicAttacking;
        }
        pc = newPossession;
        setPossesseeTarget(pc.getTarget());
        pc.targetChangeEvent += setPossesseeTarget;
        setFollowing(pc.getFollowing());
        pc.followingChangeEvent += setFollowing;
        setBasicAttacking(pc.getBasicAttacking());
        pc.basicAttackingChangeEvent += setBasicAttacking;

    }

    public void subscribeToPossessionChanges()
    {
        playerGhost.Instance.possessionChangeEvent += setPossessee;
    }
}
