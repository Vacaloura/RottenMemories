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

    [HideInInspector] public int PacoIteration = 1;

    public int life = 100;

    public override void Interact() {
        base.Interact();
        displayManager = GameObject.Find(Names.managers).GetComponent<DisplayManager>();
        DialogActionA = new UnityAction(DialogFunctionA);
        DialogActionB = new UnityAction(DialogFunctionB);
        DialogActionC = new UnityAction(DialogFunctionC);
        int iterNumPlus1 = iterationNumber + 1;
        Debug.Log("Interact");
        if(gameObject.name == "Paco")
        {
            Debug.Log("Entra " + PacoIteration);
            switch (PacoIteration)
            {
                case 1:
                    if (DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterationNumber))
                    {
                        if (PlayerController.playerControllerInstance.isMadeUp)
                        {
                            GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = false;

                            string question = DialogManager.dialogueDB[this.transform.gameObject.name + "_" + iterationNumber];
                            if (!endDialogue && DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterNumPlus1))
                            {
                                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name, 3);
                                iterationNumber++;
                            }
                            else if (endDialogue)
                            {
                                switch (selectedAnswer)
                                {
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
                            }
                            else
                            {
                                lastDialogue = true;
                                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name, 2);
                                endDialogue = true;
                            }
                        }
                        else
                        {
                            PlayerController.playerControllerInstance.PlayerDeath("Te han descubierto! \nLástima. Me caías bien. Cuidaré de tu gato. Ahora muere.");
                        }
                    }
                    break;

                case 2:
                    if (DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "__" + iterationNumber))
                    {
                        if (PlayerController.playerControllerInstance.isMadeUp)
                        {
                            GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = false;

                            string question = DialogManager.dialogueDB[this.transform.gameObject.name + "__" + iterationNumber];
                            if (!endDialogue && DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "__" + iterNumPlus1))
                            {
                                GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name, 3);
                                iterationNumber++;
                            }
                            else
                            {
                                DialogManager.modalPanel.ClosePanel();
                                displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "__" + iterationNumber]);
                                Debug.Log("Sí");
                                GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
                                PacoIteration++;
                                iterationNumber = 1;
                            }
                        }
                        else
                        {
                            PlayerController.playerControllerInstance.PlayerDeath("Te han descubierto! \nLástima. Me caías bien. Cuidaré de tu gato. Ahora muere.");

                        }
                    }
                    break;

                case 3:
                    if (PlayerController.playerControllerInstance.hasWine)
                    {
                        if (DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "___" + iterationNumber))
                        {
                            if (PlayerController.playerControllerInstance.isMadeUp)
                            {
                                GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = false;

                                string question = DialogManager.dialogueDB[this.transform.gameObject.name + "___" + iterationNumber];
                                if (!endDialogue && DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "___" + iterNumPlus1))
                                {
                                    GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name, 3);
                                    iterationNumber++;
                                }
                                else
                                {
                                    displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "___" + iterationNumber]);
                                    GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
                                    DialogManager.modalPanel.ClosePanel();
                                    PacoIteration = 0;
                                    int slot = 0, aux = 0;
                                    foreach (Item item in Inventory.inventoryInstance.itemList) {
                                        if (item.itemType == Item.ItemType.WineBottle) slot = aux;
                                        aux++;
                                    }
                                    Inventory.inventoryInstance.RemoveItem(slot);
                                    Debug.Log("Llega a eliminar");
                                    Animator anim = this.GetComponentInChildren<Animator>();
                                    Debug.Log("Anim: " + anim.name);
                                    anim.SetTrigger("DrinkWine");
                                    anim.ResetTrigger("IsGreeting");
                                }
                                
                            }
                            else
                            {
                                PlayerController.playerControllerInstance.PlayerDeath("Te han descubierto! \nLástima. Me caías bien. Cuidaré de tu gato. Ahora muere.");

                            }
                        }
                    }
                    else
                    {
                        displayManager.DisplayMessage("No veo aquí mi vino.");

                    }
                    break;
                case 0:
                    displayManager.DisplayMessage("Qué buena cosecha!");
                    break;


            }
        }
        else 
        if (DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterationNumber))
        {
            if (PlayerController.playerControllerInstance.isMadeUp)
            {
                GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = false;

                string question = DialogManager.dialogueDB[this.transform.gameObject.name + "_" + iterationNumber];
                if (!endDialogue && DialogManager.dialogueDB.ContainsKey(this.transform.gameObject.name + "_" + iterNumPlus1))
                {
                    GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name, 3);
                    iterationNumber++;
                }
                else if (endDialogue)
                {
                    switch (selectedAnswer)
                    {
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
                }
                else
                {
                    lastDialogue = true;
                    GameObject.Find(Names.managers).GetComponent<DialogManager>().Choice(question, DialogActionA, DialogActionB, DialogActionC, lastDialogue, this.transform.gameObject.name, 3);
                    endDialogue = true;
                }
            }
            else
            {
                PlayerController.playerControllerInstance.PlayerDeath("Te han descubierto! \nLástima. Me caías bien. Cuidaré de tu gato. Ahora muere.");

            }
        }
    }

    void DialogFunctionA() {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "AR"]);
        GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
        PacoIteration++;
        Debug.Log(PacoIteration);
        iterationNumber = 1;
        lastDialogue = false;
        if (gameObject.name == "Paco")
            endDialogue = false;
        selectedAnswer = "A";
    }

    void DialogFunctionB() {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "BR"]);
        GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
        PacoIteration++;
        Debug.Log(PacoIteration);
        iterationNumber = 1;
        lastDialogue = false;
        if (gameObject.name == "Paco")
            endDialogue = false;
        selectedAnswer = "B";
    }

    void DialogFunctionC() {
        displayManager.DisplayMessage(DialogManager.dialogueDB[this.transform.gameObject.name + "_" + "CR"]);
        GameObject.Find(Names.player).GetComponent<PlayerController>().playerControl = true;
        PacoIteration++;
        Debug.Log(PacoIteration);
        iterationNumber = 1;
        lastDialogue = false;
        if (gameObject.name == "Paco")
            endDialogue = false;
        selectedAnswer = "C";
    }


}
