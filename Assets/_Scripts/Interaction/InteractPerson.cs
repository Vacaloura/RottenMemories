using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteractPerson : Interactable {

    private DisplayManager displayManager;

    private UnityAction myYesAction;
    private UnityAction myNoAction;
    private UnityAction myCancelAction;

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

    void OnCollisionEnter(Collision col)
    {
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();

        if (col.gameObject.tag == "Arrow")
        {
            displayManager.DisplayMessage("Collided with" + col.gameObject.name);
            col.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            col.gameObject.transform.parent = this.transform;
        }
    }
}
