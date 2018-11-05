using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteractPerson : Interactable {

    private DisplayManager displayManager;

    private UnityAction myYesAction;
    private UnityAction myNoAction;
    private UnityAction myCancelAction;
    private int iterationNumber = 1;
    private bool lastDialogue = false;
    private bool endDialogue = false;

    public int life = 100;

    public override void Interact() {
        base.Interact();
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
        myYesAction = new UnityAction(YesFunction);
        myNoAction = new UnityAction(NoFunction);
        myCancelAction = new UnityAction(CancelFunction);
        int iterNumPlus1 = iterationNumber + 1;

        if (!endDialogue) {
            string question = DialogManager.dialogueDB[this.transform.gameObject.name + "_" + iterationNumber];
            if (DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterNumPlus1)) {
                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, myYesAction, myNoAction, myCancelAction, lastDialogue);
                iterationNumber++;
            } else {
                lastDialogue = true;
                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, myYesAction, myNoAction, myCancelAction, lastDialogue);
                endDialogue = true;
            }
        }
    }

    void YesFunction()
    {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "Yes"]);
    }

    void NoFunction()
    {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "No"]);
    }

    void CancelFunction()
    {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "Cancel"]);
    }

    
}
