using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteractPerson : Interactable {

    private DisplayManager displayManager;

    private UnityAction myYesAction;
    private UnityAction myNoAction;
    private UnityAction myCancelAction;

    [HideInInspector] public int life = 100;

    public override void Interact() {
        base.Interact();
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
        myYesAction = new UnityAction(YesFunction);
        myNoAction = new UnityAction(NoFunction);
        myCancelAction = new UnityAction(CancelFunction);
        GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction);
    }

    void YesFunction()
    {
        displayManager.DisplayMessage("Heck yeah! Yup!");
    }

    void NoFunction()
    {
        displayManager.DisplayMessage("No way, José!");
    }

    void CancelFunction()
    {
        displayManager.DisplayMessage("I give up!");
    }

    
}
