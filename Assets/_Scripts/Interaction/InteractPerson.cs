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
    private string selectedAnswer;

    public int life = 100;

    public override void Interact() {
        base.Interact();
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
        DialogActionA = new UnityAction(DialogFunctionA);
        DialogActionB = new UnityAction(DialogFunctionB);
        DialogActionC = new UnityAction(DialogFunctionC);
        int iterNumPlus1 = iterationNumber + 1;

        if (DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterationNumber)) {
            GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = false;

            string question = DialogManager.dialogueDB[this.transform.gameObject.name + "_" + iterationNumber];
            if (!endDialogue && DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterNumPlus1)) {
                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name);
                iterationNumber++;
            } else if (endDialogue) {
                switch (selectedAnswer) {
                    case "A":
                        DialogFunctionA();
                        break;
                    case "B":
                        DialogFunctionB();
                        break;
                    case "C":
                        DialogFunctionC();
                        break;
                }
            } else {
                lastDialogue = true;
                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name);
                endDialogue = true;
            }
        }
    }

    void DialogFunctionA() {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "AR"]);
        GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
        selectedAnswer = "A";
    }

    void DialogFunctionB() {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "BR"]);
        GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
        selectedAnswer = "B";
    }

    void DialogFunctionC() {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "CR"]);
        GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
        selectedAnswer = "C";
    }


}
