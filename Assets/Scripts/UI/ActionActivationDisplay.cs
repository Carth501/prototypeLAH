using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionActivationDisplay : MonoBehaviour
{
    [SerializeField]
    private action skillActivating;
    private float startTime;
    private float duration;
    private float progress;
    private actor actor;
    private float fadeEndTimestamp;
    [SerializeField]
    private GameObject display;
    [SerializeField]
    private GameObject skillIcon;
    [SerializeField]
    private Slider slider;

    private void Awake()
    {
        subscribeToPossessionChanges();
    }

    // Start is called before the first frame update
    void Start()
    {
        progress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (progress < 1.0f) 
        {
            updateProgress();
        }
    }

    private void setActor(controller pc)
    {
        setActor(pc.getSelf());
    }

    private void setActor(actor character)
    {
        if (actor)
        {
            actor.skillActivationStartEvent -= setActionActivating;
            actor.skillActivationFinishEvent -= finishActivation;
            actor.skillActivationCanceledEvent -= cancelActivation;
        }
        actor = character;
        setActionActivating(actor.getActivatingAction());
        character.skillActivationStartEvent += setActionActivating;
        character.skillActivationFinishEvent += finishActivation;
        character.skillActivationCanceledEvent += cancelActivation;
        resetDisplay();
    }

    private void setActionActivating((action, actor, float, float) beginningActionActivation)
    {
        if (checkIsActivationValid(beginningActionActivation))
        {
            resetDisplay();
            display.SetActive(true);
            startTime = beginningActionActivation.Item3;
            duration = beginningActionActivation.Item4 - beginningActionActivation.Item3;
            skillActivating = Instantiate(beginningActionActivation.Item1, skillIcon.transform);
            skillActivating.resizeIcon(0.5f);
        }
    }

    private void finishActivation()
    {
        endActivation();
    }

    private void cancelActivation()
    {
        endActivation();
    }

    private void endActivation()
    {
        resetDisplay();
        display.SetActive(false);
    }

    private void updateProgress()
    {
        progress = (Time.time - startTime) / duration;
        slider.value = progress;
    }
    private bool checkIsActivationValid((action, actor, float, float) Activation)
    {
        if (Activation.Item1 == null || Activation.Item2 == null)
        {
            return false;
        }
        return true;
    }

    private void resetDisplay()
    {
        progress = 0;
        if (skillActivating)
        {
            Destroy(skillActivating.gameObject);
        }
    }

    public void subscribeToPossessionChanges() // a parent should handle this, for this to be reused for both pcs and npcs
    {
        playerGhost.Instance.possessionChangeEvent += setActor;
    }
}
