using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class InteractPerson : Interactable {

    private DisplayManager displayManager;

    private UnityAction DialogActionA;
    private UnityAction DialogActionB;
    private UnityAction DialogActionC;
    private int iterationNumber = 1;
    private bool lastDialogue = false;
    private bool endDialogue = false;

    public int life = 100;

    public override void Interact() {
        base.Interact();
        DialogActionA = new UnityAction(DialogFunctionA);
        DialogActionB = new UnityAction(DialogFunctionB);
        DialogActionC = new UnityAction(DialogFunctionC);
        int iterNumPlus1 = iterationNumber + 1;

        if (!endDialogue && DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterationNumber)) {
            PlayerController.playerControllerInstance.isTalking = true;
            PlayerController.playerControllerInstance.playerControl = false;
            string question = DialogManager.dialogueDB[this.transform.gameObject.name + "_" + iterationNumber];
            if (DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterNumPlus1)) {
                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name);
                iterationNumber++;
            } else {
                lastDialogue = true;
                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name);
                endDialogue = true;
            }
        }
    }

    void DialogFunctionA()
    {
        DisplayManager.displayManagerInstance.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "AR"]);
        PlayerController.playerControllerInstance.playerControl = true;
        PlayerController.playerControllerInstance.isTalking = false;

    }

    void DialogFunctionB()
    {
        DisplayManager.displayManagerInstance.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "BR"]);
        PlayerController.playerControllerInstance.playerControl = true;
        PlayerController.playerControllerInstance.isTalking = false;
    }

    void DialogFunctionC()
    {
        DisplayManager.displayManagerInstance.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "CR"]);
        PlayerController.playerControllerInstance.playerControl = true;
        PlayerController.playerControllerInstance.isTalking = false;

    }


}
